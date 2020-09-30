using HealthAndDamage;
using UnityEngine;
using Utils;

namespace Weapons.Projectiles{
	/// <summary>
	/// Moves this game object a distance defined here, until a point when the object dies.
	///
	/// NOTE: details of this is intended to be set from the Weapon's properties, not the projectile.
	/// A projectile will simply represent the sprite/art asset that is fired, not be used to setup data.
	/// </summary>
	public class Projectile : MonoBehaviour{

		private WeaponProperties _weaponProperties;

		// the direction this projectile will travel when born
		private Vector2 _direction;

		// tracks when this projectile was spawned
		private float _startTime;

		/// <summary>
		/// Called by the weapon after each projectile is spawned. Sets the facing direction, as well as other information
		/// about how this projectile will perform. This keeps the configuration information about a weapon's firing rate + lifetime
		/// controlled from the weapon itself.
		/// </summary>
		public void InitProjectile(Vector2 direction, WeaponProperties weaponProperties){
			_weaponProperties = weaponProperties;
			_direction = direction.normalized;

			_startTime = Time.time;
		}

		private void Update(){
			if (Time.time - _startTime >= _weaponProperties.shotLife){
				Destroy(gameObject);
			}

			gameObject.transform.Translate(Time.deltaTime * _weaponProperties.shotSpeed * _direction);
		}

		private void OnCollisionEnter2D(Collision2D other){
			DamageShotObject(other);
		}

		/// <summary>
		/// Given the object we collided with, determine if we should damage it or not.
		/// </summary>
		private void DamageShotObject(Collision2D other){
			// is this object damageable by the projectile?
			AbstractBaseHealth abstractBaseHealth = other.gameObject.GetComponent<AbstractBaseHealth>();

			if (abstractBaseHealth != null
			    && other.gameObject.CompareTag(AllTags.ENEMY)){
				abstractBaseHealth.Damage(_weaponProperties.shotDamage);

				// apply force to the object in the direction of your fired projectile
				other.gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * _weaponProperties.shotForce, ForceMode2D.Impulse);
			}

			// always destroy yourself if you collide with something!
			Destroy(gameObject);
		}
	}
}