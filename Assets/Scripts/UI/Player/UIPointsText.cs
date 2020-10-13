using Player;

namespace UI.Player {
	public class UIPointsText : UISimpleTextListener {

		public override void AttachTextListener(){
			// TODO: how to handle this for multiple on-screen players?
			FindObjectOfType<PlayerPoints>().OnScoreChange += UpdateText;
		}
	}
}