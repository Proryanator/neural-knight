using Entities.Movement;
using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	
	public class CircleAroundPointPattern : AbstractMovementPattern{
		
		[Tooltip("How far from the point to rotate outwards from. NOTE: this is used to create a slightly larger radius for movement purposes.")]
		[SerializeField] private float _radius = 1f;

		[Tooltip("Speed to move towards the center of the map")]
		[SerializeField] private float _moveSpeed = 5f;

		[SerializeField] private bool _useCenterPoint;
		
		// point to rotate around
		private Transform _rotationPoint;

		private void Awake(){
			if (_useCenterPoint){
				_rotationPoint = GetCenterOfMap();
			}
			else{
				_rotationPoint = GetRandomFocusPoint();
			}
		}

		private Transform GetRandomFocusPoint(){
			// get all focus points in the scene
			GameObject[] focusPoints = GameObject.FindGameObjectsWithTag(AllTags.DATA_FOCUS_POINT);
			
			// randomly choose an available rotation spot
			return focusPoints[Random.Range(0, focusPoints.Length)].transform;
		}

		private Transform GetCenterOfMap(){
			return FindObjectOfType<PlayArea>().transform;
		}

		public override void Move(){
			CircleAroundCenterPointPattern(transform, _rotationPoint, _radius, _moveSpeed);
		}

		public static void CircleAroundCenterPointPattern(Transform transform, Transform rotationPoint, float radius, float moveSpeed){
			// use the circle collider if there is one, or add one if not
			CircleCollider2D circleCollider2D = AddCircleColliderIfNone(rotationPoint, radius);

			Vector2 directionOfMovement;
			
			if (IsInsideOfCircle(transform, circleCollider2D)){
				directionOfMovement = MoveInCircle(transform, rotationPoint, moveSpeed);
			}
			else{
				directionOfMovement = MoveToCircle(transform, rotationPoint, moveSpeed);
			}

			// no matter what, you'll want to face your movement direction
			transform.rotation = Utils2D.GetRotationTowardsDirection(directionOfMovement, FacingDirection.UP);
		}

		private static Vector2 MoveToCircle(Transform transform, Transform rotationPoint, float moveSpeed){
			// start moving towards the center, like you do with the MoveToCenterPattern
			MoveToCenterMovementPattern.MoveToCenterMovement(transform, rotationPoint, moveSpeed);
				
			// move towards the center until you're inside of the circle
			return (Vector2) rotationPoint.position - (Vector2) transform.position;
		}

		private static Vector2 MoveInCircle(Transform transform, Transform rotationPoint, float moveSpeed){
			// remember transform from before
			Vector2 oldPosition = transform.position;
			
			transform.RotateAround(rotationPoint.position, Vector3.back, Time.deltaTime * (moveSpeed * 20));
			
			// now, let's calculate the direction from one point to another
			return (Vector2) transform.position - oldPosition;
		}
		
		private static CircleCollider2D AddCircleColliderIfNone(Transform rotationPoint, float radius){
			CircleCollider2D circleCollider2D = rotationPoint.GetComponent<CircleCollider2D>();
			if (circleCollider2D == null){
				circleCollider2D = rotationPoint.gameObject.AddComponent<CircleCollider2D>();
			}
			
			// make sure this is a trigger, otherwise it'll collide :o
			circleCollider2D.isTrigger = true;
			circleCollider2D.radius = radius;

			return circleCollider2D;
		}
		
		private static bool IsInsideOfCircle(Transform transform, Collider2D circleCollider2D){
			return circleCollider2D.bounds.Contains(transform.position);
		}
	}
}