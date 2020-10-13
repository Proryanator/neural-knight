using UnityEngine;

namespace Weapons.Ammo {
	[RequireComponent(typeof(AmmoBox))]
	public class AmmoCollectionTrigger : MonoBehaviour {

		private AmmoBox _ammoBox;

		private void Awake(){
			_ammoBox = GetComponent<AmmoBox>();
		}

		private void OnTriggerEnter2D(Collider2D other){
			WeaponManager weaponManager = other.GetComponent<WeaponManager>();

			if (weaponManager == null){
				return;
			}
			
			weaponManager.AddAmmo(_ammoBox.Ammo);

			Destroy(gameObject);
		}
	}
}