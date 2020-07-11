using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls leg movement and behaviors.
/// </summary>
public class LegsRotation : MonoBehaviour{

	private Quaternion _initialRotation;

	private void Awake(){
		_initialRotation = transform.rotation;
	}

	private void Update(){
		// stop the rotation being applied from any parent objects
		transform.rotation = _initialRotation;
	}
}
