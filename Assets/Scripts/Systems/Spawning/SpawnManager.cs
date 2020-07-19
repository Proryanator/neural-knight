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
[RequireComponent(typeof(SpawnProperties))]
public class SpawnManager : MonoBehaviour{
	
	[Tooltip("The prefab to spawn.")]
	[SerializeField] private Transform _spawnPrefab;
	
	// this is accessed upon awake! Gets initial values for the script
	private SpawnProperties _initialProps;

	// holds the current use-able spawn properties; initialized by initial ones
	private SpawnProperties _props;

	[Tooltip("Choose the spawn rule to use for this spawner.")]
	[SerializeField] private SpawnRuleEnum _spawnRuleEnum = SpawnRuleEnum.Random;
	private AbstractSpawnRule _spawnRule;
	
	[Tooltip("Choose what rule will be applied when increasing spawning/spawn counts.")]
	[SerializeField] private SpawnAdjusterEnum _spawnAdjusterEnum = SpawnAdjusterEnum.NoAdjustment;
	private AbstractSpawnAdjuster _spawnAdjuster;
	
	[Tooltip("The object that holds spawn points that all have the same kind of spawned object.")]
	[SerializeField] private SpawnCollection _spawnCollection;

	private void Awake(){
		_initialProps = GetComponent<SpawnProperties>();

		if (_spawnCollection == null){
			Debug.LogWarning("The SpawnCollector was not set for this spawn manager, did you forget?");
		}
	}

	private void Start(){
		// we'll start with the initial properties
		_props = _initialProps;
		
		// subscribe to the level starting method; this is what initiates/restarts spawning!
		LevelManager.GetInstance().OnLevelFinish += AdjustSpawnProperties;
		
		// also, when a new level starts, we'll also start spawning again
		LevelManager.GetInstance().OnLevelStart += StartSpawning;
	}

	/// <summary>
	/// Starts the spawning. Intended to be setup externally!
	///
	/// This also instantiates Scriptable rule and adjuster objects,
	/// so make sure to make any modifications you want before
	/// calling this!
	/// </summary>
	private void StartSpawning(int gameLevel){
		// setup spawn rules
		SetSpawnRule();
		
		// also instantiates an instance of the adjuster
		SetSpawnAdjuster();
		
		// set max count, restart counter, and start co routine!
		_props.spawnCount = 0;
		StartCoroutine(Spawn());
	}

	/**
	 * Contains the logic to up the spawn count using a super basic algorithm.
	 *
	 * This can always be updated later on when researching other games like
	 * BLOPS2, boxhead, etc.
	 */
	private void AdjustSpawnProperties(int gameLevel){
		_props = _spawnAdjuster.AdjustSpawnProperties(_props, gameLevel);
	}
	
	/// <summary>
	/// Using the set SpawnRule, set the rule object.
	/// If one was already set, destroys that one and re-instantiates one.
	///
	/// Intended to be called from outside the spawn manager to change this at runtime.
	/// </summary>
	private void SetSpawnRule(){
		// destroy old rule if it existed
		if (_spawnRule != null){
			ScriptableObject.Destroy(_spawnRule);
		}

		_spawnRule = AbstractSpawnRule.GetRule(_spawnRuleEnum, _spawnPrefab, _spawnCollection.GetSpawnPoints(), _props.maxSpawnCount);
	}

	private void SetSpawnAdjuster(){
		// also destroy the old rule if exists
		if (_spawnAdjuster != null){
			ScriptableObject.Destroy(_spawnAdjuster);
		}

		_spawnAdjuster = AbstractSpawnAdjuster.GetSpawnAdjuster(_spawnAdjusterEnum);
	}

	public bool CanSpawn(){
		return _props.spawnCount < _props.maxSpawnCount;
	}
	
	/// <summary>
	/// Will spawn prefabs at the given spawn speed.
	///
	/// Updates current count, and also updates if we've spawned too many.
	/// </summary>
	private IEnumerator Spawn(){
		// one-time spawn delay, if any
		yield return new WaitForSeconds(_props.spawnDelay);
		
		while (CanSpawn()){
			_props.spawnCount +=_spawnRule.Spawn(_props.spawnCount);
			
			yield return new WaitForSeconds(_props.spawnSpeed);
		}
	}
}