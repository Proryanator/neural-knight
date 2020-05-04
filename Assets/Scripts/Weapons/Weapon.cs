using System;
using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Weapon : MonoBehaviour{
	
	[Tooltip("The name of the weapon.")]
	[SerializeField] private string _weaponName = "DEFAULT WEAPON NAME";

	[Tooltip("The game object that represents the projectile for this weapon.")]
	[SerializeField] private Projectile _projectile;

	[Tooltip("The fire rate at which this weapon will fire, in seconds between fire.")]
	[SerializeField] private float _fireRate = 10f;
	
	[Tooltip("How long a shot from this weapon will live in the Unity scene.")]
	[SerializeField] private float _shotLife = 1f;

	[Tooltip("The speed at which this shot will travel in Unity.")] 
	[SerializeField] private float _shotSpeed = 1f;

	private void Start(){
		if (_projectile == null){
			Debug.Log("No game object has been set as the projectile for this weapon.");
		}
	}

	/// <summary>
	/// Fires the equipped projectile. Projectile will handle all other aspects of it's existence.
	/// <param
	/// </summary>
	public void Fire(Vector2 direction){
		// TODO: add a fire rate at which you can fire this weapon
		GameObject.Instantiate(_projectile);
		
		// set the direction of with which to fire the projectile, from the player's facing direction
		_projectile.InitProjectile(direction, _shotLife, _shotSpeed);
	}
}