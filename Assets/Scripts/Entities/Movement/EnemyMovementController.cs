using System.Collections;
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
		
		// true means this enemy is stunned, false means it's not
		private bool _isStunned = false;
		
		// cache the original layer of this object; used to restore it's layer after taking damage
		private int _originalLayer;

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

			// save the original layer; will be modified when hit
			_originalLayer = gameObject.layer;
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
			movementPattern = gameObject.AddComponent<NoMovementPattern>();
			yield return new WaitForSeconds(_stunTime);

			// now, remove this pattern, restore the original
			Destroy(movementPattern);
			RestoreOriginalMovementPattern();
			_isStunned = false;
			gameObject.layer = _originalLayer;

			// remove any forces if any were applied
			_rigidBody2D.velocity = Vector2.zero;
		}
	}
}