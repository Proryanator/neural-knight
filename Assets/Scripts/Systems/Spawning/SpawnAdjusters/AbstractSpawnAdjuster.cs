using UnityEngine;

public abstract class AbstractSpawnAdjuster : ScriptableObject{

	/// <summary>
	/// Given a SpawnProperties, does something to the values, based on the current game level.
	/// </summary>
	/// <returns>An adjusted SpawnProperties object.</returns>
	public abstract SpawnProperties AdjustSpawnProperties(SpawnProperties props, int gameLevel);
	
	/// <summary>
	/// Returns you a new instance of the given spawn adjuster.
	///
	/// NOTE: it's up to the caller to cleanup their own objects before getting a new one.
	/// </summary>
	public static AbstractSpawnAdjuster GetSpawnAdjuster(SpawnAdjusterEnum adjusterEnum){
		switch (adjusterEnum){
			case SpawnAdjusterEnum.NoAdjustment:
				return ScriptableObject.CreateInstance<NoneAdjuster>();
			case SpawnAdjusterEnum.EnemyCountAndRate:
				return ScriptableObject.CreateInstance<EnemyCountAndRateAdjuster>();
			case SpawnAdjusterEnum.GoodDataCount:
				return ScriptableObject.CreateInstance<GoodDataAdjuster>();
			default:
				return null;
		}
	}
}