using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public abstract class UISimpleTextListener : MonoBehaviour {
		private Text _text;

		protected void Awake(){
			_text = GetComponent<Text>();
		}
		
		protected void UpdateText(int value){
			_text.text = value.ToString();
		}
	}
}