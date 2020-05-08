using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the currently equipped weapon if any.
/// </summary>
public class WeaponManager : MonoBehaviour{

	[Tooltip("Weapon to start the player with, if any.")]
	[SerializeField] private Weapon _weaponPrefab;
	
	/// <summary>
	/// The currently equipped weapon, null if nothing is equipped.
	/// An in-scene reference, not a prefab.
	/// </summary>
	[Tooltip("The currently equipped weapon.")]
	[SerializeField] private Weapon _equippedWeapon;

	/// <summary>
	/// Weapons that the player can equip.
	/// TODO: this might be a pre-defined array to maintain the order shown in the UI, but
	/// for now this will do.
	/// </summary>
	private SortedSet<Weapon> _availableWeapons;

	private void Start(){
		// if we set a prefab instance to start with, we'll instantiate that weapon
		if (_weaponPrefab != null){
			_equippedWeapon = Instantiate(_weaponPrefab, transform.position, Quaternion.identity, transform);
		}
	}

	public Weapon GetEquippedWeapon(){
		return _equippedWeapon;
	}

	/// <summary>
	/// Adds the following weapon to the list of available weapons to equip.
	/// This would happen when the player unlocks new weapons or finds new ones.
	/// </summary>
	public void AddAvailableWeapon(Weapon weapon){
		_availableWeapons.Add(weapon);
	}
}