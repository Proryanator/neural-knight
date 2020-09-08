using Systems.PlayerAgro;
using HealthAndDamage;
using UnityEngine;

namespace Entities.NewMovement{
	public class AgroAvailabilityListener : MonoBehaviour{
		private PlayerAgroManager _playerAgroManager;

		private void Start(){
			_playerAgroManager = FindObjectOfType<PlayerAgroManager>();
			
			// can this entity agro? If so, let it.
			// Otherwise, it'll listen
		}

		/// <summary>
		/// Checks to see if we can agro the player, and if we can, will enable the agro.
		/// Otherwise, will set this guy up for listening.
		/// </summary>
		public void TriggerAgroIfEnemyController(){
			if (_playerAgroManager.CanAgroPlayer()){
				StartPlayerAgro();
				return;
			}

			// TODO: otherwise this enemy will listen for an available agro spot
			// _playerAgroManager.ListenForAgroSlot(this);
		}
		
		private void StartPlayerAgro(){
			// TODO: using this to register for Agro could be potentially harmful
			_playerAgroManager.RegisterForAgro(GetComponent<EnemyHealth>());
			
			// remove itself from listening if it was
			// _playerAgroManager.StopListeningForAgroSlot(this);
		}
	}
}