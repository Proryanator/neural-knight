using System.Collections;
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
		private WormBodyController _firstBodyPart;
	
		[Tooltip("Seconds to delay rotation of body.")]
		[SerializeField] private float _rotationDelay = 1f;

		private void Awake(){
			// collect the head and body parts
			_head = GetComponentInChildren<AbstractAIMovementPattern>().gameObject.transform;
			_bodyControllers = GetComponentsInChildren<WormBodyController>();
			_firstBodyPart = _bodyControllers[0];
		
			// tell all body parts where the head is, and which body part is next (if any)
			for (int i = 0; i < _bodyControllers.Length; i++){
				WormBodyController body = _bodyControllers[i];
				body.InitBody(_head);

				// set the next body controller to the next one in the list, just not for the last one
				if (i != _bodyControllers.Length - 1){
					body.SetNextBodyController(_bodyControllers[i+1]);
				}
			}
		}

		private void Start(){
			// start a co-routine that will run ever so often to tell the body what to do
			StartCoroutine(WaitAndTellBody(_rotationDelay));
		}

		/// <summary>
		/// Waits the amount of time, of which no more rotations can be taken for this object.
		/// </summary>
		private IEnumerator WaitAndTellBody(float delay){
			while (true){
				// pass this information to the first body part
				_firstBodyPart.RotateTowardsLeader(_head.rotation, delay);
				yield return new WaitForSeconds(delay);
			}
		}
	}
}
