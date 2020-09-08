using Player;
using UnityEngine;

namespace DataPoints{
	public class DataPoint : MonoBehaviour{
		[SerializeField] private int _collectionValue;

		private static int _totalDataPointsInScene = 0;

		private void Start(){
			_totalDataPointsInScene++;
		}

		public static int GetDataPointCountInScene(){
			return _totalDataPointsInScene;
		}
		
		public void Collect(PlayerPoints playerPoints){
			_totalDataPointsInScene--;
			playerPoints.AddPoints(_collectionValue);
			Destroy(gameObject);
		}
	}
}