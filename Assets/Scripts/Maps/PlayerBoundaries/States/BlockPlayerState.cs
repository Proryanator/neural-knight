using POCO.StateMachines;
using UnityEngine;
using Utils;

namespace Maps.PlayerBoundaries.States{
	public class BlockPlayerState : State{

		private Collider2D _collider2D;

		private bool _isWallTriggered;
		private PlayerBoundary _playerBoundary;
		
		public BlockPlayerState(PlayerBoundary playerBoundary){
			_playerBoundary = playerBoundary;
		}
		
		public override void Tick(){
			if (!_collider2D.gameObject.tag.Equals(AllTags.PLAYER)){
				return;
			}
			
			TriggerWall();
		}

		public override void OnEnter(){
		}

		public override void OnExit(){
		}
		
		private void TriggerWall(){
			if (!_isWallTriggered){
				_playerBoundary.EnableCollider();
				_isWallTriggered = true;
			}
			else{
				_playerBoundary.DisableCollider();
				_isWallTriggered = false;
			}
		}

		public void SetCollider2D(Collider2D other){
			_collider2D = other;
		}
	}
}