using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Class to handle the spawning in of prefab objects.
///
/// Based on the chosen spawn rule, will spawn objects accordingly.
///
/// This is designed to spawn X number of prefabs in a given 'round' if you will, and will
/// not need to keep track of if enemies are destroyed or not.
/// </summary>
public class SpawnManager : MonoBehaviour{
	
	[Tooltip("The prefab to spawn.")]
	[SerializeField] private Transform _spawnPrefab;

	[Tooltip("Makes spawn manager wait this amount of seconds before starting it's spawn process. Waits only once.")]
	[SerializeField] private float _spawnDelay = 0f;
	
	[Tooltip("The rate at which prefabs will spawn in, in seconds.")]
	[SerializeField] private float _spawnRate = 1;

	[Tooltip("The maximum number of prefabs to spawn.")]
	[SerializeField] private int _maxSpawnCount = 5;

	[Tooltip("Enable this to initiate restart of spawning. Intended for testing purposes only.")]
	[SerializeField] private bool _restart = false;

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
	}

	private void Update(){
		// TODO: take me out, might be worth making tests for this
		if (_restart){
			Restart(.3f, 10);
			_restart = false;
		}
	}

	private void Start(){
		// setup spawn rules
		SetSpawnRule();
		
		// start the spawning process!
		StartCoroutine(Spawn());
	}

	/// <summary>
	/// Using the set SpawnRule, set the rule object.
	/// If one was already set, destroys that one and re-instantiates one.
	///
	/// Intended to be called from outside the spawn manager to change this at runtime.
	/// </summary>
	public void SetSpawnRule(){
		// destroy old rule if it existed
		if (_currentSpawnRule != null){
			ScriptableObject.Destroy(_currentSpawnRule);
		}

		_currentSpawnRule = AbstractSpawnRule.GetRule(spawnRuleEnum, _spawnPrefab, _spawnPointsInScene, _maxSpawnCount);
	}
	
	/// <summary>
	/// Restarts the spawning. Should be called upon a new level loading!
	///
	/// NOTE: might be best for the spawn manager to get the next count from somewhere else,
	/// let's say a file for example.
	/// </summary>
	public void Restart(float spawnRate, int maxCount){
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