using UnityEngine;

namespace Player.Animations {
	public class BodyAnimationController : MonoBehaviour {
		
		private Animator _animator;

		private void Awake(){
			_animator = GetComponent<Animator>();
		}

		// method to be called when the player shoots his weapon
		public void ActivateShotAnimation(){
			// we'll ensure the animation plays from the beginning each time
			_animator.Play("perceptron-torso-shot", 0, 0.0f);
		}
	}
}