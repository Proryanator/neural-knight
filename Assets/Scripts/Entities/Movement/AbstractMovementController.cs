using Entities.MovementPatterns;
using UnityEngine;

namespace Entities.Movement{
	[RequireComponent(typeof(MoveToCenterController))]
	public abstract class AbstractMovementController : MonoBehaviour{
		// the initially set movement pattern of this prefab
		private AbstractMovementPattern initialMovementPattern;
		
		// the current movement pattern being used right now
		protected AbstractMovementPattern movementPattern;

		protected void Awake(){
			// remember the initial movement pattern
			initialMovementPattern = GetComponent<AbstractMovementPattern>();
			movementPattern = initialMovementPattern;

			if (movementPattern == null){
				Debug.Log("You did not attach an AI Movement Pattern object to this game object, it won't move!");
			}
		}

		/// <summary>
		/// Call this to attach the move to central pattern, and start using that.
		/// </summary>
		public void EnableCenterPattern(){
			movementPattern = gameObject.AddComponent<MoveToCenterMovementPattern>();
		}

		/// <summary>
		/// Calls this to restore the original movement pattern, and remove the component of center pattern.
		/// </summary>
		public void DisableCenterPattern(){
			if (movementPattern.GetType() != typeof(MoveToCenterMovementPattern)){
				Debug.LogWarning(
					"You're not supposed to call this method unless you first called 'EnableCenterPattern!'");
				return;
			}

			Destroy(movementPattern);
			RestoreOriginalMovementPattern();
		}

		/// <summary>a
		/// Restores what the original movement pattern was for this controller.
		/// </summary>
		public void RestoreOriginalMovementPattern(){
			movementPattern = initialMovementPattern;

			// if this object has a rigid body, reset forces too
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
			if (rigidbody2D != null){
				rigidbody2D.velocity = Vector2.zero;
			}
		}
	}
}