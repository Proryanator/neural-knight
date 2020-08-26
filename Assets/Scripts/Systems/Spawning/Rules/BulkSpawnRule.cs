namespace Systems.Spawning.Rules{
	/// <summary>
	/// Chooses a spawn point, and spawns 3 prefabs there.
	///
	/// Simple but, shows how you can make new rules as you'd like.
	/// </summary>
	public class BulkSpawnRule : AbstractSpawnRule{

		private uint _spawnCount = 3;
	
		public override uint Spawn(SpawnProperties props){
			uint amountToSpawn = _spawnCount;
			if (props.inSceneCount + _spawnCount >= maxSpawnCount){
				amountToSpawn = maxSpawnCount - props.inSceneCount;
			}
		
			return SpawnAt(GetRandomSpawnPoint(), amountToSpawn, props);
		}
	}
}