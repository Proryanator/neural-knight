namespace Systems.Spawning.SpawnAdjusters{
	public class EnemyCountAndRateAdjuster : AbstractSpawnAdjuster{
		public override SpawnProperties AdjustSpawnProperties(SpawnProperties props, int gameLevel){
			props.sceneLimit += (uint)(2 * gameLevel);

			// let's also make spawning faster every 5 levels
			// TODO: how could we configure this from the inspector?
			int levelBoundary = 3;
			if (gameLevel % levelBoundary == 0){
				props.spawnSpeed -= ((gameLevel / levelBoundary) * .2f);
			}

			// make sure we don't go lower (or higher) than the original spawn rate
			props.ClampSpawnSpeed();

			return props;
		}
	}
}