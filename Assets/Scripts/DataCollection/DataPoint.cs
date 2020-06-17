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

	public DataPointType GetDataPointType(){
		return _dataPointType;
	}

	public int GetCollectionValue(){
		return _collectionValue;
	}
}