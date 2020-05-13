using UnityEngine;

/// <summary>
/// A class that holds information that you'd need about how an object
/// moves around in 2D space.
/// </summary>
public abstract class AbstractAIMovementPattern : MonoBehaviour{
	
	/// <summary>
	/// Defines the movement pattern for this object.
	/// </summary>
	public abstract void Move();
}