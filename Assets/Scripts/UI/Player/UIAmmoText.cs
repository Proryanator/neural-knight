using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI.Player{
	public class UIAmmoText : MonoBehaviour{
		private Text _ammoText;

		private void Awake(){
			_ammoText = GetComponent<Text>();
		}

		private void Start(){
			// TODO: how to handle this for multiple on-screen players?
			FindObjectOfType<WeaponManager>().GetEquippedWeapon().GetWeaponProps().OnAmmoChange += SetAmmoCount;
		}

		private void SetAmmoCount(int ammoCount){
			_ammoText.text = ammoCount.ToString();
		}
	}
}