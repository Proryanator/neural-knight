using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class MoveToPlayAreaState : State{
		private MoveToCenterMovementPattern _moveToCenterPattern;

		public MoveToPlayAreaState(MoveToCenterMovementPattern pattern){
			_moveToCenterPattern = pattern;
		}
		
		public override void Tick(){
			_moveToCenterPattern.Move();
		}

		public override void OnEnter(){
			
		}

		public override void OnExit(){
			_moveToCenterPattern.GetComponent<EntityPlayAreaLayerChanger>().SetToInsidePlayAreaLayer();
		}
	}
}