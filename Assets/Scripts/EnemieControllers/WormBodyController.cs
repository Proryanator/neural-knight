using System;
using UnityEngine;

/// <summary>
/// Controls the movement of each body part of a worm.
/// </summary>
public class WormBodyController : MonoBehaviour{

	private Transform _head;

	// stores the starting distance from the head, as to not move to the head fully
	private float _distanceFromHead;

	// set via the main WormController
	private float _rotateDelay;
	
	private void Start(){
		_distanceFromHead = Vector2.Distance(_head.position, transform.position);
	}

	private void Update(){
		MoveWithHead();
		RotateWithHead();
	}

	private void MoveWithHead(){
		transform.position = (transform.position - _head.position).normalized * _distanceFromHead + _head.position;
	}

	private void RotateWithHead(){
		transform.rotation = _head.rotation;
	}

	public void InitBody(Transform head, float rotationDelay){
		_head = head;
		_rotateDelay = rotationDelay;
	}
}