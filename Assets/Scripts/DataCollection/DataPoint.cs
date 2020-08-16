using Player;
using UnityEngine;

namespace DataCollection{ 
	/**
	* Holds information about this data point.
	*
	* Will hold things like how many points this is worth, what type of data point it is, etc.
	*/
	public class DataPoint : MonoBehaviour{
		[Tooltip("The point value that is acquired/lost when this object is 'collected'.")] [SerializeField]
		private int _collectionValue;

		/// <summary>
		/// Called when this object is collected.
		///
		/// Handles the differences (if any) when collected.
		/// </summary>
		public void Collect(PlayerPoints playerPoints){
			playerPoints.AddPoints(_collectionValue);
		}
	}
}