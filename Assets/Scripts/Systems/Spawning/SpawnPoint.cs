using UnityEngine;

namespace Systems.Spawning {
	public class SpawnPoint : MonoBehaviour {
		[SerializeField] private SpawnType _spawnType;
		
		public SpawnType spawnType => _spawnType;
	}
}