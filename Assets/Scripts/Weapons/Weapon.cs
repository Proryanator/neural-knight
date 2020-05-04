using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour{
	[Tooltip("The name of the weapon.")]
	[SerializeField] private string _weaponName = "DEFAULT WEAPON NAME";

	[Tooltip("The game object that represents the projectile for this weapon.")]
	[SerializeField] private GameObject _projectile;

	[Tooltip("The fire rate at which this weapon will fire, in seconds between fire.")]
	[SerializeField] private float _fireRate = 10f;
	
	private void Start(){
		if (_projectile == null){
			Debug.Log("No game object has been set as the projectile for this weapon.");
		}
	}

	/// <summary>
	/// Fires the equipped projectile. Projectile will handle all other aspects of it's existence.
	/// </summary>
	public void Fire(){
		// TODO: add a fire rate at which you can fire this weapon
		// GameObject.Instantiate(_projectile);
	}
}