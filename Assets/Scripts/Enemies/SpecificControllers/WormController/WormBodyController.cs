using System.Collections;
using UnityEngine;

namespace Enemies.SpecificControllers.WormController{
	/// <summary>
	/// Controls the movement of each body part of a worm.
	/// </summary>
	public class WormBodyController : MonoBehaviour{

		private Transform _head;

		// stores the starting distance from the head, as to not move to the head fully
		private float _distanceFromHead;

		// stores a linked list of the next body part versus the previous
		private WormBodyController _nextBodyController = null;
	
		private bool _isWaiting = false;
	
		private void Start(){
			_distanceFromHead = Vector2.Distance(_head.position, transform.position);
		}

		public void SetNextBodyController(WormBodyController controller){
			_nextBodyController = controller;
		}
	
		// head rotating, we want the body to match it's rotation X number of time later, then tell the body below it to do the same with the same rotation
		// the head will start this, always sending it's current rotation down to the top-most body part.
		// that body part will rotate, then do a wait, then pass that down to the next body part, and repeat
		// the head should call this method every rotationDelay, which will start the chain
		public void RotateTowardsLeader(Quaternion leaderRotation, float rotationDelay){
			// first, apply that rotation
			transform.rotation = leaderRotation;
		
			// then, delay before sending this down to the next one
			StartCoroutine(WaitDelayAmount(leaderRotation, rotationDelay));

		}


		/// <summary>
		/// Waits the amount of time, of which no more rotations can be taken for this object.
		/// </summary>
		private IEnumerator WaitDelayAmount(Quaternion leaderRotation, float delay){
			yield return new WaitForSeconds(delay);
		
			// only if your next body controller is defined, pass it along
			if (_nextBodyController != null){
				_nextBodyController.RotateTowardsLeader(leaderRotation, delay);
			}
		}
	
		private void Update(){
			MoveWithHead();
		}

		private void MoveWithHead(){
			transform.position = (transform.position - _head.position).normalized * _distanceFromHead + _head.position;
		}

		// call this once when you're supposed to reflect the rotation of the head, or the object in front of you
		private void RotateWithHead(){
			transform.rotation = _head.rotation;
		}

		public void InitBody(Transform head){
			_head = head;
		}
	}
}