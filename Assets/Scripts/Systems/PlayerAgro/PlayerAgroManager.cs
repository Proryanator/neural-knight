using System.Collections.Generic;
using Entities.Movement;
using HealthAndDamage;
using UnityEngine;

namespace Systems.PlayerAgro{
	/// <summary>
	/// A class intended to manage how enemies agro to the player.
	///
	/// Right now this only handles/deals with 1 player.
	/// </summary>
	public class PlayerAgroManager : MonoBehaviour{
		
		private static PlayerAgroManager _instance;
		
		[Tooltip("The maximum number of enemies that can agro to the player")]
		[SerializeField] private int _maxAgro;
		
		// the current number of enemies that are agro'd to the player
		[SerializeField] private int _currentAgro = 0;

		// holds list of unique ids of enemy listeners
		private List<EnemyMovementController> _listOfListeners = new List<EnemyMovementController>();
		
		private void Awake(){
			if (_instance == null){
				_instance = this;
			}else if (_instance != this){
				Destroy(gameObject);
			}
		}

		public static PlayerAgroManager GetInstance(){
			return _instance;
		}
		
		/// <summary>
		/// Registers the enemy as having agro to the player.
		/// A method will be attached to this enemy's OnDeath script,
		/// so that it will automatically remove itself from the active list.
		///
		/// Should only be called after calling 'CanAgroPlayer'.
		/// </summary>
		public void RegisterForAgro(EnemyHealth enemyHealth){
			if (!CanAgroPlayer()){
				Debug.LogWarning("You called this method when there were no slots left to agro the player!");
			}

			// increment the agro counter
			_currentAgro++;

			enemyHealth.OnDeath += DeRegisterAgro;

			Debug.Log("Enemy registered to agro the player.");
		}

		/// <summary>
		/// Add this object to the list of objects listening for open slots
		/// </summary>
		public void ListenForAgroSlot(EnemyMovementController enemyMovementController){
			Debug.Log("Enemy is listening for agro slot to open");
			_listOfListeners.Add(enemyMovementController);
		}

		/// <summary>
		/// Remove this object from the list of listening objects.
		/// </summary>
		public void StopListeningForAgroSlot(EnemyMovementController enemyMovementController){
			if (_listOfListeners.Contains(enemyMovementController)){
				Debug.Log("Enemy stopped listening for agro slot to open");
				_listOfListeners.Remove(enemyMovementController);
			}
		}
		
		/// <summary>
		/// Decrements the current agro, and notifies the next one listening that the slot is open!
		/// </summary>
		public void DeRegisterAgro(){
			_currentAgro--;

			TriggerAgroOnNextRandomEnemy();

			Debug.Log("Freed up agro slot.");
		}

		/// <summary>
		/// If there's an enemy that's listening to agro, activate them now!
		///
		/// TODO: Might consider making this distance based instead.
		/// </summary>
		private void TriggerAgroOnNextRandomEnemy(){
			// trigger the next in line
			if (_listOfListeners.Count > 0){
				int index = Random.Range(0, _listOfListeners.Count);
				_listOfListeners[index].TriggerAgroIfEnemyController(false);
			}
		}
		
		/// <summary>
		/// True if there is space left for enemies to agro the player, false if not.
		/// </summary>
		public bool CanAgroPlayer(){
			return _currentAgro < _maxAgro;
		}
	}
}