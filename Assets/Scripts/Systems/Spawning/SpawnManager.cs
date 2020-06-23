using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class to handle the spawning in of prefab objects.
///
/// Based on the chosen spawn rule, will spawn objects accordingly.
///
/// Subscribes to the level manager's method; when a level changes, the spawn
/// values will be set for this manager accordingly.
/// </summary>
public class SpawnManager : MonoBehaviour{
	
	[Tooltip("The prefab to spawn.")]
	[SerializeField] private Transform _spawnPrefab;

	// TODO: should this also be controlled by the LevelManager?
	[Tooltip("Makes spawn manager wait this amount of seconds before starting it's spawn process. Waits only once.")]
	[SerializeField] private float _spawnDelay = 0f;
	
	// the rate at which prefabs will spawn in, in seconds
	private float _spawnRate = -1f;

	// the maximum number of prefabs to spawn
	private int _maxSpawnCount = -1;

	[Tooltip("Choose the spawn rule to use for this spawner.")]
	[SerializeField] private SpawnRuleEnum spawnRuleEnum = SpawnRuleEnum.Random;
	
	private int _currentSpawnCount = 0;
	
	// all spawn points in the scene, gathered upon Awake() of this script
	private SpawnPoint[] _spawnPointsInScene;

	// unless changed, defaults to the random rule
	private AbstractSpawnRule _currentSpawnRule;
	
	private void Awake(){
		// cache all spawn points found within the scene
		_spawnPointsInScene = GameObject.FindObjectsOfType<SpawnPoint>();

		if (_spawnPointsInScene.Length == 0){
			Debug.LogWarning("There are no spawn points in this scene, and yet you have a Spawn Manager.");
		}
		
		// subscribe to the level starting method; this is what initiates/restarts spawning!
		LevelManager.GetInstance().OnLevelStart += StartSpawning;
	}

	/// <summary>
	/// Starts the spawning. Intended to be setup externally!
	/// </summary>
	private void StartSpawning(float spawnRate, int maxCount){
		// setup spawn rules
		SetSpawnRule();

		if (CanSpawn()){
			Debug.LogWarning("You're attempting to restart spawning while spawning is already happening!");
			return;
		}
		
		// set max count, restart counter, and start co routine!
		_maxSpawnCount = maxCount;
		_currentSpawnCount = 0;
		_spawnRate = spawnRate;
		StartCoroutine(Spawn());
	}
	
	/// <summary>
	/// Using the set SpawnRule, set the rule object.
	/// If one was already set, destroys that one and re-instantiates one.
	///
	/// Intended to be called from outside the spawn manager to change this at runtime.
	/// </summary>
	private void SetSpawnRule(){
		// destroy old rule if it existed
		if (_currentSpawnRule != null){
			ScriptableObject.Destroy(_currentSpawnRule);
		}

		_currentSpawnRule = AbstractSpawnRule.GetRule(spawnRuleEnum, _spawnPrefab, _spawnPointsInScene, _maxSpawnCount);
	}

	private bool CanSpawn(){
		return _currentSpawnCount < _maxSpawnCount;
	}
	
	/// <summary>
	/// Will spawn prefabs at the given spawn rate.
	///
	/// Updates current count, and also updates if we've spawned too many.
	/// </summary>
	private IEnumerator Spawn(){
		// one-time spawn delay, if any
		yield return new WaitForSeconds(_spawnDelay);
		
		while (CanSpawn()){
			_currentSpawnCount+=_currentSpawnRule.Spawn(_currentSpawnCount);
			
			yield return new WaitForSeconds(_spawnRate);
		}
	}
}