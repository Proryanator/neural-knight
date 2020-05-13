using System;
using UnityEngine;

/// <summary>
/// Applies some pre-defined movement to the current object's movement.
///
/// Allows for swapping out movement types at runtime.
/// </summary>
public class AIMovementController : MonoBehaviour{

	private AbstractAIMovementPattern _initialAIMovementPattern;
	private AbstractAIMovementPattern _aiMovementPattern;

	private void Awake(){
		// remember the initial movement pattern
		_initialAIMovementPattern = GetComponent<AbstractAIMovementPattern>();

		// we'll start with the initial movement pattern to begin with
		_aiMovementPattern = _initialAIMovementPattern;
		
		if (_aiMovementPattern == null){
			Debug.Log("You did not attack an AI Movement Pattern object to this game object, it won't move!");
		}
	}

	private void Update(){
		// simply move this object based on it's defined pattern
		_aiMovementPattern.Move();
	}

	/// <summary>
	/// Allows you to set the movement pattern for this controller.
	/// </summary>
	public void SetMovementPattern(AbstractAIMovementPattern pattern){
		_aiMovementPattern = pattern;
	}

	/// <summary>
	/// Restores what the original movement pattern was for this controller.
	/// </summary>
	public void RestoreOriginalMovementPattern(){
		_aiMovementPattern = _initialAIMovementPattern;
	}
}