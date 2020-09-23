using Systems.PlayerAgro;
using Entities.Movement.Controllers;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class WaitingForAgroState : IState{
		
		private EnemyMovementController _enemyMovementController;

		public WaitingForAgroState(EnemyMovementController controller){
			_enemyMovementController = controller;
		}
		
		public void Tick(){
		}

		public void OnEnter(){
			_enemyMovementController.ListenForAgroSlot();
		}

		public void OnExit(){
			PlayerAgroManager.Instance().StopListeningForAgroSlot(_enemyMovementController);
		}
	}
}