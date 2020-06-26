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
	
	[Tooltip("Makes spawn manager wait this amount of seconds before starting it's spawn process. Waits only once.")]
	[SerializeField] private float _spawnDelay = 0f;

	#region SpawnRates
	
	// the rate at which prefabs will spawn in, in seconds
	private float _spawnRate = -1f;
	
	[Tooltip("The initial spawn rate that the game will start with. Increases from here.")]
	[SerializeField] private float _initialEnemySpawnRate = 2;
	
	[Tooltip("The lowest/quickest that the spawn manager will progress to, will not get faster than this.")]
	[SerializeField] private float _minEnemySpawnRate = .2f;

	#endregion

	#region SpawnCounts

	// the maximum number of prefabs to spawn
	private int _maxSpawnCount = -1;
	
	// how many we've spawned so far
	private int _currentSpawnCount = 0;
	
	[Tooltip("The starting spawn count.")]
	[SerializeField] private int _initialMaxSpawnCount = 3;

	#endregion

	[Tooltip("Choose the spawn rule to use for this spawner.")]
	[SerializeField] private SpawnRuleEnum spawnRuleEnum = SpawnRuleEnum.Random;

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

	private void Start(){
		// initialize the values based on initials
		_spawnRate = _initialEnemySpawnRate;
		_maxSpawnCount = _initialMaxSpawnCount;
		
		// subscribe to the level starting method; this is what initiates/restarts spawning!
		LevelManager.GetInstance().OnLevelFinish += AdjustSpawnRates;
		
		// also, when a new level starts, we'll also start spawning again
		LevelManager.GetInstance().OnLevelStart += StartSpawning;
	}

	/// <summary>
	/// Starts the spawning. Intended to be setup externally!
	/// </summary>
	private void StartSpawning(int gameLevel){
		// setup spawn rules
		SetSpawnRule();

		// set max count, restart counter, and start co routine!
		_currentSpawnCount = 0;
		StartCoroutine(Spawn());
	}

	/**
	 * Contains the logic to up the spawn count using a super basic algorithm.
	 *
	 * This can always be updated later on when researching other games like
	 * BLOPS2, boxhead, etc.
	 */
	private void AdjustSpawnRates(int gameLevel){
		_maxSpawnCount = _initialMaxSpawnCount + (2 * gameLevel);
		
		// let's also make spawning faster every 5 levels
		int levelBoundary = 5;
		if (gameLevel % levelBoundary == 0){
			_spawnRate = _initialEnemySpawnRate - ((gameLevel / levelBoundary) * .2f);
		}

		// make sure we don't go lower (or higher) than the original spawn rate
		Mathf.Clamp(_spawnRate, _minEnemySpawnRate, _initialEnemySpawnRate);
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

	public bool CanSpawn(){
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