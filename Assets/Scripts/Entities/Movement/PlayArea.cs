using Entities.Movement.Controllers;
using UnityEngine;

namespace Entities.Movement{
	public class PlayArea : MonoBehaviour{

		private Collider2D _collider2D;

		private void Awake(){
			_collider2D = GetComponent<Collider2D>();
		}

		private void OnTriggerEnter2D(Collider2D other){
			AbstractEntityMovementController controller = other.gameObject.GetComponent<AbstractEntityMovementController>();

			if (controller != null){
				controller.SetInsidePlayArea();
			}
		}

		public bool IsInPlayArea(GameObject obj){
			return _collider2D.bounds.Contains(obj.transform.position);
		}
	}
}