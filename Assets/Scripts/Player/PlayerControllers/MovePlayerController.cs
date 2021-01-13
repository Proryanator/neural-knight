using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace Player{
	public class MovePlayerController : TopDownController{

		private new void Start(){
			base.Start();
		}

		private new void FixedUpdate() {
			base.FixedUpdate();
			
			// rotate the body in the direction of the movement (NOTE: facing direction is hard coded)
			transform.rotation = Utils2D.GetRotationTowardsDirection(direction, FacingDirection.UP);
		}

		public override void SetDirection(InputAction.CallbackContext context) {
			// do nothing with player input here
		}

		public void SetDirectionToMovePlayer(Vector2 newDirection){
			direction = newDirection;
		}
	}
}