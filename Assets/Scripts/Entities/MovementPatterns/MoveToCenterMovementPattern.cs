using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	/// <summary>
	/// The movement pattern that all entities will default to should they spawn outside of the play area.
	/// </summary>
	public class MoveToCenterMovementPattern : AbstractMovementPattern{

		// holds the transform of the central location of the map
		private Transform _mapCentralPoint;

		[Tooltip("The speed at which to move towards the center of the map.")] 
		[SerializeField] private float _moveSpeed = 5f;

		private void Awake(){
			_mapCentralPoint = GameObject.FindGameObjectWithTag(AllTags.MAP_CENTRAL_POINT).transform;

			if (_mapCentralPoint == null){
				Debug.LogWarning("There was no object set with the tag: " + AllTags.MAP_CENTRAL_POINT +
				                 " and movement towards center will not work.");
			}
		}

		public override void Move(){
			if (_mapCentralPoint != null){
				MoveToCenterMovement(transform, _mapCentralPoint, _moveSpeed);
			}
		}

		/// <summary>
		/// Contains the logic to move towards the center!
		/// </summary>
		public static void MoveToCenterMovement(Transform transform, Transform centralPoint, float moveSpeed){
			transform.position = Vector2.MoveTowards(transform.position,
				centralPoint.transform.position, moveSpeed * Time.deltaTime);
		}
	}
}