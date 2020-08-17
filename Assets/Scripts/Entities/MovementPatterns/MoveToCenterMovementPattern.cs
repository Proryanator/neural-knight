using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	/// <summary>
	/// The movement pattern that all entities will default to should they spawn outside of the play area.
	/// </summary>
	public class MoveToCenterMovementPattern : AbstractMovementPattern{

		// holds the transform of the central location of the map
		private GameObject _mapCentralPoint;

		[Tooltip("The speed at which to move towards the center of the map.")] [SerializeField]
		private float _moveSpeed = 5f;

		private void Awake(){
			_mapCentralPoint = GameObject.FindGameObjectWithTag(AllTags.MAP_CENTRAL_POINT);

			if (_mapCentralPoint == null){
				Debug.LogWarning("There was no object set with the tag: " + AllTags.MAP_CENTRAL_POINT +
				                 " and movement towards center will not work.");
			}
		}

		public override void Move(){
			if (_mapCentralPoint != null){
				transform.position = Vector2.MoveTowards(transform.position,
					(Vector2) _mapCentralPoint.transform.position, _moveSpeed * Time.deltaTime);
			}
		}
	}
}