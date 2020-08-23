using Entities.MovementPatterns;
using UnityEngine;

namespace Entities.Movement{
	[RequireComponent(typeof(PlayAreaEntryController))]
	public abstract class AbstractMovementController : MonoBehaviour{
		// the initially set movement pattern of this prefab
		// NOTE: for entities that follow the player, this is it's non-player following pattern
		private AbstractMovementPattern _initialMovementPattern;
		
		// the current movement pattern being used right now
		protected AbstractMovementPattern _movementPattern;

		protected void Awake(){
			// remember the initial movement pattern
			AbstractMovementPattern[] patterns = GetComponents<AbstractMovementPattern>();
			// we do not want to store the follow player pattern, this one is special and only for enemies

			foreach (AbstractMovementPattern pattern in patterns){
				if (pattern is FollowPlayerPattern){
					// if it's the follow player pattern, don't use this as the initial pattern
				}
				else{
					_initialMovementPattern = pattern;
					break;
				}
			}
			
			_movementPattern = _initialMovementPattern;

			if (_movementPattern == null){
				Debug.Log("Either a movement pattern is missing, or if this is an enemy controller you did not attach an initial movement pattern!");
			}
		}

		private void Update(){
			// simply move this object based on it's defined pattern
			_movementPattern.Move();
		}

		/// <summary>
		/// Call this in special circumstances, when the initial pattern needs to be changed.
		/// </summary>
		protected void OverwriteInitialPattern(AbstractMovementPattern pattern){
			_initialMovementPattern = pattern;
		}

		protected AbstractMovementPattern GetInitialPattern(){
			return _initialMovementPattern;
		}
		
		/// <summary>
		/// Call this to attach the move to central pattern, and start using that.
		/// </summary>
		public void EnableCenterPattern(){
			_movementPattern = gameObject.AddComponent<MoveToCenterMovementPattern>();
		}

		/// <summary>
		/// Call this to start using the originally set movement pattern!
		/// </summary>
		public void StartInitialPattern(){
			// if your current movement pattern is anything other than the center movement pattern, just skip this
			if (_movementPattern.GetType() != typeof(MoveToCenterMovementPattern)){
				return;
			}

			Destroy(_movementPattern);
			RestoreOriginalMovementPattern();
		}

		/// <summary>a
		/// Restores what the original movement pattern was for this controller.
		/// </summary>
		protected void RestoreOriginalMovementPattern(){
			_movementPattern = _initialMovementPattern;

			// if this object has a rigid body, reset forces too
			Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
			if (rigidbody2D != null){
				rigidbody2D.velocity = Vector2.zero;
			}

			Debug.Log("Restoring original movement");
		}
	}
}