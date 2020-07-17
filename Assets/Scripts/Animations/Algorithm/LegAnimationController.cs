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

	[SerializeField] private float _legRotationSpeed = 1f;
	
	[Tooltip("If true, draws a debug line in the direction of the legs facing direction.")]
	[SerializeField] private bool _drawDebugLines = false;

	// storing statistics about the current movement here
	private FacingDirection _spriteFacingDirection;
	private Vector2 _moveDirection;
	private Vector2 _facingDirection;
	
	// some standard rotation values, also calculated per frame
	private Quaternion _rotationTowardsFacingDirection;
	private Quaternion _rotationTowardsMovement;
	
	// this is the angle threshold that you're allowed to be in for your legs to be moving
	// in the same direction; otherwise, you'll be moving backwards (with the angle
	// to face calculated here too
	[Tooltip("The maximum angle difference between your movement direction and facing direction.")]
	[SerializeField] private float _turnAngleThreshold = 30;
	
	private void Awake(){
		_animator = GetComponent<Animator>();
		_faceMouseController = GetComponentInParent<TDC_FaceMouse>();
		_spriteFacingDirection = _faceMouseController.GetStartingDirection();
	}

	// NOTE: using fixed update, to make sure it's called in-line with the physics based movement system
	private void FixedUpdate(){
		// get and store relevant information about player's movement
		GetMovementInfo();
		
		// determine the movement direction, and face this object in that direction
		RotateLegsTowardMovementDirection();
		
		// set the movement speed of the animator, if any
		// if any input is received (as in, if it's not a zero vector), this will be set
		// TODO: might want to make this the actual amount you're moving, to allow for speed to vary the animation speed
		if (!_moveDirection.Equals(Vector2.zero)){
			_animator.SetFloat("Speed", 1f);
		}
		else{
			_animator.SetFloat("Speed", 0);
		}
	}

	/// <summary>
	/// Reads + calculates some information about current facing direction + movement.
	///
	/// All used later to calculate the direction of the legs.
	/// </summary>
	private void GetMovementInfo(){
		_moveDirection = _faceMouseController.GetDirection();
		_facingDirection = _faceMouseController.GetFacingDirection();
		
		// new rotation, defaults to the direction of the facing direction
		_rotationTowardsFacingDirection = GetRotationWithStartingDirection(GetAngle(_facingDirection), _spriteFacingDirection);
		_rotationTowardsMovement = GetRotationWithStartingDirection(GetAngle(_moveDirection), _spriteFacingDirection);
	}
	
	/// <summary>
	/// Rotates the legs towards the movement direction.
	/// </summary>
	private void RotateLegsTowardMovementDirection() {
		// no movement? Face legs in the direction of facing mouse
		if (_moveDirection.Equals(Vector2.zero) || _rotationTowardsMovement.Equals(_rotationTowardsFacingDirection)){
			transform.rotation = _rotationTowardsFacingDirection;
		}
		else{
			// otherwise let's calculate the leg direction
			Quaternion newRotation = ChooseLegDirection();
			// apply a lerp to the rotation, to make it smoother
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.fixedDeltaTime * _legRotationSpeed);
		}

		if (_drawDebugLines){
			// draw the facing direction's line
			Debug.DrawLine(transform.position, (Vector2)transform.position + (_faceMouseController.GetFacingDirection().normalized * 5), Color.green);
			// draw the movement direction's line
			Debug.DrawLine(transform.position, (Vector2)transform.position + (_moveDirection.normalized * 5), Color.blue);
			// draw the facing direction of the legs
			Debug.DrawLine(transform.position, (Vector2) transform.position + ((Vector2) transform.up * 5), Color.red);
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
	///
	/// Also, will change the speed to be either positive or negative, depending on the direction.
	/// </summary>
	/// <returns>The correct rotation based on your current move direction + facing direction.</returns>
	private Quaternion ChooseLegDirection(){
		Tuple<Quaternion, bool> isForward = GetForwardAngleIfMovingForward();
		if (isForward.Item2){
			return isForward.Item1;
		}
		
		// we're considered 'backwards' at this point, so reverse the animation and calculate the angle
		_animator.SetFloat("Direction", -1);
		return GetBackwardAngle();
	}

	/// <summary>
	/// Gets you the proper leg facing direction based on whether you're facing forward.
	///
	/// Returns Quaternion.identify if you're not facing forward.
	///
	/// Returns the rotation, as well as true/false if you're moving backwards or not
	/// </summary>
	private Tuple<Quaternion, bool> GetForwardAngleIfMovingForward(){
		// what's the angle between the facing direction and the moving direction?
		// note: this angle will always be 180/-180, as the angle between 2 vectors is stuck here
		float angleBetweenThem = Quaternion.Angle(_rotationTowardsMovement, _rotationTowardsFacingDirection);
		float absoluteAngleBetween = Mathf.Abs(angleBetweenThem);

		// if we're within the normal threshold allowed, then just return the direction of movement
		if (absoluteAngleBetween < _turnAngleThreshold){
			_animator.SetFloat("Direction", 1);
			
			return Tuple.Create(_rotationTowardsMovement, true);
		}
		
		// if we get here, we're not moving forward
		return Tuple.Create(Quaternion.identity, false);
	}
	
	/// <summary>
	/// Calculates the angle to rotate your legs when considered 'backwards'.
	/// </summary>
	private Quaternion GetBackwardAngle(){
		float absoluteAngleBetween = Mathf.Abs(Quaternion.Angle(_rotationTowardsMovement, _rotationTowardsFacingDirection));
		
		if (absoluteAngleBetween > 90){
			// this FIXES all things: by adding 270, we convert to our own space of angles!
			// then, rotate based on the starting sprite direction
			return GetRotationWithStartingDirection(_rotationTowardsMovement.eulerAngles.z + 270, _spriteFacingDirection);
		}
		
		// NOTE: case of whether you're perfect aligned is handled in the forward check, not here
		float angle = 0f;
		switch (GetAngleDirection(_facingDirection, _moveDirection)){
			case AngleDirection.Left:
				angle = GetAngle(_facingDirection) + (absoluteAngleBetween - _turnAngleThreshold);
				break;
			case AngleDirection.Right:
				angle = GetAngle(_facingDirection) - (absoluteAngleBetween - _turnAngleThreshold);
				break;
		}
		
		return GetRotationWithStartingDirection(angle, _spriteFacingDirection);
	}
	
	private Quaternion GetRotationWithStartingDirection(float angle, FacingDirection direction) {
		return Quaternion.AngleAxis((angle + (float)(direction)) % 360, Vector3.forward);
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