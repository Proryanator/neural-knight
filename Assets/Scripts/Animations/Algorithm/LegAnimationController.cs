using System;
using System.Diagnostics;
using System.Numerics;
using Proryanator.Controllers2D;
using UnityEngine;
using Debug = UnityEngine.Debug;
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

	[Tooltip("If true, draws a debug line in the direction of the legs facing direction.")]
	[SerializeField] private bool _drawLegDirection = false;
	
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

		if (_drawLegDirection){
			Debug.DrawLine(transform.position, Vector3.Cross(Vector2.up, transform.rotation.eulerAngles), Color.red);
		}
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
		// TODO: this is set in the TDC_FaceMouse controller, we should expose this variable
		FacingDirection _spriteFacingDirection = FacingDirection.UP;
		Vector2 facingDirection = _faceMouseController.GetFacingDirection();
		
		// new rotation, defaults to the direction of the facing direction
		Quaternion newRotation = GetRotationForStartingDirection(GetAngle(facingDirection), _spriteFacingDirection);
		
		// no movement? Face legs in the direction of facing mouse
		if (moveDirection.Equals(Vector2.zero)){
			return newRotation;
		}
		
		Quaternion rotationToFaceMovement = GetRotationForStartingDirection(GetAngle(moveDirection), _spriteFacingDirection);
		
		// what's the angle between the facing direction and the moving direction?
		float angleBetweenThem = Vector2.SignedAngle(moveDirection, facingDirection);
		float absoluteAngleBetween = Mathf.Abs(angleBetweenThem);
		// Debug.Log("Angle between facing direction and moving direction: " + angleBetweenThem);
		// if we're within the normal threshold allowed, then just return the direction of movement
		// otherwise, we're considered 'moving backwards'
		if (absoluteAngleBetween < _turnAngleThreshold){
			newRotation = rotationToFaceMovement;
		}else if (absoluteAngleBetween < 90){
			// the angle will depend on the angle between and whether it is to the right or left
			switch (GetAngleDirection(facingDirection, moveDirection)){
				case AngleDirection.Left:
					float leftAngle = GetAngle(facingDirection) + (absoluteAngleBetween - _turnAngleThreshold);
					newRotation = GetRotationForStartingDirection(leftAngle, _spriteFacingDirection);
					break;
				case AngleDirection.Right:
					float rightAngle = GetAngle(facingDirection) - (absoluteAngleBetween - _turnAngleThreshold);
					newRotation = GetRotationForStartingDirection(rightAngle, _spriteFacingDirection);
					break;
				case AngleDirection.Aligned:
					// if they're the same direction, we've already set the new rotation correctly
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}else{
			newRotation = Quaternion.Inverse(rotationToFaceMovement);
		}

		Debug.Log("New Rotation: " + newRotation.eulerAngles);
		return newRotation;
	}

	private Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction) {
		return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
	}

	private float GetAngle(Vector2 direction){
		return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	}

	/// <summary>
	/// Calculates if the given vector is to the left or right of another.
	///
	/// COUGH acquired from stack overflow, thank you!
	/// </summary>
	/// <returns>Left if angle is left, right if right, aligned if they're the exact same.</returns>
	private AngleDirection GetAngleDirection(Vector2 facingDirection, Vector2 moveDirection){
		float direction = -facingDirection.x * moveDirection.y + facingDirection.y * moveDirection.x;

		if (direction == 0){
			return AngleDirection.Aligned;
		}

		if (Mathf.Sign(direction) == -1){
			return AngleDirection.Left;
		}
		
		// otherwise, this is to the right

		return AngleDirection.Right;
	}
}