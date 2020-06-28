using System;
using UnityEngine;

/**
 * Holds information about this data point.
 *
 * Will hold things like how many points this is worth, what type of data point it is, etc.
 */
public class DataPoint : MonoBehaviour{
	
	[Tooltip("The type of datapoint this is. May be useful in determining other things about the data collection.")]
	[SerializeField] private DataPointType _dataPointType;

	[Tooltip("The point value that is acquired/lost when this object is 'collected'.")] 
	[SerializeField] private int _collectionValue;

	/// <summary>
	/// Called when this object is collected.
	///
	/// Handles the differences (if any) when collected.
	/// </summary>
	public void Collect(PlayerPoints playerPoints){
		switch (_dataPointType){
			case DataPointType.GoodData:
				playerPoints.AddPoints(_collectionValue);
				Destroy(gameObject);
				break;
			case DataPointType.BadData:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}