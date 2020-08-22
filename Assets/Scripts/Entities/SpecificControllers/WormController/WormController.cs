using Entities.MovementPatterns;
using UnityEngine;

namespace Entities.SpecificControllers.WormController{
	/// <summary>
	/// A controller that applies worm movement, where the 'head' moves and rotates on it's own, and the body follows,
	/// after a set delay.
	/// </summary>
	public class WormController : MonoBehaviour{
	
		// the head object of the worm
		private Transform _head;

		// holds the actual body parts, initialized at wake
		private WormBodyController[] _bodyControllers;

		private void Awake(){
			// collect the head and body parts
			_head = GetComponentInChildren<AbstractMovementPattern>().gameObject.transform;
			_bodyControllers = GetComponentsInChildren<WormBodyController>();

			// tell all body parts where the head is, and which body part is next (if any)
			for (int i = 0; i < _bodyControllers.Length; i++){
				WormBodyController body = _bodyControllers[i];
				
				// if this is the first body part, initialize with the head
				if (i == 0){
					body.SetBodyPartInFront(_head);
				}else{
					// otherwise, you set this to the previous body controller
					body.SetBodyPartInFront(_bodyControllers[i - 1].transform);
				}
			}
		}
	}
}
