using Entities.MovementPatterns;
using UnityEngine;

namespace Entities.NewMovement{
	
	public class EntityMovementController : MonoBehaviour{

		private AbstractMovementPattern _initialMovementPattern;
		private AbstractMovementPattern _movementPatternInUse;

		private void Awake(){
			_initialMovementPattern = GetComponent<AbstractMovementPattern>();
			_movementPatternInUse = _initialMovementPattern;

			Debug.LogWarning("You did not attach any sub-class of 'AbstractMovementPattern' to this controller, therefore it will have no movement.");
		}

		private void Update(){
			CallMoveOnPatternIfNotNull(_movementPatternInUse);
		}

		private void CallMoveOnPatternIfNotNull(AbstractMovementPattern pattern){
			if (pattern != null){
				pattern.Move();
			}
		}

		public void SetMovementPattern(AbstractMovementPattern pattern){
			_movementPatternInUse = pattern;
		}

		public void RestoreInitialMovementPattern(){
			_movementPatternInUse = _initialMovementPattern;
		}
	}
}