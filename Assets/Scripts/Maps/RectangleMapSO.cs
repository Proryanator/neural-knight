using System.Collections.Generic;
using UnityEngine;

namespace Maps {
	
	// a crude simple map in matrix form
	[CreateAssetMenu(fileName = "FullMap", menuName = "NeuralDefender: FullMap", order = 0)]
	public class RectangleMapSO : ScriptableObject{
		[SerializeField] private int _width = 1;
		[SerializeField] private int _height = 1;

		// track the player's current whereabouts in the map
		[SerializeField] private int _currentX;
		[SerializeField] private int _currentY;

		private RoomMetaData[][] _roomMetaDataList;

		public void Init(){
			BuildMapMatrix();
			SetClosedDoors();
			SetupStartPosition();
		}
		
		public void MoveToNextRoom(Vector2 moveDirection){
			if (moveDirection.Equals(Vector2.down)){
				_currentY++;
			}else if (moveDirection.Equals(Vector2.up)){
				_currentY--;
			}else if (moveDirection.Equals(Vector2.right)){
				_currentX++;
			}else if (moveDirection.Equals(Vector2.left)){
				_currentX--;
			}
		}

		private RoomMetaData GetCurrentRoomMetaData(){
			return _roomMetaDataList[_currentX][_currentY];
		}

		public void EnableDoorsInCorrectPlaces(GameObject newRoom){
			MapDoorGroup mapDoorGroup = newRoom.GetComponentInChildren<MapDoorGroup>();
			
			// based on the player's current location, enable some doors
			RoomMetaData data = GetCurrentRoomMetaData();
			foreach (DoorDirection doorDirection in data.GetClosedDoors()){
				mapDoorGroup.GetMapDoorIn(doorDirection).EnableDoor();
			}
		}
		
		private void BuildMapMatrix(){
			_roomMetaDataList = new RoomMetaData[_height][];

			for (int i = 0; i < _height; i++){
				_roomMetaDataList[i] = new RoomMetaData[_width];
			}
			
			// now, create an entry in each
			for (int i = 0; i < _height; i++){
				for (int k = 0; k < _width; k++){
					_roomMetaDataList[i][k] = new RoomMetaData();
				}
			}
		}

		private void SetupStartPosition(){
			_currentX = 0;
			_currentY = 0;
		}

		private void SetClosedDoors(){
			for (int i = 0; i < _height; i++){
				for (int k = 0; k < _width; k++){
					RoomMetaData roomMetaData = _roomMetaDataList[i][k];
					roomMetaData.InitDoors(GetClosedDoorsFor(i, k));
				}
			}
		}
		
		private DoorDirection[] GetClosedDoorsFor(int x, int y){
			List<DoorDirection> doorDirectionsIfAny = new List<DoorDirection>();

			if (x == 0){
				doorDirectionsIfAny.Add(DoorDirection.Left);
			}
			if (y == 0){
				doorDirectionsIfAny.Add(DoorDirection.Up);
			}
			if (x == _width-1){
				doorDirectionsIfAny.Add(DoorDirection.Right);
			}
			if (y == _height - 1){
				doorDirectionsIfAny.Add(DoorDirection.Down);
			}

			return doorDirectionsIfAny.ToArray();
		}
	}
}