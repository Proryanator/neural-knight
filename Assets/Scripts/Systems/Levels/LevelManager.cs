using System;
using Systems.Spawning;
using DataPoints;
using UnityEngine;

namespace Systems.Levels{
	/// <summary>
	/// Responsible for managing what's considered 'levels' in the game.
	///
	/// For now, can be a singleton!
	///
	/// Will distribute out information about what changes if needed through events.
	/// </summary>
	public class LevelManager : MonoBehaviour{
	
		private static LevelManager _instance;

		[Tooltip("How many enemies to start spawning with. Will increase from here.")]
		[SerializeField] private int _initialEnemySpawnCount = 10;
	
		// the current game level, incremented through some game behavior 
		private int _gameLevel = 1;
	
		// we need a cache of all spawn managers for checking if a level has ended
		private SpawnManager[] _allSpawnManagers;
	
		// notifies when a level ends
		public Action<int> OnLevelFinish;
	
		// notifies when a level starts
		public Action<int> OnLevelStart;

		private void Awake(){
			if (_instance == null){
				_instance = this;
			}else if (_instance != this){
				Destroy(gameObject);
			}

			_allSpawnManagers = GameObject.FindObjectsOfType<SpawnManager>();
		}

		private void Start(){
			// for now, we just start the game
			// TODO: might add logic here to allow for starting the level when the player is ready (perhaps to allow
			// for upgrades or something?
			StartLevel();
		}

		public static LevelManager GetInstance(){
			return _instance;
		}
	
		/// <summary>
		/// Initiates the start of a new level.
		///
		/// Calls any other methods that need to happen here.
		/// </summary>
		public void StartLevel(){
			Debug.Log("Starting Level [" + _gameLevel + "]!");

			if (OnLevelStart != null){
				OnLevelStart(_gameLevel);
			}
		}

		/// <summary>
		/// Performs a check to see if the level is considered 'ended' or not.
		///
		/// Checks if all spawn managers are done spawning. If they are, checks to see
		/// if any children are still left in the scene.
		/// </summary>
		public void LevelEndCheck(){
			// if any of the spawn managers is still spawning, we're not done
			foreach (SpawnManager manager in _allSpawnManagers){
				if (manager.CanSpawn()){
					return;
				}
			}
		
			// if all the spawned objects are missing, then the level is over!
			// TODO: this is an expensive call, refactor to do it a better way
			if (GameObject.FindObjectsOfType<DataPoint>().Length == 0){
				EndLevel();
			}
		}
	
		/// <summary>
		/// Initiates the end of a level. This does any level cleanup,
		/// including preparing for the next level.
		/// </summary>
		private void EndLevel(){
			// first, let's increment the level counter to the next level
			IncrementLevel();
		
			// and for now, just starts the next level!
			StartLevel();
		}

		private void IncrementLevel(){
			_gameLevel++;
		
			// if there are any listeners, notify them that the level has changed!
			if (OnLevelFinish != null){
				OnLevelFinish(_gameLevel);
			}
		}
	}
}