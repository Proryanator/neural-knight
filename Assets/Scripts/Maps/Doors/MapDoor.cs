using UnityEngine;

namespace Maps {
	public class MapDoor : MonoBehaviour {

		private SpriteRenderer _spriteRenderer;
		private Collider2D _collider2D;

		private void Awake(){
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_collider2D = GetComponent<Collider2D>();
		}

		public void EnableDoor(){
			_collider2D.enabled = true;
			_spriteRenderer.enabled = true;
		}

		public void DisableDoor(){
			_collider2D.enabled = false;
			_spriteRenderer.enabled = false;
		}
	}
}