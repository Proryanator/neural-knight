using UnityEngine;
using Weapons;

namespace UI.Mechanics{
	public class WeaponIconSwapper : MonoBehaviour{
		
		private Sprite _weaponSprite;

		private void Awake(){
			_weaponSprite = GetComponent<Sprite>();
		}

		private void Start(){
			// NOTE: this is fragile, look for a better way to access this
			WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
			weaponManager.OnWeaponChange += UpdateWeaponImage;
		}

		private void UpdateWeaponImage(Sprite sprite){
			_weaponSprite = sprite;
		}
	}
}