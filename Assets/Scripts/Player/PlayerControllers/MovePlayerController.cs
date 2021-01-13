using Player.PlayerControllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
	public class MovePlayerController : NewTopDownController{

		private new void Start(){
			base.Start();
		}
		
		public override void SetDirection(InputAction.CallbackContext context) {
			// do nothing with player input here
		}

		public void SetDirectionToMovePlayer(Vector2 newDirection){
			direction = newDirection;
		}
	}
}