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
			if (_collider2D.tag.Equals(AllTags.PLAYER)){
				_levelManager.PlayerHasExited();	
			}
		}

		public override void OnEnter(){
			
		}

		public override void OnExit(){
		}
		
		public void SetCollider2D(Collider2D other){
			_collider2D = other;
		}
	}
}