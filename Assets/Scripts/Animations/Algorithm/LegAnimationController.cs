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
	private TDC_FaceMouse _faceMouseController;

	// this is the angle threshold that you're allowed to be in for your legs to be moving
	// in the same direction; otherwise, you'll be moving backwards (with the angle
	// to face calculated here too
	[Tooltip("The maximum angle difference between your movement direction and facing direction.")]
	[SerializeField] private float _turnAngleThreshold = 30;
	
	private void Awake(){
		_animator = GetComponent<Animator>();
		_faceMouseController = GetComponentInParent<TDC_FaceMouse>();
	}

	// NOTE: using fixed update, to make sure it's called in-line with the physics based movement system
	private void FixedUpdate(){
		Vector2 moveDirection = _faceMouseController.GetDirection();
		// determine the movement direction, and face this object in that direction
		RotateLegsTowardMovementDirection(moveDirection);
		
		// set the movement speed of the animator, if any
		// if any input is received (as in, if it's not a zero vector), this will be set
		// TODO: might want to make this the actual amount you're moving, to allow for speed to vary the animation speed
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
	private void RotateLegsTowardMovementDirection(Vector2 moveDirection) {
		transform.rotation = ChooseLegDirection(moveDirection);
	}
	
	/// <summary>
	/// If there's no movement, point your legs in the facing direction.
	///
	/// If there is movement, and you're within the threshold, face legs
	/// towards the movement.
	///
	/// If there is movement but outside of the threshold, apply 'backwards'
	/// movement logic.
	/// </summary>
	/// <returns>The correct rotation based on your current move direction + facing direction.</returns>
	private Quaternion ChooseLegDirection(Vector2 moveDirection){
		Quaternion newRotation = Quaternion.identity;
		
		// TODO: this is set in the TDC_FaceMouse controller, we should expose this variable
		FacingDirection _spriteFacingDirection = FacingDirection.UP;
		Vector2 facingDirection = _faceMouseController.GetFacingDirection();
		
		// no movement? Face legs in the direction of facing mouse
		if (moveDirection.Equals(Vector2.zero)){
			return GetRotationForStartingDirection(GetAngle(facingDirection), _spriteFacingDirection);
		}
		
		Quaternion rotationToFaceMovement = GetRotationForStartingDirection(GetAngle(moveDirection), _spriteFacingDirection);
		
		// what's the angle between the facing direction and the moving direction?
		float angleBetweenThem = Vector2.Angle(moveDirection, facingDirection);
		
		// if we're within the normal threshold allowed, then just return the direction of movement
		// otherwise, we're considered 'moving backwards'
		if (angleBetweenThem < _turnAngleThreshold){
			newRotation = rotationToFaceMovement;
		}else if (angleBetweenThem < 90){
			Debug.Log("Within 90 part, what to do?");
		}else{
			Debug.Log("Angle is past 90 threshold, facing legs inverse of move direction.");
			newRotation = Quaternion.Inverse(rotationToFaceMovement);
		}

		return newRotation;
	}

	private Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction) {
		return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
	}

	private float GetAngle(Vector2 direction){
		return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	}
}