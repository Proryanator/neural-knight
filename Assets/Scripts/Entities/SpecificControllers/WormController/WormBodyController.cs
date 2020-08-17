using System.Collections;
using UnityEngine;

namespace Entities.SpecificControllers.WormController{
	/// <summary>
	/// Controls the movement of each body part of a worm.
	/// </summary>
	public class WormBodyController : MonoBehaviour{

		// stores the heads current transform, updated by the body chain
		private Transform _currentHeadTransform;

		// how quickly to rotate to face the head's rotation
		private float _rotationSpeedFromHead;

		// how long to wait before telling the next body part to start moving
		private float _bodyTellTime;
		
		// stores the starting distance from the head, as to not move to the head fully
		private float _distanceFromHead;

		// stores a linked list of the next body part versus the previous
		private WormBodyController _nextBodyController = null;
	
		// true when you're waiting to send the data along to the next body part
		private bool _isWaiting = false;

		private void Start(){
			_distanceFromHead = Vector2.Distance(_currentHeadTransform.position, transform.position);
		}

		private void Update(){
			// change your transform with the head always
			MoveWithHead();
			
			// rotate towards the head's position over time always
			transform.rotation = Quaternion.RotateTowards(_currentHeadTransform.rotation, transform.rotation, Time.deltaTime * _rotationSpeedFromHead);
			
			// send information to the next body, after waiting an amount of time
			// only if you're not currently doing that
			if (!_isWaiting){
				StartCoroutine(WaitAndTellNextBody(_currentHeadTransform, _bodyTellTime));
			}
		}

		public void Init(WormBodyController controller, float rotationSpeed, float bodyTellTime){
			_nextBodyController = controller;
			_rotationSpeedFromHead = rotationSpeed;
			_bodyTellTime = bodyTellTime;
		}
	
		// head rotating, we want the body to match it's rotation X number of time later, then tell the body below it to do the same with the same rotation
		// the head will start this, always sending it's current rotation down to the top-most body part.
		// that body part will rotate, then do a wait, then pass that down to the next body part, and repeat
		// the head should call this method every rotationDelay, which will start the chain
		public void SetCurrentHeadTransform(Transform head){
			_currentHeadTransform = head;
		}

		/// <summary>
		/// Waits a certain amount of time before passing the information down to the next body part.
		/// </summary>
		private IEnumerator WaitAndTellNextBody(Transform newHeadTransform, float delay){
			_isWaiting = true;
			yield return new WaitForSeconds(delay);
			_isWaiting = false;
			
			// only if your next body controller is defined, pass it along
			if (_nextBodyController != null){
				_nextBodyController.SetCurrentHeadTransform(newHeadTransform);
			}
		}

		/// <summary>
		/// Moves the current body to the heads location, with a lerp + keeping initial distance from the head.
		/// </summary>
		private void MoveWithHead(){
			transform.position = (transform.position - _currentHeadTransform.position).normalized * _distanceFromHead + _currentHeadTransform.position;
		}

		// call this once when you're supposed to reflect the rotation of the head, or the object in front of you
		private void RotateWithHead(){
			transform.rotation = _currentHeadTransform.rotation;
		}

		public void InitBody(Transform head){
			_currentHeadTransform = head;
		}
	}
}