namespace Systems.Spawning.Rules{
	/// <summary>
	/// Spawns the prefab at a random spawn point; it's that simple!
	/// </summary>
	public class RandomSpawnRule : AbstractSpawnRule {

		public override int Spawn(SpawnProperties props){
			return SpawnAt(GetRandomSpawnPoint(), 1, props);
		}
	}
}