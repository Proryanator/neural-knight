using UnityEngine;

public class NoneAdjuster : AbstractSpawnAdjuster{
	public override SpawnProperties AdjustSpawnProperties(SpawnProperties props, int gameLevel){
		// does not actually affect the properties!
		return props;
	}
}