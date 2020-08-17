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
		
		// all movement information will be passed here first, then passed along by each body controller
		private WormBodyController _firstBodyPart;
	
		[Tooltip("The amount of time between each body telling the next about the heads position.")]
		[SerializeField] private float _bodyTellTime = 1f;

		private void Awake(){
			// collect the head and body parts
			FollowPlayerPattern pattern = GetComponentInChildren<FollowPlayerPattern>();
			_head = pattern.gameObject.transform;
			_bodyControllers = GetComponentsInChildren<WormBodyController>();
			_firstBodyPart = _bodyControllers[0];

			// tell all body parts where the head is, and which body part is next (if any)
			for (int i = 0; i < _bodyControllers.Length; i++){
				WormBodyController body = _bodyControllers[i];
				body.InitBody(_head);

				// set the next body controller to the next one in the list, just not for the last one
				if (i != _bodyControllers.Length - 1){
					body.Init(_bodyControllers[i+1], pattern.GetFaceTargetSpeed(), _bodyTellTime);
				}
				else{
					body.Init(null, pattern.GetFaceTargetSpeed(), _bodyTellTime);
				}
			}

		}

		private void Start(){
			// start a co-routine that will run ever so often to tell the body what to do
			StartCoroutine(WaitAndTellBody(_bodyTellTime));
		}

		/// <summary>
		/// Waits the amount of time, of which no more rotations can be taken for this object.
		/// </summary>
		private IEnumerator WaitAndTellBody(float bodyTellTime){
			while (true){
				// pass this information to the first body part
				_firstBodyPart.SetCurrentHeadTransform(_head);
				yield return new WaitForSeconds(bodyTellTime);
			}
		}
	}
}
