using UnityEngine;
using Random = System.Random;

/// <summary>
/// Spawns the prefab at a random spawn point; it's that simple!
/// </summary>
public class RandomSpawnRule : AbstractSpawnRule {

	public override int Spawn(int currentSpawnCount){
		return SpawnAt(GetRandomSpawnPoint(), 1);
	}
}