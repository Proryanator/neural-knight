using UnityEngine;
using Weapons;

namespace UI.Player{
	public class WeaponIconSwapper : MonoBehaviour{
		
		private Sprite _weaponSprite;

		private void Awake(){
			_weaponSprite = GetComponent<Sprite>();
		}

		private void Start(){
			// NOTE: this is fragile, look for a better way to access this
			// pretty sure there's some design pattern where we could hook many classes together,
			// and prevent them from even knowing about each other!
			WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
			weaponManager.OnWeaponChange += UpdateWeaponImage;
		}

		private void UpdateWeaponImage(Sprite sprite){
			_weaponSprite = sprite;
		}
	}
}