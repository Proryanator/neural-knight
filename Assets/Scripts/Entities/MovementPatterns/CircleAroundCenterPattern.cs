using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	
	/// <summary>
	/// Moves the object in a circle around the center of the map.
	/// </summary>
	public class CircleAroundCenterPattern : AbstractMovementPattern{
		
		[Tooltip("How far from the point to rotate outwards from")]
		[SerializeField] private float _radius = 1f;
		
		[Tooltip("How fast to rotate around the center")]
		[SerializeField] private float _speed = 5f;

		// point to rotate around
		private Transform _rotationOrigin;
		
		private void Awake(){
			// for now, this is just the center of the map
			_rotationOrigin = GameObject.FindGameObjectWithTag(AllTags.MAP_CENTRAL_POINT).transform;
		}

		public override void Move(){
			// remember transform from before
			Vector2 oldPosition = transform.position;
			
			transform.RotateAround(_rotationOrigin.position, Vector3.back, Time.deltaTime* _speed);
			
			// now, let's calculate the direction from one point to another
			Vector2 directionOfMovement = (Vector2) transform.position - oldPosition;
			
			// also, face you in the right direction
			Utils2D.RotateToFaceDirection(transform, directionOfMovement, startingDirection);
		}
	}
}