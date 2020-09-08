using Entities.Events;
using Entities.MovementPatterns;
using UnityEngine;

namespace Systems.PlayerAgro{
	public class AgroAvailabilityListener : MonoBehaviour{
		private PlayerAgroManager _playerAgroManager;

		private FollowPlayerPattern _followPlayerPattern;
		
		private void Start(){
			_playerAgroManager = FindObjectOfType<PlayerAgroManager>();

			_followPlayerPattern = GetComponent<FollowPlayerPattern>();

			if (_followPlayerPattern == null){
				Debug.LogWarning(
					"Agro will not work properly cause you forgot to attach a 'FollowPlayerPattern' object to this!");
			}
		}

		/// <summary>
		/// Checks to see if we can agro the player, and if we can, will enable the agro.
		/// Otherwise, will set this guy up for listening.
		/// </summary>
		public void TriggerAgroOrListen(){
			if (_followPlayerPattern.IsAgroing()){
				return;
			}
			
			if (_playerAgroManager.CanAgroPlayer()){
				StartPlayerAgro();
				return;
			}

			// TODO: otherwise this enemy will listen for an available agro spot
			_playerAgroManager.ListenForAgroSlot(this);
			
			// remove yourself from listening if you're killed before!
			GetComponent<DeSpawnable>().OnDeSpawn += StopListeningIfKilledFirst;
		}

		private void StopListeningIfKilledFirst(){
			_playerAgroManager.StopListeningForAgroSlot(this);
		}

		private void StartPlayerAgro(){
			_playerAgroManager.RegisterForAgro(GetComponent<DeSpawnable>());
			
			// turn on ability to go for the player!
			_followPlayerPattern.EnableAgro();
			
			// remove itself from listening if it was
			_playerAgroManager.StopListeningForAgroSlot(this);
		}
	}
}