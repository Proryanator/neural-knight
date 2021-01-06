﻿using UnityEngine;
using Utils;

namespace Systems.Rooms {
	public class RoomPlacer : MonoBehaviour {
		private static RoomPlacer _instance;
		
		[SerializeField] private GameObject _roomPrefab;
		[SerializeField] private Transform _parentWorldObject;

		private Transform _playerTransform;
		
		private float _roomLength = 19.2f;
		private int _roomHeight = 11;
		
		private void Awake(){
			if (_instance == null){
				_instance = this;
			}else if (_instance != this){
				Destroy(gameObject);
			}

			_playerTransform = GameObject.FindGameObjectWithTag(AllTags.PLAYER).transform;
		}

		public static RoomPlacer GetInstance(){
			return _instance;
		}
		
		public GameObject SpawnRoomInMovementDirection(){
			Vector2 directionOfRoomExit = GetPlayerDirection(_playerTransform);
			
			Vector2 roomLocation = new Vector2(directionOfRoomExit.x * _roomLength, directionOfRoomExit.y * _roomHeight);
			
			// normalize the direction given to one of 4 directions, north/south/east/west
			GameObject newRoom = Instantiate(_roomPrefab, roomLocation, Quaternion.identity);
			newRoom.transform.parent = _parentWorldObject;

			return newRoom;
		}

		private Vector2 GetPlayerDirection(Transform playerTransform){
			Vector2[] allDirections = new [] {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
			foreach (Vector2 possibleDirection in allDirections){
				if (Mathf.Abs(Vector2.Angle(playerTransform.position, possibleDirection)) < 45){
					return possibleDirection;
				}
			}
			
			// something went wrong here, shouldn't happen!
			return Vector2.zero;
		}
	}
}