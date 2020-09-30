using System;
using UnityEngine;

namespace Weapons{
	public class WeaponProperties : MonoBehaviour{
		[SerializeField] private float _fireRate = 2;
		[SerializeField] private float _shotLife = 1f;
		[SerializeField] private float _shotSpeed = 1f;
		[SerializeField] private int _shotDamage = 1;
		[SerializeField] private float _shotForce = 5f;
		[SerializeField] private int _maxAmmo = 100;

		private void Awake(){
			// TODO: this would be initialized as part of some setup beforehand
			_currentAmmo = _maxAmmo;
		}

		private int _currentAmmo;

		public Action<int> OnAmmoChange;
		
		public int maxAmmo => _maxAmmo;

		public float fireRate => _fireRate;

		public float shotLife => _shotLife;

		public float shotSpeed => _shotSpeed;

		public int shotDamage => _shotDamage;

		public float shotForce => _shotForce;

		public void SetMaxAmmo(int newMaxAmmo){
			_maxAmmo = newMaxAmmo;
		}
		
		public void AddAmmo(int ammo){
			_currentAmmo += ammo;
			
			// only add up to the max ammo for this weapon
			if (_currentAmmo > _maxAmmo){
				_currentAmmo = _maxAmmo;
			}

			OnAmmoChange?.Invoke(_currentAmmo);
		}

		public bool HasAmmo(){
			return _currentAmmo != 0;
		}
		
		public void DecrementAmmo(){
		_currentAmmo--;
		
		OnAmmoChange?.Invoke(_currentAmmo);
	}
	}
}