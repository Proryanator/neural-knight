using Entities.Movement.Controllers;
using UnityEngine;

namespace Entities.Movement{
	public class PlayArea : MonoBehaviour{

		private static PlayArea _instance;

		private void Awake(){
			if (_instance == null){
				_instance = this;
			}
			else if (_instance != this){
				Destroy(gameObject);
			}
		}

		private void OnCollisionEnter2D(Collision2D other){
			AbstractEntityMovementController controller = other.gameObject.GetComponent<AbstractEntityMovementController>();

			if (controller != null){
				controller.SetInsidePlayArea();
			}
		}
	}
}