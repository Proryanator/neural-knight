using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class NormalMovementState : State{
		private AbstractMovementPattern _abstractMovementPattern;

		public NormalMovementState(AbstractMovementPattern pattern){
			_abstractMovementPattern = pattern;
		}

		public override void Tick(){
			_abstractMovementPattern.Move();
		}

		public override void OnEnter(){
		}

		public override void OnExit(){
			
		}
	}
}