using System;
using UnityEngine;

/**
 * Handles objects taking damage from colliding into other types of objects.
 */
public abstract class BaseCollisionDamageHandler : MonoBehaviour{
	
	private BaseHealth _health;

	protected abstract bool TakeDamage(Collision2D other);
	
	public void OnCollisionEnter2D(Collision2D other){
		if (TakeDamage(other)){
			_health.Damage();
		}
	}
}