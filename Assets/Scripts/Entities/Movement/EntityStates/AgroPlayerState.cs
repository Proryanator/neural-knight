using Systems.PlayerAgro;
using Entities.Events;
using Entities.Movement.Controllers;
using Entities.MovementPatterns;
using POCO.StateMachines;

namespace Entities.Movement.EntityStates{
	public class AgroPlayerState : IState{

		private EnemyMovementController _enemyMovementController;
		private FollowPlayerPattern _followPlayerPattern;
		private PlayerAgroManager _playerAgroManager;
		
		public AgroPlayerState(EnemyMovementController controller, FollowPlayerPattern pattern){
			_enemyMovementController = controller;
			_followPlayerPattern = pattern;
			
			_playerAgroManager = PlayerAgroManager.Instance();
		}
		
		public void Tick(){
			_followPlayerPattern.Move();
		}

		public void OnEnter(){
			// if you're already agroing from the start, just skip this
			if (_enemyMovementController.IsAgroEnabledFromStart()){
				return;
			}
			
			_enemyMovementController.SetPlayerAgro(true);
			
			// register with the agro manager
			_playerAgroManager.RegisterForAgro(_enemyMovementController.GetComponent<DeSpawnable>());
			
			// stop listening!
			_playerAgroManager.StopListeningForAgroSlot(_enemyMovementController);
		}

		public void OnExit(){
			
		}
	}
}