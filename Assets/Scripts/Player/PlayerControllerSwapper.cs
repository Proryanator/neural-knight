using Proryanator.Controllers2D;
using UnityEngine;

namespace Player {
	public class PlayerControllerSwapper : MonoBehaviour {

		private TDC_FaceMouse _playerController;
		private MovePlayerController _movePlayerController;

		private void Awake(){
			_playerController = GetComponent<TDC_FaceMouse>();
			_movePlayerController = GetComponent<MovePlayerController>();
		}

		public void EnablePlayerControl(){
			_playerController.enabled = true;
			_movePlayerController.enabled = false;
		}

		public void EnableAutoWalk(Vector2 direction){
			_playerController.enabled = false;
			_movePlayerController.SetDirectionToMovePlayer(direction);
			_movePlayerController.enabled = true;
		}
	}
}