using UnityEngine;

namespace Player.Animations{
	/// <summary>
	/// Prevents rotations from parents from being applied to this object.
	/// </summary>
	public class PreventParentRotation : MonoBehaviour{

		private Quaternion _initialRotation;

		private void Awake(){
			_initialRotation = transform.rotation;
		}

		private void Update(){
			// stop the rotation being applied from any parent objects
			transform.rotation = _initialRotation;
		}
	}
}