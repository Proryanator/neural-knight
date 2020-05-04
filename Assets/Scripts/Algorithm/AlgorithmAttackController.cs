﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls when the player takes input, and performs an attack action.
/// </summary>
[RequireComponent(typeof(WeaponManager))]
public class AlgorithmAttackController : MonoBehaviour{

	private WeaponManager _weaponManager;

	private void Start(){
		_weaponManager = gameObject.GetComponent<WeaponManager>();
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
		
		// TODO: would attaching this to the same game object make this a bit flaky?
		_weaponManager.GetEquippedWeapon().Fire();
	}
}