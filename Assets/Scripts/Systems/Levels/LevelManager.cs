using System;
using Systems.Levels.LevelStates;
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

		private StateMachine _stateMachine;

		private void Awake(){
			if (_instance == null){
				_instance = this;
				
				CreateAndSetupStates();
			}else if (_instance != this){
				Destroy(gameObject);
			}
		}

		private void Update(){
			_stateMachine.Tick();
		}

		private void CreateAndSetupStates(){
			_stateMachine = new StateMachine();
			
			IState startLevelState = new StartLevelState(this);
			IState collectDataState = new CollectDataState();
			IState enemyCleanupState = new EnemyCleanupState();
			IState endOfLevelState = new EndOfLevelState(this);
			
			_stateMachine.AddTransition(startLevelState, collectDataState, HasGameStarted());
			_stateMachine.AddTransition(collectDataState, enemyCleanupState, IsDataCollectedButEnemiesLeft());
			_stateMachine.AddTransition(collectDataState, endOfLevelState, AreAllEntitiesGone());
			_stateMachine.AddTransition(endOfLevelState, startLevelState, HasGameStarted());
			
			_stateMachine.SetState(startLevelState);
		}
		
		private Func<bool> HasGameStarted() => () => true;
		private Func<bool> IsDataCollectedButEnemiesLeft() => () => IsAllDataCollected() && AreThereEnemiesLeft();
		private Func<bool> AreAllEntitiesGone() => () => IsAllDataCollected() && !AreThereEnemiesLeft();
		
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
		
		private bool IsAllDataCollected(){
			return DataPoint.GetDataPointCountInScene() == 0;
		}
		
		private bool AreThereEnemiesLeft(){
			return EnemiesInSceneCounter.GetTotalEnemiesInScene() > 0;
		}
	}
}