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
		private Vector2 _colliderPosition;
		private bool _hasEntered;

		// tick is only called on collision enter/exit, so we can make sure this is on an exit call only
		public override void Tick(){
			// if the player has already left, do nothing
			if (_levelManager.HasPlayerExitedTheRoom()){
				return;
			}

			// now, if this is the player, trigger the transition
			if (_collider2D.tag.Equals(AllTags.PLAYER) && !_hasEntered && IsPlayerFartherAwayThanCollider()){
				_levelManager.PlayerHasExited();	
			}
		}

		public override void OnEnter(){
			// disable all triggers, no longer in a blocking state
			PlayerBoundary.DisableAllPlayerBoundaries();
		}

		public override void OnExit(){
		}
		
		public void SetCollider2D(Collider2D other, bool hasEntered, Vector2 colliderPosition){
			_collider2D = other;
			_hasEntered = hasEntered;
			_colliderPosition = colliderPosition;
		}

		private bool IsPlayerFartherAwayThanCollider(){
			return Vector2.Distance(_collider2D.transform.position, Vector2.zero) >
			       Vector2.Distance(_colliderPosition, Vector2.zero);
		}
	}
}