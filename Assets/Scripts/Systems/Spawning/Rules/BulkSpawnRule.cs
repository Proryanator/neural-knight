/// <summary>
/// Chooses a spawn point, and spawns 3 prefabs there.
///
/// Simple but, shows how you can make new rules as you'd like.
/// </summary>
public class BulkSpawnRule : AbstractSpawnRule{

	private int _spawnCount = 3;
	
	public override int Spawn(int currentSpawnCount){
		int amountToSpawn = _spawnCount;
		if (currentSpawnCount + _spawnCount >= maxSpawnCount){
			amountToSpawn = maxSpawnCount - currentSpawnCount;
		}
		
		return SpawnAt(GetRandomSpawnPoint(), amountToSpawn);
	}
}