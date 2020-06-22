using UnityEngine;
using Random = System.Random;

/// <summary>
/// Spawns the prefab at a random spawn point; it's that simple!
/// </summary>
public class RandomSpawnRule : AbstractSpawnRule {
	
	public override void Spawn(Transform prefab, SpawnPoint[] points){
		Random random = new Random();
		SpawnAt(prefab, points[random.Next(points.Length)]);
	}
}