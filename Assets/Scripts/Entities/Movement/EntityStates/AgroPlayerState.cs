using Entities.Movement.Controllers;
using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class AgroPlayerState : IState{

		private EnemyMovementController _enemyMovementController;
		private FollowPlayerPattern _followPlayerPattern;
		
		public AgroPlayerState(EnemyMovementController controller, FollowPlayerPattern pattern){
			_enemyMovementController = controller;
			_followPlayerPattern = pattern;
		}
		
		public void Tick(){
			_followPlayerPattern.Move();
		}

		public void OnEnter(){
			_enemyMovementController.SetPlayerAgro(true);
		}

		public void OnExit(){
			
		}
	}
}