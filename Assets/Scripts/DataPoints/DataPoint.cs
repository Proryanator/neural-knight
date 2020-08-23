using Player;
using UnityEngine;

namespace DataPoints{ 
	/// <summary>
	/// Represents a collect-able data point in-game.
	/// </summary>
	public class DataPoint : MonoBehaviour{
		[Tooltip("The value of this data point.")]
		[SerializeField] private int _collectionValue;

		private static int _totalDataPointsInScene = 0;

		private void Start(){
			// when this data point is spawned, increment the counter
			_totalDataPointsInScene++;
		}

		public static int GetDataPointCountInScene(){
			return _totalDataPointsInScene;
		}
		
		/// <summary>
		/// Adds the data point's points to the player points,
		/// and destroys the object.
		/// </summary>
		public void Collect(PlayerPoints playerPoints){
			// decrement how many are in the scene
			_totalDataPointsInScene--;
			
			playerPoints.AddPoints(_collectionValue);
			Destroy(gameObject);
		}
	}
}