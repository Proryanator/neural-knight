﻿using System;
using Systems.Spawning;
using DataPoints;
using Entities.Movement;
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

		// tracks the current level state of the current level
		private LevelState _levelState = LevelState.WaitingToStart;

		// call this when the levels' state changes, to alert things that need to know this
		public Action<LevelState> OnLevelStateChange;
		
		private void Awake(){
			if (_instance == null){
				_instance = this;
			}else if (_instance != this){
				Destroy(gameObject);
			}

			_allSpawnManagers = FindObjectsOfType<SpawnManager>();
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
			SetLevelState(LevelState.Started);
			
			Debug.Log("Starting Level [" + _gameLevel + "]!");

			if (OnLevelStart != null){
				OnLevelStart(_gameLevel);
			}
		}

		/// <summary>
		/// A method intended to be called from entities that affect how the levels' state changes.
		///
		/// Will potentially end the level or change it's state if the rules are met.
		/// </summary>
		public void LevelStateChangeCheck(){
			// if there are data points left, the level is not over
			if (AreThereDataPointsLeft()){
				return;
			}
			
			// if there are enemies left in the scene, this is a special level state
			if (AreThereEnemiesLeft()){
				SetLevelState(LevelState.EnemyCleanup);
			}
			else{
				// TODO: one day, trigger the level state where the player can walk around/take action
				EndLevel();	
			}
		}

		/// <summary>
		/// True if there are data points left in the scene, false if not.
		/// </summary>
		private bool AreThereDataPointsLeft(){
			return DataPoint.GetDataPointCountInScene() > 0;
		}

		/// <summary>
		/// True if there are enemies still left in the scene, false if not.
		/// </summary>
		private bool AreThereEnemiesLeft(){
			return EnemyMovementController.GetTotalEnemiesInScene() > 0;
		}
	
		/// <summary>
		/// Initiates the end of a level. This does any level cleanup,
		/// including preparing for the next level.
		/// </summary>
		private void EndLevel(){
			SetLevelState(LevelState.Ended);
			
			// first, let's increment the level counter to the next level
			_gameLevel++;
			
			// if there are any listeners, notify them that the level has changed!
			if (OnLevelFinish != null){
				OnLevelFinish(_gameLevel);
			}
		
			// TODO: instead of stopping the level altogether, let the player do something here instead
			// and for now, just starts the next level!
			StartLevel();
		}

		/// <summary>
		/// Wrapper to change level state. Use this to see nice logging if you'd like.
		/// </summary>
		private void SetLevelState(LevelState state){
			Debug.Log("Changed Level State from [" + _levelState + "] to: [" + state + "]");
			_levelState = state;

			// call any listeners that need to know when the current state has changed
			if (OnLevelStateChange != null){
				OnLevelStateChange(_levelState);
			}
		}
	}
}