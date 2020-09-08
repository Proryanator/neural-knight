using System.Collections;
using Systems.PlayerAgro;
using Entities.MovementPatterns;
using HealthAndDamage;
using UnityEngine;
using Utils;

namespace Entities.Movement{
	/// <summary>
	/// Applies some pre-defined movement to the current object's movement.
	///
	/// Allows for swapping out movement types at runtime.
	/// </summary>
	public class EnemyMovementController : AbstractMovementController{

		private Rigidbody2D _rigidBody2D;

		[Tooltip("Amount of seconds that movement is disabled when enemy is hit.")]
		[SerializeField] private float _stunTime = 1f;

		[Tooltip("Toggling this on will make a check to see if agro'ing is possible, otherwise will use other movement pattern.")]
		[SerializeField] private bool _doesAgro = true;
		
		// true means this enemy is stunned, false means it's not
		private bool _isStunned = false;

		// the follow player pattern, if any is attached to this enemy
		private FollowPlayerPattern _followPlayerPattern;

		private PlayerAgroManager _playerAgroManager;

		private static int _totalEnemiesInScene = 0;

		private void Awake(){
			base.Awake();
			
			// register for the 'OnDamageTaken', to make movement temporarily stop when damaged
			// only if you have a health script!
			// NOTE: we may want to pull this down into an enemy Movement Controller variant
			AbstractBaseHealth health = GetComponent<AbstractBaseHealth>();
			if (health != null){
				health.OnDamageTaken += StopMovement;
			}
			else{
				Debug.LogWarning("This entity should have a health object for movement to be stopped properly.");
			}

			_rigidBody2D = GetComponent<Rigidbody2D>();
			if (_rigidBody2D == null){
				Debug.LogWarning("Not able to stop any forces applied to the object.");
			}

			// get the follow player pattern if you intend to use it
			if (_doesAgro){
				_followPlayerPattern = GetComponent<FollowPlayerPattern>();

				if (_followPlayerPattern == null){
					Debug.LogWarning("You did not attach a FollowPlayerPattern to this enemy, which you marked as supposed to agro the player.");
				}
			}
		}

		private void Start(){
			_playerAgroManager = PlayerAgroManager.GetInstance();
			
			_totalEnemiesInScene++;
		}

		/// <summary>
		/// A one method to call on enemy controller to start using the FollowPlayerPattern,
		/// as well as register to the PlayerAgroManager.
		/// </summary>
		private void StartAgro(){
			// register this enemy as following the player!
			// _playerAgroManager.RegisterForAgro(GetComponent<EnemyHealth>());
			
			// remove itself from listening if it was
			// playerAgroManager.StopListeningForAgroSlot(this);
			
			// we'll attach the FollowPlayerPattern and remove the other one!
			EnableFollowPlayerPattern();
		}
		
		/// <summary>
		/// Checks to see if we can agro the player, and if we can, will enable the agro.
		/// Otherwise, will set this guy up for listening.
		/// </summary>
		public void TriggerAgroIfEnemyController(){
			// entities that can agro, and are allowed to, will do so
			if (_doesAgro && _playerAgroManager.CanAgroPlayer()){
				StartAgro();
				return;
			}

			// otherwise this enemy will listen
			// _playerAgroManager.ListenForAgroSlot(this);

			// we'll want to start the initial movement if this is true
			StartInitialPattern();
		}

		public static int GetTotalEnemiesInScene(){
			return _totalEnemiesInScene;
		}

		private void OnDestroy(){
			_totalEnemiesInScene--;
		}

		/// <summary>
		/// Enables the set follow player pattern on the enemy controller
		/// </summary>
		private void EnableFollowPlayerPattern(){
			// if you added a follow player pattern, then it'll grab this
			_movementPattern = _followPlayerPattern;
			
			// also, overwrite the intial movement pattern, to be able to re-set this after damage
			OverwriteInitialPattern(_followPlayerPattern);
		}
		
		/// <summary>
		/// True if this enemy is supposed to agro to the player, false if not
		/// </summary>
		/// <returns></returns>
		private bool DoesAgro(){
			return _doesAgro;
		}

		/// <summary>
		/// Temporarily uses the no movement controller, until the time is up.
		///
		/// TODO: perhaps we can make the stun time a method based on how much damage was applied?
		/// </summary>
		private void StopMovement(int damage){
			// only call this if you're not already stunned, don't want to permanently stun an enemy
			if (!_isStunned){
				StartCoroutine(StopAndRestoreMovement());
			}
		}

		/// <summary>
		/// The enemy is considered 'stunned' at this point. Their movement is replaced with the NoMovementPattern,
		/// they're put on a layer to prevent them from colliding with other enemies, and have a force
		/// applied to them to send them flying back.
		/// </summary>
		private IEnumerator StopAndRestoreMovement(){
			_isStunned = true;
			gameObject.layer = LayerMask.NameToLayer(AllLayers.DAMAGED_ENEMY);

			// set the movement pattern to the no movement pattern
			_movementPattern = gameObject.AddComponent<NoMovementPattern>();
			yield return new WaitForSeconds(_stunTime);

			// now, remove this pattern, restore the original
			Destroy(_movementPattern);
			RestoreOriginalMovementPattern();
			_isStunned = false;
			gameObject.layer = originalLayer;

			// remove any forces if any were applied
			_rigidBody2D.velocity = Vector2.zero;
		}
	}
}