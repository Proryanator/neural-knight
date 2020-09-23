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
			_followPlayerPattern = GetComponent<FollowPlayerPattern>();

			CreateEnemyStatesAndTransitions();
			
			// call the stun method when the enemy takes damage
			EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
			enemyHealth.OnDamageTaken += StunEnemy;
			
			_playerAgroManager = PlayerAgroManager.Instance();
		}

		private void Update(){
			base.Update();
		}

		private void CreateEnemyStatesAndTransitions(){
			WaitingForAgroState waitingForAgroState = new WaitingForAgroState(this);
			AgroPlayerState agroPlayerState = new AgroPlayerState(this, _followPlayerPattern);
			DamagedEntityState damagedEntityState = new DamagedEntityState(this);
			
			stateMachine.AddTransition(moveToPlayAreaState, waitingForAgroState, IsInPlayArea());
			stateMachine.AddTransition(waitingForAgroState, agroPlayerState, IsAgroSpotAvailable());
			
			stateMachine.AddAnyTransition(damagedEntityState, IsStunned());
			
			// if you were agroing, go back to that, if you were just waiting, go back to that
			stateMachine.AddTransition(damagedEntityState, waitingForAgroState, IsWaitingForAgro());
			stateMachine.AddTransition(damagedEntityState, agroPlayerState, IsAgroingPlayer());
		}

		private Func<bool> IsAgroSpotAvailable() => () => _agroSlotOpened;
		private Func<bool> IsStunned() => () => _isStunned;
		private Func<bool> IsAgroingPlayer() => () => !_isStunned && _isAgroingPlayer;
		private Func<bool> IsWaitingForAgro() => () => !_isStunned && !_isAgroingPlayer;

		public void SetPlayerAgro(bool isAgroing){
			_isAgroingPlayer = isAgroing;
		}
		
		public void StunEnemy(int damage){
			if (_isStunned){
				return;
			}
			
			_isStunned = true;
			StartCoroutine(StunEntityForSetTime(_stunTime));
			_isStunned = false;
		}

		public void SetAgroSlotOpen(bool isOpen){
			_agroSlotOpened = isOpen;
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
			yield return new WaitForSeconds(stunTime);
		}
	}
}