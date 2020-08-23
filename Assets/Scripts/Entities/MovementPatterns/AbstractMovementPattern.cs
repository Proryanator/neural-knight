using UnityEngine;

namespace Entities.MovementPatterns{
	/// <summary>
	/// A class that holds information that you'd need about how an object
	/// moves around in 2D space.
	/// </summary>
	public abstract class AbstractMovementPattern : MonoBehaviour{

		[Tooltip("The starting facing direction of the sprite")] 
		[SerializeField] protected FacingDirection startingDirection = FacingDirection.UP;
		
		/// <summary>
		/// Defines the movement pattern for this object.
		/// </summary>
		public abstract void Move();
	}
}