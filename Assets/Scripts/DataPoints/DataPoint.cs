using Player;
using UnityEngine;

namespace DataPoints{ 
	/// <summary>
	/// Represents a collect-able data point in-game.
	/// </summary>
	public class DataPoint : MonoBehaviour{
		[Tooltip("The value of this data point.")]
		[SerializeField] private int _collectionValue;

		private static int _dataPointsInScene = 0;
		
		public DataPoint(){
			// when this data point is spawned, increment the counter
			_dataPointsInScene++;
		}

		public static int GetDataPointCountInScene(){
			return _dataPointsInScene;
		}
		
		/// <summary>
		/// Adds the data point's points to the player points,
		/// and destroys the object.
		/// </summary>
		public void Collect(PlayerPoints playerPoints){
			playerPoints.AddPoints(_collectionValue);
			Destroy(gameObject);
		}
	}
}