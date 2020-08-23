using UnityEngine;
using Utils;

namespace Entities.Movement{
	/// <summary>
	/// Any entity outside of the 'play area' will need to have their
	/// movement pattern set to one that tracks to the center of the map.
	///
	/// This way, objects will move to the center of the map, and 'enter' the map.
	///
	/// Intended for there to only be 1 of these at all times.
	/// </summary>
	public class PlayAreaMovementStarter : MonoBehaviour{

		private static PlayAreaMovementStarter _instance;

		private Collider2D _collider2D;

		private void Awake(){
			if (_instance == null){
				_instance = this;
			}
			else if (_instance != this){
				Destroy(gameObject);
			}

			_collider2D = GetComponent<Collider2D>();
		}

		public static PlayAreaMovementStarter Instance(){
			return _instance;
		}

		private void OnTriggerEnter2D(Collider2D other){
			StartInitialMovementIfViable(other);
		}

		private void StartInitialMovementIfViable(Collider2D other){
			// call a start call on objects that have movement patterns
			AbstractMovementController movementController = other.GetComponent<AbstractMovementController>();
			
			// special setup code required for enemy controllers here
			if (movementController is EnemyMovementController){
				EnemyMovementController enemyMovementController = (EnemyMovementController) movementController;
				enemyMovementController.TriggerAgroIfEnemyController(true);
				return;
			}

			if (movementController != null){
				Debug.Log("Found AI! Starting their normal movement.");
				movementController.StartInitialPattern();

				// also sets the layer of 'Entity' to this object, keeping it inside the playable area
				other.gameObject.layer = LayerMask.NameToLayer(AllLayers.ENTITY);
			}
		}

		/// <summary>
		/// Determines if you're in the play area.
		/// </summary>
		/// <returns>true if you're inside the play area, false if you're not</returns>
		public bool IsInsidePlayArea(Transform transform){
			return _collider2D.bounds.Contains(transform.position);
		}
	}
}