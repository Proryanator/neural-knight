using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	
	/// <summary>
	/// Moves the object in a circle around the center of the map.
	/// </summary>
	public class CircleAroundCenterPattern : AbstractMovementPattern{
		
		[Tooltip("How far from the point to rotate outwards from. NOTE: this is used to create a slightly larger radius for movement purposes.")]
		[SerializeField] private float _radius = 1f;
		
		[Tooltip("How fast to rotate around the center")]
		[SerializeField] private float _rotationSpeed = 5f;

		[Tooltip("Speed to move towards the center of the map")]
		[SerializeField] private float _toCenterSpeed = 5f;
		
		// point to rotate around
		private Transform _rotationOrigin;

		private CircleCollider2D _circleCollider2DOfOrigin;
		
		private void Awake(){
			// for now, this is just the center of the map
			_rotationOrigin = GameObject.FindGameObjectWithTag(AllTags.MAP_CENTRAL_POINT).transform;
			
			// set the radius of the collider of the central point to a radius slightly larger than this!
			_circleCollider2DOfOrigin = _rotationOrigin.GetComponent<CircleCollider2D>();
			_circleCollider2DOfOrigin.radius = _radius;
		}

		private void OnDrawGizmos(){
			// draw this circle as a gizmo so we can see how large this circle is
			Gizmos.DrawWireSphere(_rotationOrigin.position, _circleCollider2DOfOrigin.radius);
		}

		public override void Move(){
			Vector2 directionOfMovement;
			
			if (IsInsideOfCircle()){
				// remember transform from before
				Vector2 oldPosition = transform.position;
			
				transform.RotateAround(_rotationOrigin.position, Vector3.back, Time.deltaTime* _rotationSpeed);
			
				// now, let's calculate the direction from one point to another
				directionOfMovement = (Vector2) transform.position - oldPosition;
			}
			else{
				// start moving towards the center, like you do with the MoveToCenterPattern
				MoveToCenterMovementPattern.MoveToCenterMovement(transform, _rotationOrigin, _toCenterSpeed);
				
				// move towards the center until you're inside of the circle
				directionOfMovement = (Vector2) _rotationOrigin.position - (Vector2) transform.position;
			}

			// no matter what, you'll want to face your movement direction
			Utils2D.RotateToFaceDirection(transform, directionOfMovement, startingDirection);
		}

		/// <summary>
		/// Determine if the enemy is inside of the circle to begin the movement!
		/// </summary>
		private bool IsInsideOfCircle(){
			return _circleCollider2DOfOrigin.bounds.Contains(transform.position);
		}
	}
}