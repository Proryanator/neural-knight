using System;
using UnityEngine;

/// <summary>
/// Holds values of how to spawn objects in the scene, the min/max rate,
/// and the min/max count.
/// </summary>
public class SpawnProperties : MonoBehaviour{

	[SerializeField] private float _spawnDelay = 0f;
	
	[SerializeField] private float _spawnSpeed;
	[SerializeField] private float _minSpawnSpeed;
	[SerializeField] private float _maxSpawnSpeed;
	
	[SerializeField] private uint _spawnCount;
	[SerializeField] private uint _minSpawnCount = 0;
	[SerializeField] private uint _maxSpawnCount = uint.MaxValue;

	public float spawnDelay{
		get => _spawnDelay;
		set => _spawnDelay = value;
	}

	public float spawnSpeed{
		get => _spawnSpeed;
		set => _spawnSpeed = value;
	}

	public float minSpawnSpeed{
		get => _minSpawnSpeed;
		set => _minSpawnSpeed = value;
	}

	public float maxSpawnSpeed{
		get => _maxSpawnSpeed;
		set => _maxSpawnSpeed = value;
	}

	public uint spawnCount{
		get => _spawnCount;
		set => _spawnCount = value;
	}

	public uint minSpawnCount{
		get => _minSpawnCount;
		set => _minSpawnCount = value;
	}

	public uint maxSpawnCount{
		get => _maxSpawnCount;
		set => _maxSpawnCount = value;
	}
}