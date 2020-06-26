using UnityEngine;

public class EnemyCountAndRateAdjuster : AbstractSpawnAdjuster{
	public override SpawnProperties AdjustSpawnProperties(SpawnProperties currentProps, int gameLevel){
		/*_maxSpawnCount = _initialMaxSpawnCount + (2 * gameLevel);
		
		// let's also make spawning faster every 5 levels
		int levelBoundary = 5;
		if (gameLevel % levelBoundary == 0){
			_spawnRate = _initialEnemySpawnRate - ((gameLevel / levelBoundary) * .2f);
		}

		// make sure we don't go lower (or higher) than the original spawn rate
		Mathf.Clamp(_spawnRate, _minEnemySpawnRate, _initialEnemySpawnRate);*/

		
		return currentProps;
	}
}