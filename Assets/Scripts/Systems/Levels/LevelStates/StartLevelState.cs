using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class StartLevelState : IState{

		private LevelManager _levelManager;
		
		public StartLevelState(LevelManager levelManager){
			_levelManager = levelManager;
		}
		
		public void Tick(){
		}

		public void OnEnter(){
			// invoke any events that are attached to this event
			_levelManager.InvokeStartLevelEvent();

			_levelManager.IncrementLevel();
		}

		public void OnExit(){
		}
	}
}