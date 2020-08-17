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
	[RequireComponent(typeof(MoveToCenterController))]
	public class AIMovementController : MonoBehaviour{

		// the initially set movement pattern when this object loads
		// this is saved so you can return this object back to it's original set movement pattern
		private AbstractAIMovementPattern _initialAIMovementPattern;
		
		// the current movement pattern being used right now
		private AbstractAIMovementPattern _aiMovementPattern;

		private Rigidbody2D _rigidBody2D;

		[Tooltip("Amount of seconds that movement is disabled when enemy is hit.")]
		[SerializeField] private float _stunTime = 1f;
		
		// true means this enemy is stunned, false means it's not
		private bool _isStunned = false;
		
		// cache the original layer of this object; used to restore it's layer after taking damage
		private int _originalLayer;

		private void Awake(){
			// remember the initial movement pattern
			_initialAIMovementPattern = GetComponent<AbstractAIMovementPattern>();
			_aiMovementPattern = _initialAIMovementPattern;

			if (_aiMovementPattern == null){
				Debug.Log("You did not attach an AI Movement Pattern object to this game object, it won't move!");
			}

			// register for the 'OnDamageTaken', to make movement temporarily stop when damaged
			// only if you have a health script!
			// NOTE: we may want to pull this down into an enemy Movement Controller variant
			AbstractBaseHealth health = GetComponent<AbstractBaseHealth>();
			if (health != null){
				health.OnDamageTaken += StopMovement;
			}

			_rigidBody2D = GetComponent<Rigidbody2D>();
			if (_rigidBody2D == null){
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
			gameObject.layer = LayerMask.NameToLayer(AllLayers.DAMAGED_ENEMY);

			// set the movement pattern to the no movement pattern
			_aiMovementPattern = gameObject.AddComponent<NoMovementPattern>();
			yield return new WaitForSeconds(_stunTime);

			// now, remove this pattern, restore the original
			Destroy(_aiMovementPattern);
			RestoreOriginalMovementPattern();
			_isStunned = false;
			gameObject.layer = _originalLayer;

			// remove any forces if any were applied
			_rigidBody2D.velocity = Vector2.zero;
		}
	}
}