using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons{
	/// <summary>
	/// Holds the currently equipped weapon if any.
	/// </summary>
	public class WeaponManager : MonoBehaviour{

		[Tooltip("Weapon to start the player with, if any.")]
		[SerializeField] private Weapon[] _weaponPrefabs;

		/// <summary>
		/// The currently equipped weapon, null if nothing is equipped.
		/// An in-scene reference, not a prefab.
		/// </summary>
		[Tooltip("The currently equipped weapon.")]
		[SerializeField] private Weapon _equippedWeapon;

		/// <summary>
		/// Weapons that the player can equip.
		/// </summary>
		private SortedSet<Weapon> _availableWeapons;

		public Action<Sprite> OnWeaponChange;
		
		private void Start(){
			// if we set a prefab instance to start with, we'll instantiate that weapon
			if (_weaponPrefabs != null){
				_equippedWeapon = Instantiate(_weaponPrefabs[0], transform.position, Quaternion.identity, transform);
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

		/// <summary>
		/// Intended to be called at some point, passing in a weapon object.
		///
		/// NOTE: might want to keep the ability to change weapons more inside.
		/// </summary>
		public void ChangeWeapon(Weapon weapon){
			// TODO: make it so that there's no chance that you can try to equip a weapon that you do not have
			_equippedWeapon = weapon;
			
			OnWeaponChange?.Invoke(_equippedWeapon.GetWeaponIcon());
		}
		
		private void Equip(Weapon weapon){
			_equippedWeapon = weapon;
		}

	}
}