using System.Collections;
using System.Collections.Generic;
using Systems.Levels;
using Systems.Spawning.Rules;
using Systems.Spawning.SpawnAdjusters;
using UnityEngine;
using Utils;

namespace Systems.Spawning{
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

		[Tooltip("Choose the spawn rule to use for this spawner.")]
		[SerializeField] private SpawnRuleEnum _spawnRuleEnum = SpawnRuleEnum.Random;
		private AbstractSpawnRule _spawnRule;
	
		[Tooltip("Choose what rule will be applied when increasing spawning/spawn counts.")]
		[SerializeField] private SpawnAdjusterEnum _spawnAdjusterEnum = SpawnAdjusterEnum.NoAdjustment;
		private AbstractSpawnAdjuster _spawnAdjuster;

		[Tooltip("If you want this spawner to prevent level progression until it's finished spawning ALL of it's objects, this is true.")]
		[SerializeField] private bool _affectsLevelProgression;

		[SerializeField] private SpawnType _spawnType;
		
		private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
		
		// this is accessed upon awake! Gets initial values for the script
		private SpawnProperties _initialProps;

		// holds the current use-able spawn properties; initialized by initial ones
		private SpawnProperties _props;

		// tracks how many total have been spawned, even if they've been removed
		private uint _totalSpawned;
		
		private void Awake(){
			_initialProps = GetComponent<SpawnProperties>();

			// we'll start with the initial properties
			_props = _initialProps;

			// initial lookup is with the room object
			LookUpSpawnPoints(GameObject.FindGameObjectWithTag(AllTags.ROOM));
		}

		private void Start(){
			// subscribe to the level starting method; this is what initiates/restarts spawning!
			LevelManager.GetInstance().OnLevelFinish += AdjustSpawnProperties;
		
			// also, when a new level starts, we'll also start spawning again
			LevelManager.GetInstance().OnLevelStart += StartSpawning;
		}

		public void UseSpawnPointsIn(GameObject obj){
			LookUpSpawnPoints(obj);
		}
		
		private void LookUpSpawnPoints(GameObject obj){
			// wipe any previous spawn points
			_spawnPoints.Clear();
			SpawnPoint[] allSpawnPoints = FindObjectsOfType<SpawnPoint>();

			foreach (var spawnPoint in allSpawnPoints){
				if (spawnPoint.spawnType.Equals(_spawnType)){
					_spawnPoints.Add(spawnPoint);
				}
			}

			if (_spawnRule != null){
				// also set the spawn rule (required to update spawn points)
				_spawnRule.ReSetSpawnPoints(_spawnPoints.ToArray());
			}
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
			_props.ResetCounters();
			StartCoroutine(Spawn());
		}

	
	
		/// <summary>
		/// Contains the logic to up the spawn count using a super basic algorithm.
		/// This can always be updated later on when researching other games like
		/// BLOPS2, boxhead, etc.
		/// </summary>
		/// <param name="gameLevel"></param>
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
				Destroy(_spawnRule);
			}

			_spawnRule = AbstractSpawnRule.GetRule(_spawnRuleEnum, _spawnPrefab, _spawnPoints, _props.sceneLimit);
		}

		private void SetSpawnAdjuster(){
			// also destroy the old rule if exists
			if (_spawnAdjuster != null){
				Destroy(_spawnAdjuster);
			}

			_spawnAdjuster = AbstractSpawnAdjuster.GetSpawnAdjuster(_spawnAdjusterEnum);
		}

		/// <summary>
		/// True if this spawner controls level progression, false if not.
		///
		/// Useful to see which spawners still have left to spawn, versus which will keep spawning
		/// no matter if the level is over or not.
		/// </summary>
		public bool AffectsLevelProgression(){
			return _affectsLevelProgression;
		}

		/// <summary>
		/// True if you can spawn more entities right now in the scene, false if not
		/// </summary>
		private bool IsSpaceInScene => _props.inSceneCount < _props.sceneLimit;

		/// <summary>
		/// True if you still have entities left to spawn, regardless if you can't spawn any right now
		/// due to a scene limit. False if you're done.
		/// </summary>
		public bool CanSpawn => _props.totalSpawnedThisLevel < _props.totalToSpawn;

		/// <summary>
		/// Will spawn prefabs at the given spawn speed.
		///
		/// Updates current count, and also updates if we've spawned too many.
		/// </summary>
		private IEnumerator Spawn(){
			// one-time spawn delay, if any
			yield return new WaitForSeconds(_props.spawnDelay);
		
			// while we can spawn (still have entities left), keep checking
			while (CanSpawn){
				// if we have space for more entities, then spawn them!
				if (IsSpaceInScene){
					// increment how many are in the scene based on how many were made
					_props.IncrementSpawnCount(_spawnRule.Spawn(_props));
				}
				
				yield return new WaitForSeconds(_props.spawnSpeed);
			}
		}
	}
}