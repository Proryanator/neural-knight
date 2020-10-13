using Systems.PlayerAgro;
using Entities.Movement.Controllers;
using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class WaitingForAgroState : State{
		
		private EnemyMovementController _enemyMovementController;
		private AbstractMovementPattern _abstractMovementPattern;

		public WaitingForAgroState(AbstractMovementPattern pattern, EnemyMovementController controller){
			_abstractMovementPattern = pattern;
			_enemyMovementController = controller;
		}
		
		public override void Tick(){
			_abstractMovementPattern.Move();
		}

		public override void OnEnter(){
			// we might actually leave this right after we start
			if (PlayerAgroManager.Instance().CanAgroPlayer()){
				_enemyMovementController.EnableAgro();
			}
			
			_enemyMovementController.ListenForAgroSlot();
		}

		public override void OnExit(){
			PlayerAgroManager.Instance().StopListeningForAgroSlot(_enemyMovementController);
		}
	}
}