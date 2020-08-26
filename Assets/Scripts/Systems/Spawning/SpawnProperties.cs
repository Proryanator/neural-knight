using UnityEngine;

namespace Systems.Spawning{
	/// <summary>
	/// Holds values of how to spawn objects in the scene, the min/max rate,
	/// and the min/max count.
	/// </summary>
	public class SpawnProperties : MonoBehaviour{

		[Tooltip("How long to delay starting to spawn this type")]
		[SerializeField] private float _spawnDelay = 0f;
	
		[Tooltip("How fast to currently spawn objects")]
		[SerializeField] private float _spawnSpeed;
		
		[Tooltip("The slowest spawn speed allowed")]
		[SerializeField] private float _minSpawnSpeed;
		
		[Tooltip("The farthest/largest")]
		[SerializeField] private float _maxSpawnSpeed;
		
		// how many units of a certain type exist in the scene
		[SerializeField] private int _inSceneCount = 0;

		// how many entities have been spawned this level
		[SerializeField] private int _totalSpawnedThisLevel = 0;
		
		[Tooltip("The maximum number of a unit allowed in the scene at once.")]
		[SerializeField] private int sceneLimitLimit = 10;

		[Tooltip("Total number of this entity that will be spawned, even if it's greater than the max allowed in the scene")]
		[SerializeField] private int _totalToSpawn = int.MaxValue;
		
		public float spawnDelay{
			get => _spawnDelay;
			set => _spawnDelay = value;
		}

		public float spawnSpeed{
			get => _spawnSpeed;
			set => _spawnSpeed = value;
		}

		public float minSpawnSpeed{
			get => _minSpawnSpeed;
			set => _minSpawnSpeed = value;
		}

		public float maxSpawnSpeed{
			get => _maxSpawnSpeed;
			set => _maxSpawnSpeed = value;
		}

		public int inSceneCount{
			get => _inSceneCount;
			set => _inSceneCount = value;
		}

		public int totalSpawnedThisLevel{
			get => _totalSpawnedThisLevel;
			set => _totalSpawnedThisLevel = value;
		}
		
		public int sceneLimit{
			get => sceneLimitLimit;
			set => sceneLimitLimit = value;
		}

		public int totalToSpawn{
			get => _totalToSpawn;
			set => _totalToSpawn = value;
		}

		/// <summary>
		/// Zero out all counters for preparation of a new level.
		/// </summary>
		public void ResetCounters(){
			_inSceneCount = 0;
			_totalSpawnedThisLevel = 0;
		}
		
		/// <summary>
		/// Increment scene counter, as well as total counter.
		/// </summary>
		public void IncrementSpawnCount(int amount){
			_inSceneCount+=amount;
			_totalSpawnedThisLevel+=amount;
		}

		/// <summary>
		/// Call this when an entity is destroyed.
		/// </summary>
		public void DecrementSceneCount(){
			_inSceneCount--;
		}
		
		/// <summary>
		/// Clamps the spawn speed to be between the set limits.
		/// </summary>
		public void ClampSpawnSpeed(){
			spawnSpeed = Mathf.Clamp(spawnSpeed, minSpawnSpeed, maxSpawnSpeed);
		}
	}
}