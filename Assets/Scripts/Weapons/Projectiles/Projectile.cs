﻿using UnityEngine;

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

	// how much damage this will cause to an enemy that it hits
	private int _projectileDamage;

	// how much force will be applied to enemies when the projectile lands
	private float _shotForce;
	
	/// <summary>
	/// Called by the weapon after each projectile is spawned. Sets the facing direction, as well as other information
	/// about how this projectile will perform. This keeps the configuration information about a weapon's firing rate + lifetime
	/// controlled from the weapon itself.
	/// </summary>
	/// <param name="direction">The direction this projectile will fire. Doesn't need to be normalized, we'll do that here.</param>
	/// <param name="lifeOfProjectile">How long this projectile will travel in the scene until it despawns.</param>
	/// <param name="speed">The speed at which this will translate through the world.</param>
	/// <param name="damage">The amount of damage this projectile will cause
	public void InitProjectile(Vector2 direction, float lifeOfProjectile, float speed, int damage, float shotForce){
		_direction = direction.normalized;
		_lifeOfProjectile = lifeOfProjectile;
		_speed = speed;
		_projectileDamage = damage;
		_shotForce = shotForce;

		_startTime = Time.time;
	}

	private void Update(){
		if (Time.time - _startTime >= _lifeOfProjectile){
			Destroy(gameObject);
		}

		gameObject.transform.Translate(Time.deltaTime * this._speed * _direction);
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
			abstractBaseHealth.Damage(_projectileDamage);
			
			// apply force to the object in the direction of your fired projectile
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * _shotForce, ForceMode2D.Impulse);
		}
		
		// always destroy yourself if you collide with something!
		Destroy(gameObject);
	}
}