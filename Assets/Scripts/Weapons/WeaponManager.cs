using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Ammo;
using Random = UnityEngine.Random;

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
		private List<Weapon> _availableWeapons = new List<Weapon>();

		public Action<Sprite> OnWeaponChange;
		
		private void Awake(){
			// if we set a prefab instance to start with, we'll instantiate that weapon
			if (_weaponPrefabs != null){
				Equip(Instantiate(_weaponPrefabs[0], transform.position, Quaternion.identity, transform));
				
				// also add this equipped weapon into the list of available weapons
				AddAvailableWeapon(_equippedWeapon);
			}
		}

		public Weapon GetEquippedWeapon(){
			return _equippedWeapon;
		}

		public void SetAmmoBasedOnWeaponNeeds(AmmoBox ammoBox){
			List<Weapon> weaponsThatNeedAmmo = GetWeaponsWithNonMaxAmmo();

			if (weaponsThatNeedAmmo.Count == 0){
				weaponsThatNeedAmmo = _availableWeapons;
			}
			
			Weapon weapon = GetRandomWeapon(_availableWeapons);
			ammoBox.Weapon = weapon;
			ammoBox.Ammo = weapon.GetWeaponProps().ammoBoxCount;
		}
		
		public void AddAmmo(AmmoBox ammoBox){
			Weapon weaponToAddAmmoTo = GetWeaponOfType(ammoBox.Weapon);
			
			weaponToAddAmmoTo.GetWeaponProps().AddAmmo(ammoBox.Ammo);
		}
		
		/// <summary>
		/// Adds the following weapon to the list of available weapons to equip.
		/// This would happen when the player unlocks new weapons or finds new ones.
		/// </summary>
		public void AddAvailableWeapon(Weapon weapon){
			if (_availableWeapons.Contains(weapon)){
				Debug.LogWarning("You're trying to add another weapon of the same type for some reason. Check your code");
			}
			
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

		private Weapon GetWeaponOfType(Weapon weapon){
			foreach (Weapon wiepon in _availableWeapons){
				if (wiepon.Equals(weapon)){
					return wiepon;
				}
			}

			return null;
		}
		
		private void Equip(Weapon weapon){
			_equippedWeapon = weapon;
		}

		private List<Weapon> GetWeaponsWithNonMaxAmmo(){
			List<Weapon> weaponsThatCouldUseAmmo = new List<Weapon>();
			
			foreach (var weapon in _availableWeapons){
				if (weapon.GetWeaponProps().HasSpaceForMoreAmmo()){
					weaponsThatCouldUseAmmo.Add(weapon);
				}
			}

			return weaponsThatCouldUseAmmo;
		}

		private Weapon GetRandomWeapon(List<Weapon> weapons){
			return weapons[Random.Range(0, weapons.Count)];
		}
	}
}