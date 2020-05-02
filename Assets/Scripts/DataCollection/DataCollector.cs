using System;
using UnityEngine;

/**
 * Collects a data point object, and passes it to the respective location to store the points/etc.
 */
public class DataCollector : MonoBehaviour{
	
	public void OnCollisionEnter2D(Collision2D other){
		CollectDataPoint(other);
	}

	private void CollectDataPoint(Collision2D other){
		DataPointType dataPointType = other.gameObject.GetComponent<DataPointType>();

		// only interacting with data points here
		if (dataPointType == null){
			return;
		}

		// depending on the data type, we'll do something different
		switch (dataPointType){
			case DataPointType.GoodData:
				// TODO: implement what happens with good data
				break;
			case DataPointType.BadData:
				// TODO: implement what happens with bad data
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}