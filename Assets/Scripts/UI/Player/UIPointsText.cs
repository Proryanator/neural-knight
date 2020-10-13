using Player;

namespace UI.Player {
	public class UIPointsText : UISimpleTextListener {
		
		private void Start(){
			// TODO: how to handle this for multiple on-screen players?
			FindObjectOfType<PlayerPoints>().OnScoreChange += UpdateText;
		}
	}
}