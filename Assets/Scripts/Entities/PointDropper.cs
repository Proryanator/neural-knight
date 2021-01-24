using Entities.Events;
using UnityEngine;
using Utils;

namespace Entities {
	public class PointDropper : MonoBehaviour {
		[SerializeField] private GameObject _pointPrefab;

		// might change how many of these point values are dropped
		private int _totalToSpawn;
		
		private void Awake(){
			// attach the spawn method to the despawn method of it's parent
			GetComponent<DeSpawnable>().OnDeSpawn += SpawnPoints;
			
			_totalToSpawn = Random.Range(1, 5);
		}

		private void SpawnPoints(){
			for (int i = 0; i < _totalToSpawn; i++){
				GameObject newEntityDataPoint = Instantiate(_pointPrefab, transform.position, Quaternion.identity);
				// parent to the room object so that it disappears when player's leave the map
				newEntityDataPoint.transform.SetParent(GameObject.FindGameObjectWithTag(AllTags.ROOM).transform);	
			}
		}
	}
}