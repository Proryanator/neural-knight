using System;

/// <summary>
/// Holds values of how to spawn objects in the scene, the min/max rate,
/// and the min/max count.
/// </summary>
public class SpawnProperties{
	
	public float spawnSpeed{ get; set; }

	public uint spawnCount{ get; set; }

	public float minSpawnSpeed{ get; set; } = .1f;

	public float maxSpawnSpeed{ get; set; } = float.MaxValue;
	
	public uint minSpawnCount{ get; set; } = 0;
	
	public uint maxSpawnCount{ get; set; } = UInt32.MaxValue;
	
	public float spawnDelay{ get; set; } = 0f;
}