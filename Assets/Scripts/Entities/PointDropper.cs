using Entities.Events;
using UnityEngine;
using Utils;

namespace Entities {
	public class PointDropper : MonoBehaviour {
		[SerializeField] private GameObject _pointPrefab;

		// might change how many of these point values are dropped
		private int _totalToSpawn;

		// the angle modification range possible for random movement
		// a data point can move away from the player at any angle +- this modifier
		[SerializeField] private float _angleModifier = 30f;

		[SerializeField] private float _speed = 3f;
		
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
				
				// set an initial direction away from the player to move the point objects
				Vector2 playerPosition = GameObject.FindGameObjectWithTag(AllTags.PLAYER).transform.position;
				newEntityDataPoint.GetComponent<Rigidbody2D>().velocity
					= RandomDirectionFromPlayer(newEntityDataPoint.transform.position,
						playerPosition,
						_speed);
			}
		}

		private Vector2 RandomDirectionFromPlayer(Vector2 pointStartPosition, Vector2 playerPosition, float speed){
			// calculate normalized direction away from the player
			Vector2 directionOfMovement = (pointStartPosition - playerPosition).normalized;

			return directionOfMovement * speed;
		}
	}
}