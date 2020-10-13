using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class EndOfLevelState : State{

		private LevelManager _levelManager;
		
		public EndOfLevelState(LevelManager levelManager){
			_levelManager = levelManager;
		}
		
		public override void Tick(){
		}

		public override void OnEnter(){
			_levelManager.InvokeEndLevelEvent();
		}

		public override void OnExit(){
		}
	}
}