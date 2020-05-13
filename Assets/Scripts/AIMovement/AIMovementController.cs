using System;
using UnityEngine;

/// <summary>
/// Applies some pre-defined movement to the current object's movement.
///
/// Allows for swapping out movement types at runtime.
/// </summary>
public class AIMovementController : MonoBehaviour{

	private AbstractAIMovementPattern _aiMovementPattern;

	private void Awake(){
		_aiMovementPattern = GetComponent<AbstractAIMovementPattern>();

		if (_aiMovementPattern == null){
			Debug.Log("You did not attack an AI Movement Pattern object to this game object, it won't move!");
		}
	}

	private void Update(){
		// simply move this object based on it's defined pattern
		_aiMovementPattern.Move();
	}
}