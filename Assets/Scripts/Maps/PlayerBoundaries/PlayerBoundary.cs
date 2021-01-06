using System.Collections;
using UnityEngine;

namespace Maps.PlayerBoundaries{
	public class PlayerBoundary : MonoBehaviour{

		private Collider2D _collider2D;

		private Transform _childSpriteAnimation;
	
		// all player boundaries in the scene, useful for turning off all boundaries in one shot
		private static ArrayList _allPlayerBoundaries = new ArrayList();
		
		private void Awake(){
			_collider2D = gameObject.GetComponent<Collider2D>();

			_childSpriteAnimation = transform.GetChild(0);
		
			// when awoken, initially disable the collider
			DisableCollider();
			
			// add myself to the list of current player boundaries
			_allPlayerBoundaries.Add(this);
		}

		public static void DisableAllPlayerBoundaries(){
			// for each one in the list, disable them
			foreach (PlayerBoundary boundary in _allPlayerBoundaries){
				boundary.DisableCollider();
			}

			_allPlayerBoundaries.Clear();
		}

		public void DisableCollider(){
			_collider2D.enabled = false;
			_childSpriteAnimation.gameObject.SetActive(false);
		}

		public void EnableCollider(){
			_collider2D.enabled = true;
			_childSpriteAnimation.gameObject.SetActive(true);
		}
	}
}