using Systems.Levels;
using POCO.StateMachines;

namespace Maps.PlayerBoundaries.States{
	public class NotifyLevelManagerState : State{

		public NotifyLevelManagerState(LevelManager levelManager){
			_levelManager = levelManager;
		}

		private LevelManager _levelManager;
		
		public override void Tick(){
			
		}

		public override void OnEnter(){
			_levelManager.PlayerHasExited();
		}

		public override void OnExit(){
		}
	}
}