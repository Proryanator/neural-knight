using UnityEngine;

namespace Systems.Levels{
	public class EnemiesInSceneCounter : MonoBehaviour{
		private static int _totalEnemiesInScene = 0;

		private void Awake(){
			_totalEnemiesInScene++;
		}
		
		private void OnDestroy(){
			_totalEnemiesInScene--;
		}

		public static int GetTotalEnemiesInScene(){
			return _totalEnemiesInScene;
		}
	}
}