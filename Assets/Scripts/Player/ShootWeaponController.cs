using System;
using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls when the player takes input, and performs an attack action.
/// </summary>
[RequireComponent(typeof(WeaponManager))]
public class ShootWeaponController : MonoBehaviour{

	private WeaponManager _weaponManager;
	private TDC_FaceMouse _faceMouseController;

	private void Start(){
		_weaponManager = GetComponent<WeaponManager>();
		_faceMouseController = GetComponent<TDC_FaceMouse>();
	}

	/// <summary>
	/// Fires the currently equipped weapon. Accesses the WeaponManager class to gain this information.
	/// </summary>
	public void FireWeapon(InputAction.CallbackContext context){
		_weaponManager.GetEquippedWeapon().Fire(transform, _faceMouseController.GetFacingDirection());
	}
}