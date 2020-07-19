
using System;
using UnityEngine;

public class PlayerBoundary : MonoBehaviour{

	private Collider2D _collider2D;
	
	private void Awake(){
		_collider2D = gameObject.GetComponent<Collider2D>();
		
		// when awoken, initially disable the collider
		DisableCollider();
	}

	public void DisableCollider(){
		_collider2D.enabled = false;
	}

	public void EnableCollider(){
		_collider2D.enabled = true;
	}
}