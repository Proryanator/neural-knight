using System.Collections;
using UnityEngine;
using Weapons.Projectiles;

namespace Weapons{
	[RequireComponent(typeof(WeaponProperties))]
	public class Weapon : MonoBehaviour{
		
		[SerializeField] private string _weaponName = "DEFAULT WEAPON NAME";

		[SerializeField] private Sprite _weaponIcon;
		
		[SerializeField] private Transform _projectile;

		private WeaponProperties _weaponProperties;

		// whether we're waiting for the next chance to fire or not, prevents spamming of input
		private bool _intervalBetweenShots = false;

		private void Awake(){
			_weaponProperties = GetComponent<WeaponProperties>();
		}

		private void Start(){
			if (_projectile == null){
				Debug.Log("No game object has been set as the projectile for this weapon.");
			}
		}

		/// <summary>
		/// Fires the equipped projectile. Projectile will handle all other aspects of it's existence.
		/// <param
		/// </summary>
		public void Fire(Transform spawnTransform, Vector2 direction){
			if (_intervalBetweenShots || !_weaponProperties.HasAmmo()){
				return;
			}
			
			Projectile projectile = Instantiate(_projectile, spawnTransform.position, Quaternion.identity)
				.GetComponent<Projectile>();

			// set the direction of with which to fire the projectile, from the player's facing direction
			projectile.InitProjectile(direction, _weaponProperties);

			// decrement ammo from the weapon!
			_weaponProperties.DecrementAmmo();
			
			// start the wait time for the fire rate
			StartCoroutine(WaitFireRateRoutine());
		}

		public Sprite GetWeaponIcon(){
			return _weaponIcon;
		}
		
		private IEnumerator WaitFireRateRoutine(){
			_intervalBetweenShots = true;
			yield return new WaitForSeconds(_weaponProperties.fireRate);
			_intervalBetweenShots = false;
		}
	}
}