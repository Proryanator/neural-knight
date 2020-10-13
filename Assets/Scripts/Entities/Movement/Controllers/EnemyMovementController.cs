using System;
using System.Collections;
using Systems.PlayerAgro;
using Entities.Events;
using Entities.Movement.EntityStates;
using Entities.MovementPatterns;
using HealthAndDamage;
using UnityEngine;

namespace Entities.Movement.Controllers{
	public class EnemyMovementController : AbstractEntityMovementController{
		
		[SerializeField] private float _stunTime = 1f;

		private bool _isStunned;
		private bool _agroSlotOpened;
		private bool _isAgroingPlayer;
		
		private FollowPlayerPattern _followPlayerPattern;

		private PlayerAgroManager _playerAgroManager;
		
		private void Awake(){
			base.Awake();
			_playerAgroManager = PlayerAgroManager.Instance();
			_followPlayerPattern = GetComponent<FollowPlayerPattern>();

			CreateEnemyStatesAndTransitions();
			
			// call the stun method when the enemy takes damage
			EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
			enemyHealth.OnDamageTaken += StunEnemy;
		}

		private void Update(){
			base.Update();
		}

		private void CreateEnemyStatesAndTransitions(){
			WaitingForAgroState waitingForAgroState = new WaitingForAgroState(GetNormalMovementPattern(), this);
			AgroPlayerState agroPlayerState = new AgroPlayerState(this, _followPlayerPattern);
			DamagedEntityState damagedEntityState = new DamagedEntityState(this);
			
			// if you can agro, just go ahead and agro
			if (_playerAgroManager.CanAgroPlayer()){
				EnableAgro();
				_playerAgroManager.RegisterForAgro(GetComponent<DeSpawnable>());
				
				// move directly to agro the player when in play area
				moveToPlayAreaState.AddTransition(agroPlayerState, IsInPlayArea());
			}else{
				moveToPlayAreaState.AddTransition(waitingForAgroState, IsInPlayArea());
				waitingForAgroState.AddTransition(agroPlayerState, IsAgroSpotAvailable());
				
				// also add a way to get back to this transition if you're needing to wait
				damagedEntityState.AddTransition(waitingForAgroState, IsWaitingForAgro());
			}

			// this stuff happens no matter if you started to agro or not
			stateMachine.AddAnyTransition(damagedEntityState, IsStunned());
			damagedEntityState.AddTransition(agroPlayerState, IsAgroingPlayer());
		}
		
		private Func<bool> IsAgroSpotAvailable() => () => _agroSlotOpened;
		private Func<bool> IsStunned() => () => _isStunned;
		private Func<bool> IsAgroingPlayer() => () => !_isStunned && _isAgroingPlayer;
		private Func<bool> IsWaitingForAgro() => () => !_isStunned && !_isAgroingPlayer;

		public void SetPlayerAgro(bool isAgroing){
			_isAgroingPlayer = isAgroing;
		}

		public bool IsAgroEnabledFromStart(){
			return _isAgroingPlayer;
		}
		
		public void StunEnemy(){
			if (_isStunned){
				return;
			}
			
			StartCoroutine(StunEntityForSetTime(_stunTime));
		}

		public void EnableAgro(){
			_agroSlotOpened = true;
		}
		
		public void ListenForAgroSlot(){
			_playerAgroManager.ListenForAgroSlot(this);
			
			// remove yourself from listening if you're killed before!
			GetComponent<DeSpawnable>().OnDeSpawn += StopListeningIfKilledFirst;
		}
		
		private void StopListeningIfKilledFirst(){
			_playerAgroManager.StopListeningForAgroSlot(this);
		}

		private IEnumerator StunEntityForSetTime(float stunTime){
			_isStunned = true;
			yield return new WaitForSeconds(stunTime);
			_isStunned = false;
		}
	}
}