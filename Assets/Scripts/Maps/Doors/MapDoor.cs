using System.Collections.Generic;
using UnityEngine;

namespace Maps {
	public class MapDoor : MonoBehaviour {

		[SerializeField] private DoorDirection _doorDirection;
		
		private SpriteRenderer _spriteRenderer;
		private Collider2D _collider2D;
		private List<Transform> _childSpawnPoints = new List<Transform>();
		
		private void Awake(){
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_collider2D = GetComponent<Collider2D>();

			foreach (Transform child in transform){
				_childSpawnPoints.Add(child);
			}
		}

		public void EnableDoor(){
			_collider2D.enabled = true;
			_spriteRenderer.enabled = true;
			SetAllChildrenToDisabled(_childSpawnPoints, false);
		}

		public void DisableDoor(){
			_collider2D.enabled = false;
			_spriteRenderer.enabled = false;
			SetAllChildrenToDisabled(_childSpawnPoints, true);
		}

		public DoorDirection GetDoorDirection(){
			return _doorDirection;
		}

		private void SetAllChildrenToDisabled(List<Transform> children, bool enabled){
			foreach (var child in children){
				child.gameObject.SetActive(enabled);
			}
		}
	}
}