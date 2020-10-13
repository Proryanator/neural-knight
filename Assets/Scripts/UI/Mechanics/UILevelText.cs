using Systems.Levels;

namespace UI.Mechanics{
	public class UILevelText : UISimpleTextListener{

		public override void AttachTextListener(){
			LevelManager.GetInstance().OnLevelStart += UpdateText;
		}
	}
}