using System.Collections.Generic;
using Systems.Levels;
using Entities.Events;
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

		[Tooltip("Trigger agro increase every X levels")]
		[SerializeField] private int _agroLevelBoundary = 2;
		
		// holds list of unique ids of enemy listeners
		private List<AgroAvailabilityListener> _listOfAgroListeners = new List<AgroAvailabilityListener>();
		
		private void Awake(){
			if (_instance == null){
				_instance = this;
			}else if (_instance != this){
				Destroy(gameObject);
			}
		}

		private void Start(){
			// let's register this method to the 'OnLevelFinish' method, to increase/adjust agro level
			LevelManager.GetInstance().OnLevelFinish += IncreaseAgroAmount;
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
		public void RegisterForAgro(DeSpawnable deSpawnable){
			if (!CanAgroPlayer()){
				Debug.LogWarning("You called this method when there were no slots left to agro the player!");
			}

			// increment the agro counter
			_currentAgro++;

			deSpawnable.OnDeSpawn += DeRegisterAgro;

			Debug.Log("Enemy registered to agro the player.");
		}

		/// <summary>
		/// Add this object to the list of objects listening for open slots
		/// </summary>
		public void ListenForAgroSlot(AgroAvailabilityListener agroAvailabilityListener){
			Debug.Log("Enemy is listening for agro slot to open");
			_listOfAgroListeners.Add(agroAvailabilityListener);
		}

		/// <summary>
		/// Remove this object from the list of listening objects.
		/// </summary>
		public void StopListeningForAgroSlot(AgroAvailabilityListener agroAvailabilityListener){
			if (_listOfAgroListeners.Contains(agroAvailabilityListener)){
				Debug.Log("Enemy stopped listening for agro slot to open");
				_listOfAgroListeners.Remove(agroAvailabilityListener);
			}
		}
		
		/// <summary>
		/// True if there is space left for enemies to agro the player, false if not.
		/// </summary>
		public bool CanAgroPlayer(){
			return _currentAgro < _maxAgro;
		}
		
		/// <summary>
		/// Decrements the current agro, and notifies the next one listening that the slot is open!
		/// </summary>
		private void DeRegisterAgro(){
			_currentAgro--;

			TriggerAgroOnNextRandomEnemy();

			Debug.Log("Freed up agro slot.");
		}

		/// <summary>
		/// Logic to increase the amount of enemies that can agro to the player at once.
		///
		/// If increase would be smaller than original, just keeps that.
		/// </summary>
		private void IncreaseAgroAmount(int gameLevel){
			if (gameLevel % _agroLevelBoundary == 0){
				int newAgro = gameLevel / _agroLevelBoundary;

				if (newAgro > _maxAgro){
					_maxAgro = newAgro;
				}
			}
		}

		/// <summary>
		/// If there's an enemy that's listening to agro, activate them now!
		///
		/// TODO: Might consider making this distance based instead.
		/// </summary>
		private void TriggerAgroOnNextRandomEnemy(){
			// trigger the next in line
			if (_listOfAgroListeners.Count > 0){
				RemoveNullControllers();
				
				int index = Random.Range(0, _listOfAgroListeners.Count);
				_listOfAgroListeners[index].TriggerAgroOrListen();
			}
		}

		/// <summary>
		/// It is possible that these enemies may have already been destroyed before this point,
		/// so let's make sure to remove any from the list that may be null.
		/// </summary>
		private void RemoveNullControllers(){
			List<int> indicesToRemove = new List<int>();
			// trim this list of empty components (they might have been destroyed already)
			for (int i = 0; i < _listOfAgroListeners.Count; i++){
				if (_listOfAgroListeners[i] == null){
					indicesToRemove.Add(i);
				}
			}

			foreach (int index in indicesToRemove){
				_listOfAgroListeners.RemoveAt(index);
			}
		}
	}
}