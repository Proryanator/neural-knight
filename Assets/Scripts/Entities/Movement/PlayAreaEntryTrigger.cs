using Systems.PlayerAgro;
using UnityEngine;

namespace Entities.Movement{
	public class PlayAreaEntryTrigger : MonoBehaviour{

		private static PlayAreaEntryTrigger _instance;

		private void Awake(){
			if (_instance == null){
				_instance = this;
			}
			else if (_instance != this){
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other){
			StartEntityMovementIfViable(other);
		}
		
		private void StartEntityMovementIfViable(Collider2D other){
			EntityMovementController entityMovementController = other.GetComponent<EntityMovementController>();
			if (entityMovementController == null){
				return;
			}

			TriggerOrListenForAgroIfAvailable(other);
			entityMovementController.StartInitialMovementPattern();

			// sets the layer appropriately to what it should be once the object is inside of the playable area
			other.GetComponent<EntityPlayAreaLayerChanger>().SetToInsidePlayAreaLayer();
		}

		private void TriggerOrListenForAgroIfAvailable(Collider2D other){
			AgroAvailabilityListener agroAvailabilityListener = other.GetComponent<AgroAvailabilityListener>();
			if (agroAvailabilityListener != null){
				agroAvailabilityListener.TriggerAgroOrListen();
			}
		}
	}
}