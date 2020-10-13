using Entities.Events;
using UnityEngine;

namespace Weapons.Ammo {
	public class AmmoDropper : MonoBehaviour {
		[Range(0, 1)] 
		[SerializeField] private float _chanceToDrop = .5f;

		[SerializeField] private GameObject _ammoBoxPrefab;
		
		private void Awake(){
			// attach the spawn method to the despawn method of it's parent
			GetComponent<DeSpawnable>().OnDeSpawn += SpawnAmmo;
		}

		private void SpawnAmmo(){
			if (Random.Range(0f, 1f) <= _chanceToDrop){
				Instantiate(_ammoBoxPrefab, transform.position, Quaternion.identity);
			}
		}
	}
}