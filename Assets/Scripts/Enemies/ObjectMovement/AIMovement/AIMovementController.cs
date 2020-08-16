using System.Collections;
using Enemies.ObjectMovement.AIMovementPatterns;
using HealthAndDamage;
using UnityEngine;

namespace Enemies.ObjectMovement.AIMovement{
	/// <summary>
	/// Applies some pre-defined movement to the current object's movement.
	///
	/// Allows for swapping out movement types at runtime.
	/// </summary>
	[RequireComponent(typeof(MoveToCenterController))]
	public class AIMovementController : MonoBehaviour{

		private AbstractAIMovementPattern _initialAIMovementPattern;
		private AbstractAIMovementPattern _aiMovementPattern;

		[Tooltip("How much time an AI movement controller will be disabled if damaged.")] [SerializeField]
		private float _stunTime = 1f;

		private bool _isStunned = false;

		private Rigidbody2D _rigidbody2D;

		// cache the original layer of this object
		private int _originalLayer;

		// the layer to make the enemy when it's damaged, to make it fly back and not hit other enemies
		private int _enemyDamageLayer = 11;

		private void Awake(){
			// remember the initial movement pattern
			_initialAIMovementPattern = GetComponent<AbstractAIMovementPattern>();

			// we'll start with the initial movement pattern to begin with
			_aiMovementPattern = _initialAIMovementPattern;

			if (_aiMovementPattern == null){
				Debug.Log("You did not attack an AI Movement Pattern object to this game object, it won't move!");
			}

			// register for the 'OnDamageTaken', to make movement temporarily stop when damaged
			// only if you have a health script!
			AbstractBaseHealth health = GetComponent<AbstractBaseHealth>();
			if (health != null){
				health.OnDamageTaken += StopMovement;
			}

			_rigidbody2D = GetComponent<Rigidbody2D>();
			if (_rigidbody2D == null){
				Debug.LogWarning("Not able to stop any forces applied to the object.");
			}

			// save the original layer; will be modified when hit
			_originalLayer = gameObject.layer;
		}

		private void Update(){
			// simply move this object based on it's defined pattern
			_aiMovementPattern.Move();
		}

		/// <summary>
		/// Call this to attach the move to central pattern, and start using that.
		/// </summary>
		public void EnableCenterPattern(){
			_aiMovementPattern = gameObject.AddComponent<MoveToCenterMovementPattern>();
		}

		/// <summary>
		/// Calls this to restore the original movement pattern, and remove the component of center pattern.
		/// </summary>
		public void DisableCenterPattern(){
			if (_aiMovementPattern.GetType() != typeof(MoveToCenterMovementPattern)){
				Debug.LogWarning(
					"You're not supposed to call this method unless you first called 'EnableCenterPattern!'");
				return;
			}

			Destroy(_aiMovementPattern);
			RestoreOriginalMovementPattern();
		}

		/// <summary>a
		/// Restores what the original movement pattern was for this controller.
		/// </summary>
		public void RestoreOriginalMovementPattern(){
			_aiMovementPattern = _initialAIMovementPattern;

			// if this object has a rigid body, reset forces too
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
			if (rigidbody2D != null){
				rigidbody2D.velocity = Vector2.zero;
			}
		}

		/// <summary>
		/// Used to get the stun time set here; allows for synchronizing other parts of the enemy when stunned.
		/// </summary>
		/// <returns></returns>
		public float GetStunTime(){
			return _stunTime;
		}

		/// <summary>
		/// Temporarily uses the no movement controller, until the time is up.
		///
		/// TODO: perhaps we can make the stun time a method based on how much damage was applied?
		/// </summary>
		private void StopMovement(int damage){
			if (!_isStunned){
				StartCoroutine(StopAndRestoreMovement());
			}
		}

		private IEnumerator StopAndRestoreMovement(){
			_isStunned = true;
			gameObject.layer = _enemyDamageLayer;

			// set the movement pattern to the no movement pattern
			_aiMovementPattern = gameObject.AddComponent<NoMovementPattern>();
			yield return new WaitForSeconds(_stunTime);

			// now, remove this pattern, restore the original
			Destroy(_aiMovementPattern);
			RestoreOriginalMovementPattern();
			_isStunned = false;
			gameObject.layer = _originalLayer;

			// remove any forces if any were applied
			_rigidbody2D.velocity = Vector2.zero;
		}
	}
}