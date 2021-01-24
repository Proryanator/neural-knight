using System;
using System.Collections;
using Systems.Levels;
using Maps.PlayerBoundaries.States;
using POCO.StateMachines;
using UnityEngine;

namespace Maps.PlayerBoundaries{
	/// <summary>
	/// Trigger for whether the player enters in the space to wall off the entrance for players.
	///
	/// When the player enters the trigger on this object, then we'll enable the boundary.
	///
	/// When the player leaves, we'll disable the boundary.
	/// </summary>
	public class PlayerBoundaryTrigger : MonoBehaviour{

		// holds reference of player boundary located as child
		private PlayerBoundary _playerBoundary;
		private LevelManager _levelManager;

		private StateMachine _stateMachine;
		private BlockPlayerState _blockPlayerState;
		private NotifyLevelManagerState _notifyLevelManagerState;
		private State _waitForDeLoadState;

		private static readonly ArrayList _allPlayerBoundaryTriggers = new ArrayList();

		private static bool _hasFinalTicked;

		private void Awake(){
			_playerBoundary = GetComponentInChildren<PlayerBoundary>();

			_allPlayerBoundaryTriggers.Add(this);
		}

		private void Start(){
			_levelManager = LevelManager.GetInstance();
			SetupAndCreateStates();
		}

		private void SetupAndCreateStates(){
			_stateMachine = new StateMachine();
			
			_blockPlayerState = new BlockPlayerState(_playerBoundary);
			_notifyLevelManagerState = new NotifyLevelManagerState(_levelManager);
			_waitForDeLoadState = new WaitForDeLoadState();
			
			_blockPlayerState.AddTransition(_notifyLevelManagerState, CanPlayerExit());
			_notifyLevelManagerState.AddTransition(_waitForDeLoadState, DidNotify());
			_stateMachine.SetState(_blockPlayerState);
		}
		
		private Func<bool> CanPlayerExit() => () => _levelManager.CanPlayerExitTheRoom();
		private Func<bool> DidNotify() => () => _levelManager.HasPlayerExitedTheRoom();
		
		private void OnTriggerEnter2D(Collider2D other){
			Tick(other, true);
		}

		private void OnTriggerExit2D(Collider2D other){
			Tick(other, false);
		}

		private void Tick(Collider2D other, bool hasEntered){
			_blockPlayerState.SetCollider2D(other);
			_notifyLevelManagerState.SetCollider2D(other, hasEntered, transform.position);
			_stateMachine.Tick();
		}

		public void ForceToNotifyState(){
			_stateMachine.SetState(_notifyLevelManagerState);
		}
		
		public static void ForceAllToNextState(){
			if (!_hasFinalTicked){
				_hasFinalTicked = true;
				
				foreach (PlayerBoundaryTrigger trigger in _allPlayerBoundaryTriggers){
					trigger.ForceToNotifyState();
				}
			}
		}
	}
}