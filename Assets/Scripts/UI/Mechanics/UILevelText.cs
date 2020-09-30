using Systems.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Mechanics{
	public class UILevelText : MonoBehaviour{
		private Text _levelText;

		private void Awake(){
			_levelText = GetComponent<Text>();
		}

		private void Start(){
			LevelManager.GetInstance().OnLevelStart += SetLevelText;
		}

		private void SetLevelText(int gameLevel){
			_levelText.text = gameLevel.ToString();
		}
	}
}