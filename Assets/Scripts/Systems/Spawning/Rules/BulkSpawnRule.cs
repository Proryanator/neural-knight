namespace Systems.Spawning.Rules{
	/// <summary>
	/// Chooses a spawn point, and spawns 3 prefabs there.
	///
	/// Simple but, shows how you can make new rules as you'd like.
	/// </summary>
	public class BulkSpawnRule : AbstractSpawnRule{

		private int _spawnCount = 3;
	
		public override int Spawn(SpawnProperties props){
			int amountToSpawn = _spawnCount;
			if (props.inSceneCount + _spawnCount >= maxSpawnCount){
				amountToSpawn = maxSpawnCount - props.inSceneCount;
			}
		
			return SpawnAt(GetRandomSpawnPoint(), amountToSpawn, props);
		}
	}
}