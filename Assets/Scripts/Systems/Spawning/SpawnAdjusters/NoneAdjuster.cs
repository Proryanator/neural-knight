using UnityEngine;

public class NoneAdjuster : AbstractSpawnAdjuster{
	public override SpawnProperties AdjustSpawnProperties(SpawnProperties currentProps, int gameLevel){
		// does not actually affect the properties!
		return currentProps;
	}
}