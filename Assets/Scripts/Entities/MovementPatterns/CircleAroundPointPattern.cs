using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	
	/// <summary>
	/// Moves the object around a point, in a circle radius.
	///
	/// If set properly can just move to the center of the map instead.
	/// </summary>
	public class CircleAroundPointPattern : AbstractMovementPattern{
		
		[Tooltip("How far from the point to rotate outwards from. NOTE: this is used to create a slightly larger radius for movement purposes.")]
		[SerializeField] private float _radius = 1f;

		[Tooltip("Speed to move towards the center of the map")]
		[SerializeField] private float _moveSpeed = 5f;

		[Tooltip("True if you want to use a point at the center of the map.")]
		[SerializeField] private bool _useCenterOfMap = false;
		
		// point to rotate around
		private Transform _rotationPoint;

		private CircleCollider2D _circleCollider2DOfPoint;
		
		private void Awake(){
			if (_useCenterOfMap){
				// for now, this is just the center of the map
				_rotationPoint = GameObject.FindGameObjectWithTag(AllTags.MAP_CENTRAL_POINT).transform;
			}
			else{
				_rotationPoint = GetRandomFocusPoint();
			}

			// use the circle collider if there is one, or add one if not
			_circleCollider2DOfPoint = _rotationPoint.GetComponent<CircleCollider2D>();
			if (_circleCollider2DOfPoint == null){
				_circleCollider2DOfPoint = _rotationPoint.gameObject.AddComponent<CircleCollider2D>();
			}
			
			// make sure this is a trigger, otherwise it'll collide :o
			_circleCollider2DOfPoint.isTrigger = true;
			
			_circleCollider2DOfPoint.radius = _radius;
		}

		private Transform GetRandomFocusPoint(){
			// get all focus points in the scene
			GameObject[] focusPoints = GameObject.FindGameObjectsWithTag(AllTags.DATA_FOCUS_POINT);
			
			// randomly choose an available rotation spot
			return focusPoints[Random.Range(0, focusPoints.Length)].transform;
		}

		private void OnDrawGizmos(){
			// draw this circle as a gizmo so we can see how large this circle is
			Gizmos.DrawWireSphere(_rotationPoint.position, _circleCollider2DOfPoint.radius);
		}

		public override void Move(){
			Vector2 directionOfMovement;
			
			if (IsInsideOfCircle()){
				// remember transform from before
				Vector2 oldPosition = transform.position;
			
				transform.RotateAround(_rotationPoint.position, Vector3.back, Time.deltaTime * (_moveSpeed * 20));
			
				// now, let's calculate the direction from one point to another
				directionOfMovement = (Vector2) transform.position - oldPosition;
			}
			else{
				// start moving towards the center, like you do with the MoveToCenterPattern
				MoveToCenterMovementPattern.MoveToCenterMovement(transform, _rotationPoint, _moveSpeed);
				
				// move towards the center until you're inside of the circle
				directionOfMovement = (Vector2) _rotationPoint.position - (Vector2) transform.position;
			}

			// no matter what, you'll want to face your movement direction
			transform.rotation = Utils2D.GetRotationTowardsDirection(directionOfMovement, startingDirection);
		}

		/// <summary>
		/// Determine if the enemy is inside of the circle to begin the movement!
		/// </summary>
		private bool IsInsideOfCircle(){
			return _circleCollider2DOfPoint.bounds.Contains(transform.position);
		}
	}
}