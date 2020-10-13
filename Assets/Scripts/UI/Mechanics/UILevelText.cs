using Systems.Levels;

namespace UI.Mechanics{
	public class UILevelText : UISimpleTextListener{
		
		private void Start(){
			LevelManager.GetInstance().OnLevelStart += UpdateText;
		}
	}
}