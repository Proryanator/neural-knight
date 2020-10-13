using Weapons;

namespace UI.Player{
	public class UIAmmoText : UISimpleTextListener{

		private void Start(){
			// TODO: how to handle this for multiple on-screen players?
			WeaponProperties props = FindObjectOfType<WeaponManager>().GetEquippedWeapon().GetWeaponProps();
			
			props.OnAmmoChange += UpdateText;
			UpdateText(props.CurrentAmmo());
		}
	}
}