using System;
using UnityEngine;

/**
 * Collects a data point object, and passes it to the respective location to store the points/etc.
 *
 * Does not handle anything to do with the object colliding (or should it?)
 */
public class DataCollector : MonoBehaviour{
	
	public void OnCollisionEnter2D(Collision2D other){
		CollectDataPoint(other);
	}

	private void CollectDataPoint(Collision2D other){
		DataPoint dataPoint = other.gameObject.GetComponent<DataPoint>();

		// only interacting with data points here
		if (dataPoint == null){
			return;
		}

		// if there is a datapoint, collect it!
		dataPoint.Collect();
	}
}