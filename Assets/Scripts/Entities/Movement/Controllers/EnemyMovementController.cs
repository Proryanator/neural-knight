using System;
using System.Collections;
using Entities.Movement.EntityStates;
using Entities.MovementPatterns;
using HealthAndDamage;
using UnityEngine;

namespace Entities.Movement.Controllers{
	public class EnemyMovementController : AbstractEntityMovementController{
		
		[SerializeField] private float _stunTime = 1f;

		private bool _isStunned = false;

		private bool _isAgroingPlayer = false;
		
		private FollowPlayerPattern _followPlayerPattern;
		
		private void Awake(){
			base.Awake();
			_followPlayerPattern = GetComponent<FollowPlayerPattern>();

			CreateEnemyStatesAndTransitions();
			
			// call the stun method when the enemy takes damage
			EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
			enemyHealth.OnDamageTaken += StunEnemy;
		}

		private void CreateEnemyStatesAndTransitions(){
			AgroPlayerState agroPlayerState = new AgroPlayerState(this, _followPlayerPattern);
			
			stateMachine.AddTransition(normalMovementState, agroPlayerState, IsAgroSpotAvailable());
			
			// enemies can be damaged, and thus this is also added as an any transition!
			DamagedEntityState damagedEntityState = new DamagedEntityState(this);
			stateMachine.AddAnyTransition(damagedEntityState, IsStunned());
			
			// if you were agroing, go back to that, if you were just waiting, go back to that
			stateMachine.AddTransition(damagedEntityState, normalMovementState, IsWaitingForAgro());
			stateMachine.AddTransition(damagedEntityState, agroPlayerState, IsAgroingPlayer());
		}

		private Func<bool> IsAgroSpotAvailable() => () => true;
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
		
		private IEnumerator StunEntityForSetTime(float stunTime){
			yield return new WaitForSeconds(stunTime);
		}
	}
}