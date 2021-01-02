using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class WaitForPlayerToLeaveRoomState : State{

		private LevelManager _levelManager;

		private bool _hasPlayerExitedTheRoom;
		
		public WaitForPlayerToLeaveRoomState(LevelManager levelManager){
			_levelManager = levelManager;
		}
		
		public override void Tick(){
		}

		public override void OnEnter(){
			_levelManager.InvokeEndLevelEvent();
		}

		public override void OnExit(){
			_hasPlayerExitedTheRoom = false;
		}

		public void PlayerHasExited(){
			_hasPlayerExitedTheRoom = true;
		}

		public bool HasPlayerExitedTheRoom(){
			return _hasPlayerExitedTheRoom;
		}
	}
}