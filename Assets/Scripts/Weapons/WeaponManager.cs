using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the currently equipped weapon if any.
/// </summary>
public class WeaponManager : MonoBehaviour{
	
	/// <summary>
	/// The currently equipped weapon, null if nothing is equipped.
	/// </summary>
	[Tooltip("The currently equipped weapon.")]
	[SerializeField] private Weapon _equippedWeapon;

	/// <summary>
	/// Weapons that the player can equip.
	/// TODO: this might be a pre-defined array to maintain the order shown in the UI, but
	/// for now this will do.
	/// </summary>
	private SortedSet<Weapon> _availableWeapons;

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