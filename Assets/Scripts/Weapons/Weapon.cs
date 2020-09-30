using System.Collections;
using UnityEngine;
using Weapons.Projectiles;

namespace Weapons{
	public class Weapon : MonoBehaviour{

		[Tooltip("The name of the weapon.")] [SerializeField]
		private string _weaponName = "DEFAULT WEAPON NAME";

		[SerializeField] private Sprite _weaponIcon;
		
		[Tooltip("The game object that represents the projectile for this weapon.")] [SerializeField]
		private Transform _projectile;

		[Tooltip("The fire rate at which this weapon will fire, in seconds between fire.")] [SerializeField]
		private float _fireRate = 2;

		[Tooltip("How long a shot from this weapon will live in the Unity scene.")] [SerializeField]
		private float _shotLife = 1f;

		[Tooltip("The speed at which this shot will travel in Unity.")] [SerializeField]
		private float _shotSpeed = 1f;

		[Tooltip("How much this shot will cause damage to those that it hits.")] [SerializeField]
		private int _shotDamage = 1;

		[Tooltip("How much force is applied to the enemy upon colliding.")] [SerializeField]
		private float _shotForce = 5f;

		// whether we're waiting for the next chance to fire or not, prevents spamming of input
		private bool _intervalBetweenShots = false;

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
			if (_intervalBetweenShots){
				return;
			}

			// TODO: we'll probably want to spawn these shots under something later on so it's not just all up in the scene :D
			Projectile projectile = Instantiate(_projectile, spawnTransform.position, Quaternion.identity)
				.GetComponent<Projectile>();

			// set the direction of with which to fire the projectile, from the player's facing direction
			projectile.InitProjectile(direction, _shotLife, _shotSpeed, _shotDamage, _shotForce);

			// start the wait time for the fire rate
			StartCoroutine(WaitFireRateRoutine());
		}

		public Sprite GetWeaponIcon(){
			return _weaponIcon;
		}
		
		private IEnumerator WaitFireRateRoutine(){
			_intervalBetweenShots = true;
			yield return new WaitForSeconds(_fireRate);
			_intervalBetweenShots = false;
		}
	}
}