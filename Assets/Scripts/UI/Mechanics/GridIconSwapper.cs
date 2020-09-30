using UnityEngine;
using UnityEngine.UI;

namespace UI.Mechanics{
	public class GridIconSwapper : MonoBehaviour{
		[SerializeField] private Sprite _onSprite;
		[SerializeField] private Sprite _offSprite;

		private Image _image;

		private void Awake(){
			_image = GetComponent<Image>();
		}

		public void TurnSpriteOn(){
			_image.sprite = _onSprite;
		}
		
		public void TurnOffSprite(){
			_image.sprite = _offSprite;
		}
		
	}
}