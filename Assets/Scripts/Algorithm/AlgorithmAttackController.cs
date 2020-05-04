using System;
using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls when the player takes input, and performs an attack action.
/// </summary>
[RequireComponent(typeof(WeaponManager))]
public class AlgorithmAttackController : MonoBehaviour{

	private WeaponManager _weaponManager;
	private Base2DController _base2DController;

	private void Start(){
		_weaponManager = GetComponent<WeaponManager>();
		_base2DController = GetComponent<Base2DController>();
	}

	/// <summary>
	/// Placeholder for a melee attack, should there be one.
	/// </summary>
	public void MeleeAttack(InputAction.CallbackContext context){
		// TODO: implement me to do something if we need to?
	}

	/// <summary>
	/// Fires the currently equipped weapon. Accesses the WeaponManager class to gain this information.
	/// </summary>
	public void FireWeapon(InputAction.CallbackContext context){
		Debug.Log("Player fired their weapon!");
		
		// TODO: get the actual direction to fire this projectile! How to do this without too much overhead?
		_weaponManager.GetEquippedWeapon().Fire(Vector2.up);
	}
}