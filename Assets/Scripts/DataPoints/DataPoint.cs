using Player;
using UnityEngine;

namespace DataPoints{ 
	/// <summary>
	/// Represents a collect-able data point in-game.
	/// </summary>
	public class DataPoint : MonoBehaviour{
		[Tooltip("The value of this data point.")]
		[SerializeField] private int _collectionValue;

		/// <summary>
		/// Called when this object is collected.
		/// </summary>
		public void Collect(PlayerPoints playerPoints){
			playerPoints.AddPoints(_collectionValue);
		}
	}
}