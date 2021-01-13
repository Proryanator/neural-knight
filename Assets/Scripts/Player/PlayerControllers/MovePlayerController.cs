using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
	public class MovePlayerController : TopDownController{

		private new void Start(){
			base.Start();
		}

		private new void FixedUpdate() {
			base.FixedUpdate();
		}

		public override void SetDirection(InputAction.CallbackContext context) {
			// do nothing with player input here
		}

		public void SetDirectionToMovePlayer(Vector2 newDirection){
			direction = newDirection;
		}
	}
}