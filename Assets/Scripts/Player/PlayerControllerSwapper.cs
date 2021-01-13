using Player.Animations;
using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
	public class PlayerControllerSwapper : MonoBehaviour {

		private TDC_FaceMouse _playerController;
		private MovePlayerController _movePlayerController;
		private LegAnimationController _legAnimationController;
		// let's disable player input (preventing any player input)
		// TODO: might make this sort of player interruption available outside this class
		private PlayerInput _playerInput;
		
		private void Awake(){
			_playerController = GetComponent<TDC_FaceMouse>();
			_movePlayerController = GetComponent<MovePlayerController>();
			_legAnimationController = GetComponentInChildren<LegAnimationController>();
			_playerInput = FindObjectOfType<PlayerInput>();
		}

		public void EnablePlayerControl(){
			_playerController.enabled = true;
			_movePlayerController.enabled = false;
			_playerInput.enabled = true;
			_legAnimationController.UseFaceMouseController();
		}

		public void EnableAutoWalk(Vector2 direction){
			_playerController.enabled = false;
			_movePlayerController.SetDirectionToMovePlayer(direction);
			_movePlayerController.enabled = true;
			_playerInput.enabled = false;
			_legAnimationController.UseMoveController();
		}
	}
}