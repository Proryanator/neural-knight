using System;
using Systems;
using UnityEngine;

/// <summary>
/// Moves this game object a distance defined here, until a point when the object dies.
///
/// NOTE: details of this is intended to be set from the Weapon's properties, not the projectile.
/// A projectile will simply represent the sprite/art asset that is fired, not be used to setup data.
/// </summary>
public class Projectile : MonoBehaviour{

	// speed at which this projectile travels
	private float _speed;
	
	// The time at which this object will live in the scene, in seconds
	private float _lifeOfProjectile;

	// the direction this projectile will travel when born
	private Vector2 _direction;

	// tracks when this projectile was spawned
	private float _startTime;
	
	/// <summary>
	/// Called by the weapon after each projectile is spawned. Sets the facing direction, as well as other information
	/// about how this projectile will perform. This keeps the configuration information about a weapon's firing rate + lifetime
	/// controlled from the weapon itself.
	/// </summary>
	/// <param name="direction">The direction this projectile will fire. Doesn't need to be normalized, we'll do that here.</param>
	/// <param name="lifeOfProjectile">How long this projectile will travel in the scene until it despawns.</param>
	/// <param name="speed">The speed at which this will translate through the world.</param>
	public void InitProjectile(Vector2 direction, float lifeOfProjectile, float speed){
		_direction = direction.normalized;
		_lifeOfProjectile = lifeOfProjectile;
		_speed = speed;

		_startTime = Time.time;
	}

	private void Update(){
		if (Time.time - _startTime >= _lifeOfProjectile){
			Destroy(gameObject);
		}

		gameObject.transform.Translate(_direction * _speed);
	}

	private void OnCollisionEnter2D(Collision2D other){
		DamageShotObject(other);
	}

	/// <summary>
	/// Given the object we collided with, determine if we should damage it or not.
	/// </summary>
	private void DamageShotObject(Collision2D other){
		// is this object damageable by the projectile?
		BaseHealth baseHealth = other.gameObject.GetComponent<BaseHealth>();
		
		if (baseHealth != null 
		    && other.gameObject.CompareTag(AllTags.ENEMY)){
			baseHealth.Damage();
			
			// let's say it always will de spawn upon hitting something else (what if it's you?)
			Destroy(gameObject);
		}
	}
}