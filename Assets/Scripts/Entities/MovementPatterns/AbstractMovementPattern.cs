using UnityEngine;

namespace Entities.MovementPatterns{
	public abstract class AbstractMovementPattern : MonoBehaviour{
		
		protected FacingDirection startingDirection = FacingDirection.UP;
		
		/// <summary>
		/// Defines the movement pattern for this object.
		/// </summary>
		public abstract void Move();
	}
}