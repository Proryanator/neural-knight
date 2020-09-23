using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class NormalMovementState : IState{
		private AbstractMovementPattern _abstractMovementPattern;

		public NormalMovementState(AbstractMovementPattern pattern){
			_abstractMovementPattern = pattern;
		}

		public void Tick(){
			_abstractMovementPattern.Move();
		}

		public void OnEnter(){
		}

		public void OnExit(){
			
		}
	}
}