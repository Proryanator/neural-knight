using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller that applies worm movement, where the 'head' moves and rotates on it's own, and the body follows,
/// after a set delay.
/// </summary>
public class WormController : MonoBehaviour{
	
	// the head object of the worm
	private Transform _head;

	// holds the actual body parts, initialized at wake
	private WormBodyController[] _bodyControllers;
	
	[Tooltip("Seconds to delay rotation of body.")]
	[SerializeField] private float _rotationDelay = 1f;

	private void Awake(){
		// collect the head and body parts
		_head = GetComponentInChildren<AbstractAIMovementPattern>().gameObject.transform;
		_bodyControllers = GetComponentsInChildren<WormBodyController>();

		// tell all body parts where the head is
		foreach (WormBodyController body in _bodyControllers){
			body.InitBody(_head, _rotationDelay);
		}
	}
}
