using UnityEngine;

namespace Maps {
	public class MapDoorGroup : MonoBehaviour {
		private MapDoor[] _mapDoors;

		private void Awake(){
			_mapDoors = GetComponentsInChildren<MapDoor>();
		}

		public MapDoor GetMapDoorIn(DoorDirection direction){
			foreach (MapDoor door in _mapDoors){
				if (door.GetDoorDirection().Equals(direction)){
					return door;
				}
			}

			return null;
		}
	}
}