using System;
using System.Numerics;
using Proryanator.Controllers2D;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Will send information between the TopDownController and the
/// Animation component on this object.
///
/// NOTE: requires there to be an Animator object attached!
/// </summary>
public class LegAnimationController : MonoBehaviour{

	// we'll need a reference to the animator on this object
	private Animator _animator;

	// also needing a reference to the controller! Will be in the parent
	// TODO: when we've moved those updates into the module, you can make this generic to TopDownController!
	private New_TopDownController _controller;

	private void Awake(){
		_animator = GetComponent<Animator>();
		_controller = GetComponentInParent<New_TopDownController>();
	}

	// NOTE: using fixed update, to make sure it's called in-line with the physics based movement system
	private void FixedUpdate(){
		Vector2 moveDirection = _controller.GetMovingDirection();
		// determine the movement direction, and face this object in that direction
		RotateLegsTowardMovementDirection(moveDirection);
		
		// set the movement speed of the animator, if any
		// if any input is received (as in, if it's not a zero vector), this will be set
		if (!moveDirection.Equals(Vector2.zero)){
			_animator.SetFloat("Speed", 1f);
		}
		else{
			_animator.SetFloat("Speed", 0);
		}
	}

	/// <summary>
	/// Rotates the legs towards the movement direction.
	/// </summary>
	private void RotateLegsTowardMovementDirection(Vector2 directionToFace) {
		// don't do this if there's no movement
		if (directionToFace.Equals(Vector2.zero)){
			return;
		}
		
		float angle = Mathf.Atan2(directionToFace.y, directionToFace.x) * Mathf.Rad2Deg;

		// taking initial direction into account, now rotate towards the mouse!
		transform.rotation = GetRotationForStartingDirection(angle, FacingDirection.UP);
	}

	protected Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction) {
		return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
	}

	/// <summary>
	/// TODO: implement me! Will play the backwards walking animation if the player is
	/// facing left, but walking right.
	/// </summary>
	private void WalkBackwardsIfFacingOpposite(){
		
	}
}