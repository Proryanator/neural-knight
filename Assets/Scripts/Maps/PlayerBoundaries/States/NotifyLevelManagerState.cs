using Systems.Levels;
using POCO.StateMachines;
using UnityEngine;
using Utils;

namespace Maps.PlayerBoundaries.States{
	public class NotifyLevelManagerState : State{

		public NotifyLevelManagerState(LevelManager levelManager){
			_levelManager = levelManager;
		}

		private LevelManager _levelManager;
		private Collider2D _collider2D;
		
		public override void Tick(){
			// if the player has already left, do nothing
			if (_levelManager.HasPlayerExitedTheRoom()){
				return;
			}
			
			// otherwise, if this is the player, we'll want to 
			if (_collider2D.tag.Equals(AllTags.PLAYER)){
				_levelManager.PlayerHasExited();	
			}
		}

		public override void OnEnter(){
			// disable all triggers, no longer in a blocking state
			PlayerBoundary.DisableAllPlayerBoundaries();
		}

		public override void OnExit(){
		}
		
		public void SetCollider2D(Collider2D other){
			_collider2D = other;
		}
	}
}