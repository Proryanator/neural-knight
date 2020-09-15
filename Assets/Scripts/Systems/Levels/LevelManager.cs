using System;
using System.Collections.Generic;
using Systems.Levels.LevelStates;
using Systems.Spawning;
using DataPoints;
using POCO.StateMachines;
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

		// the current game level, incremented through some game behavior 
		private int _gameLevel = 0;

		// notifies when a level ends
		public Action<int> OnLevelFinish;
	
		// notifies when a level starts
		public Action<int> OnLevelStart;

		private SpawnManager[] _spawnManagers;
		
		private StateMachine _stateMachine;
		private IState _startLevelState;
		
		private void Awake(){
			if (_instance == null){
				_instance = this;
				
				CreateAndSetupStates();
			}else if (_instance != this){
				Destroy(gameObject);
			}
		}

		private void Start(){
			_stateMachine.SetState(_startLevelState);
		}

		private void Update(){
			_stateMachine.Tick();
		}

		private void CreateAndSetupStates(){
			_stateMachine = new StateMachine();
			
			_startLevelState = new StartLevelState(this);
			IState collectDataState = new CollectDataState();
			IState enemyCleanupState = new EnemyCleanupState();
			IState endOfLevelState = new EndOfLevelState(this);
			
			_stateMachine.AddTransition(_startLevelState, collectDataState, HasGameStarted());
			_stateMachine.AddTransition(collectDataState, enemyCleanupState, IsDataCollectedButEnemiesLeft());
			_stateMachine.AddTransition(collectDataState, endOfLevelState, AreAllEntitiesGone());
			_stateMachine.AddTransition(enemyCleanupState, endOfLevelState, AreAllEntitiesGone());
			_stateMachine.AddTransition(endOfLevelState, _startLevelState, HasGameStarted());
		}
		
		private Func<bool> HasGameStarted() => () => true;
		private Func<bool> IsDataCollectedButEnemiesLeft() => () => AreAllSpawnersDoneSpawning() && IsAllDataCollected() && AreThereEnemiesLeft();
		private Func<bool> AreAllEntitiesGone() => () => AreAllSpawnersDoneSpawning() && IsAllDataCollected() && !AreThereEnemiesLeft();
		
		public static LevelManager GetInstance(){
			return _instance;
		}

		public void InvokeStartLevelEvent(){
			OnLevelStart?.Invoke(_gameLevel);
		}

		public void InvokeEndLevelEvent(){
			OnLevelFinish?.Invoke(_gameLevel);
		}

		public void IncrementLevel(){
			_gameLevel++;
		}
		
		/// <summary>
		/// Gets you an array of spawn managers that need to spawn completely before the level progresses.
		/// </summary>
		public void SetSpawnManagersForLevelProgression(){
			List<SpawnManager> levelSpawnManagers = new List<SpawnManager>();
			SpawnManager[] allSpawnManagers = FindObjectsOfType<SpawnManager>();

			foreach (SpawnManager manager in allSpawnManagers){
				if (manager.AffectsLevelProgression()){
					levelSpawnManagers.Add(manager);
				}
			}

			_spawnManagers = levelSpawnManagers.ToArray();
		}
		
		private bool IsAllDataCollected(){
			return DataPoint.GetDataPointCountInScene() == 0;
		}
		
		private bool AreThereEnemiesLeft(){
			return EnemiesInSceneCounter.GetTotalEnemiesInScene() > 0;
		}

		private bool AreAllSpawnersDoneSpawning(){
			foreach (SpawnManager manager in _spawnManagers){
				if (manager.CanSpawn){
					return false;
				}
			}

			return true;
		}
	}
}