using Player;

namespace UI.Player {
	public class UIPlayerScoreText : UISimpleTextListener {
		public override void AttachTextListener(){
			FindObjectOfType<PlayerPoints>().OnScoreChange += UpdateText;
		}
	}
}