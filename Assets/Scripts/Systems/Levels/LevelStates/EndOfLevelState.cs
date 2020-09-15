using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class EndOfLevelState : IState{

		private LevelManager _levelManager;
		
		public EndOfLevelState(LevelManager levelManager){
			_levelManager = levelManager;
		}
		
		public void Tick(){
		}

		public void OnEnter(){
			_levelManager.InvokeEndLevelEvent();
		}

		public void OnExit(){
		}
	}
}