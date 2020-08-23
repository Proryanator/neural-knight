using UnityEngine;

namespace Entities.Movement{
	/// <summary>
	/// Determines if the entity is in the play area, if so then this does nothing.
	/// If the entity is of a specific type, will make an additional call to see if
	/// they should agro the player or not.
	///
	/// If not, sets the entity's movement pattern to the MoveToCenterMovementPattern,
	/// to force the entity to begin walking towards the center.
	/// </summary>
	public class PlayAreaEntryController : MonoBehaviour{

		private AbstractMovementController _abstractMovementController;

		private void Awake(){
			_abstractMovementController = GetComponent<AbstractMovementController>();
		}

		public void Start(){
			// if this object is not inside of the play area, call special function
			// on trigger enter of the play area, this will be turned off
			if (!PlayAreaMovementStarter.Instance().IsInsidePlayArea(transform)){
				Debug.Log("I'm outside the play area, gonna move to the center.");
				_abstractMovementController.EnableCenterPattern();
			}
		}

		
	}
}