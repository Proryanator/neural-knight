using UnityEngine;

namespace Enemies.ObjectMovement.AIMovement{
	/// <summary>
	/// Determines if the entity is in the play area, if so then this does nothing.
	///
	/// If not, sets the entity's movement pattern to the MoveToCenterMovementPattern,
	/// to force the entity to begin walking towards the center.
	/// </summary>
	public class MoveToCenterController : MonoBehaviour{

		private AIMovementController _aiMovementController;

		private void Awake(){
			_aiMovementController = GetComponent<AIMovementController>();
		}

		public void Start(){
			// if this object is not inside of the play area, call special function
			// on trigger enter of the play area, this will be turned off
			if (!PlayAreaMovementStarter.Instance().IsInsidePlayArea(transform)){
				Debug.Log("I'm outside the play area, gonna move to the center.");
				_aiMovementController.EnableCenterPattern();
			}
		}
	}
}