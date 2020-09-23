using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class MoveToPlayAreaState : IState{
		private MoveToCenterMovementPattern _moveToCenterPattern;

		public MoveToPlayAreaState(MoveToCenterMovementPattern pattern){
			_moveToCenterPattern = pattern;
		}
		
		public void Tick(){
			_moveToCenterPattern.Move();
		}

		public void OnEnter(){
			
		}

		public void OnExit(){
			_moveToCenterPattern.GetComponent<EntityPlayAreaLayerChanger>().SetToInsidePlayAreaLayer();
		}
	}
}