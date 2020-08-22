using UnityEngine;

namespace Entities.SpecificControllers.WormController{
	/// <summary>
	/// Controls the movement of each body part of a worm.
	///
	/// Each body part will stay within it's initial distance of the body part in front of it, which may be the head.
	/// </summary>
	public class WormBodyController : MonoBehaviour{

		// the transform of the body part in front, could be the head!
		private Transform _bodyInFront;

		// stores the starting distance from the front body part, as to not move away from the head all the way
		private float _distanceFromBodyInFront;

		private void Start(){
			_distanceFromBodyInFront = Vector2.Distance(_bodyInFront.position, transform.position);
		}

		private void Update(){
			MaintainDistanceFromFrontBody();
		}

		/// <summary>
		/// If the front body part gets farther away from your initial setting, move closer to it!
		/// Otherwise, don't move at all.
		/// </summary>
		private void MaintainDistanceFromFrontBody(){
			// only move closer to the front body part if you're farther away than the initial distance!
			float distance = Vector2.Distance(transform.position, _bodyInFront.position);
			if (distance > _distanceFromBodyInFront){
				transform.position = (transform.position - _bodyInFront.position).normalized * _distanceFromBodyInFront + _bodyInFront.position;

				// also, apply the rotation
				transform.rotation = _bodyInFront.rotation;
			}
		}

		/// <summary>
		/// Call this to set the body part that this body part is to follow.
		/// </summary>
		public void SetBodyPartInFront(Transform frontTransform){
			_bodyInFront = frontTransform;
		}
	}
}