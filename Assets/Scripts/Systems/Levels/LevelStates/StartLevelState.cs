using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class StartLevelState : State{

		private LevelManager _levelManager;
		
		public StartLevelState(LevelManager levelManager){
			_levelManager = levelManager;
		}
		
		public override void Tick(){
		}

		public override void OnEnter(){
			_levelManager.SetSpawnManagersForLevelProgression();
			
			_levelManager.IncrementLevel();
			
			// invoke any events that are attached to this event
			_levelManager.InvokeStartLevelEvent();
		}

		public override void OnExit(){
		}
	}
}