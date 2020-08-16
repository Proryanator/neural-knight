using UnityEngine;

namespace DataCollection{
	/**
 * Collects a data point object, and passes it to the respective location to store the points/etc.
 *
 * Intended to be placed on a player object!
 */
	[RequireComponent(typeof(PlayerPoints))]
	public class DataCollector : MonoBehaviour{

		// cached value of the player points
		private PlayerPoints _playerPoints;

		private void Awake(){
			_playerPoints = gameObject.GetComponent<PlayerPoints>();
		}

		public void OnCollisionEnter2D(Collision2D other){
			CollectDataPoint(other);
		}

		private void CollectDataPoint(Collision2D other){
			DataPoint dataPoint = other.gameObject.GetComponent<DataPoint>();

			// only interacting with data points here
			if (dataPoint == null){
				return;
			}

			dataPoint.Collect(_playerPoints);
		}
	}
}