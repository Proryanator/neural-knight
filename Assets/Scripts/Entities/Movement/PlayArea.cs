using UnityEngine;

namespace Entities.Movement{
	public class PlayArea : MonoBehaviour{

		private static PlayArea _instance;

		private Collider2D _collider2D;
		
		private void Awake(){
			if (_instance == null){
				_instance = this;

				_collider2D = GetComponent<Collider2D>();
			}
			else if (_instance != this){
				Destroy(gameObject);
			}
		}

		public static PlayArea Instance(){
			return _instance;
		}
		
		public bool IsInsidePlayArea(Transform transform){
			return _collider2D.bounds.Contains(transform.position);
		}
	}
}