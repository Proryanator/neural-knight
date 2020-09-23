using Systems.PlayerAgro;
using Entities.Movement.Controllers;
using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class WaitingForAgroState : IState{
		
		private EnemyMovementController _enemyMovementController;
		private AbstractMovementPattern _abstractMovementPattern;

		public WaitingForAgroState(AbstractMovementPattern pattern, EnemyMovementController controller){
			_abstractMovementPattern = pattern;
			_enemyMovementController = controller;
		}
		
		public void Tick(){
			_abstractMovementPattern.Move();
		}

		public void OnEnter(){
			_enemyMovementController.ListenForAgroSlot();
		}

		public void OnExit(){
			PlayerAgroManager.Instance().StopListeningForAgroSlot(_enemyMovementController);
		}
	}
}