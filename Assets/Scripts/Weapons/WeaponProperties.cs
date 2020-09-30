using UnityEngine;

namespace Weapons{
	public class WeaponProperties : MonoBehaviour{
		[SerializeField] private float _fireRate = 2;

		[SerializeField] private float _shotLife = 1f;
		
		[SerializeField] private float _shotSpeed = 1f;
		
		[SerializeField] private int _shotDamage = 1;
		
		[SerializeField] private float _shotForce = 5f;
		
		public float fireRate => _fireRate;

		public float shotLife => _shotLife;

		public float shotSpeed => _shotSpeed;

		public int shotDamage => _shotDamage;

		public float shotForce => _shotForce;
	}
}