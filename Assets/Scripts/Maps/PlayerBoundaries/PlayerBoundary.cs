using UnityEngine;

namespace Maps.PlayerBoundaries{
	public class PlayerBoundary : MonoBehaviour{

		private Collider2D _collider2D;

		private Transform _childSpriteAnimation;
	
		private void Awake(){
			_collider2D = gameObject.GetComponent<Collider2D>();

			_childSpriteAnimation = transform.GetChild(0);
		
			// when awoken, initially disable the collider
			DisableCollider();
		}

		public void DisableCollider(){
			_collider2D.enabled = false;
		
			// disable all children
			_childSpriteAnimation.gameObject.SetActive(false);
		}

		public void EnableCollider(){
			_collider2D.enabled = true;
			_childSpriteAnimation.gameObject.SetActive(true);
		}
	}
}