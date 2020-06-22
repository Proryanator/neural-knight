using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class to handle the spawning in of prefab objects.
///
/// For now, will randomly choose a spawn point to spawn said enemy.
/// TODO: might be able to extend this (or have a sub-class that determines how to spawn enemies)
/// to be able to spawn prefabs in waves from 1 spawn point, instead of randomly.
///
/// This is designed to spawn X number of prefabs in a given 'round' if you will, and will
/// not need to keep track of if enemies are destroyed or not.
/// </summary>
public class SpawnManager : MonoBehaviour{
	
	[Tooltip("The prefab to spawn.")]
	[SerializeField] private Transform _spawnPrefab;

	[Tooltip("The rate at which prefabs will spawn in, in seconds.")]
	[SerializeField] private float _spawnRate = 1;

	private int _currentSpawnCount = 0;
	
	[Tooltip("The maximum number of prefabs to spawn.")]
	[SerializeField] private int _maxSpawnCount = 5;

	// all spawn points in the scene, gathered upon Awake() of this script
	private SpawnPoint[] _spawnPointsInScene;

	private void Awake(){
		// cache all spawn points found within the scene
		_spawnPointsInScene = GameObject.FindObjectsOfType<SpawnPoint>();

		if (_spawnPointsInScene.Length == 0){
			Debug.LogWarning("There are no spawn points in this scene, and yet you have a Spawn Manager.");
		}
	}

	private void Start(){
		// start the spawning process!
		StartCoroutine(Spawn());
	}

	/// <summary>
	/// Will spawn prefabs at the given spawn rate.
	///
	/// Updates current count, and also updates if we've spawned too many.
	/// </summary>
	private IEnumerator Spawn(){
		while (_currentSpawnCount < _maxSpawnCount){
			// just picks 1 random spawn point and spawns an enemy there!
			Random random = new Random();
			Transform spawnPoint = _spawnPointsInScene[random.Next(_spawnPointsInScene.Length)].transform;
		
			// now, spawn the prefab at this location!
			GameObject.Instantiate(_spawnPrefab, spawnPoint.position, Quaternion.identity);
			_currentSpawnCount++;
			
			yield return new WaitForSeconds(_spawnRate);
		}
	}
}