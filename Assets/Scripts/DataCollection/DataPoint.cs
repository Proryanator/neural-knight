using UnityEngine;

/**
 * Holds information about this data point.
 *
 * Will hold things like how many points this is worth, what type of data point it is, etc.
 */
public class DataPoint : MonoBehaviour{
	/// <summary>
	/// The type of data point this object is. Helps tell the player objects what happens when they're collided with.
	/// </summary>
	[SerializeField] private DataPointType _dataPointType;
	
	
}