using Systems.Rooms;
using POCO.StateMachines;
using UnityEngine;
using Utils;

namespace Systems.Levels.LevelStates{
	public class RoomTransitionState : State {

		private float _slideSpeed = 2f;

		private GameObject _newMap;
		private GameObject _oldMap;

		// TODO: we could create a state machine for the player to handle this, instead of doing it here
		private GameObject _player;
		
		private float _distanceBetweenMaps;
		private float _distanceBetweenPlayerAndMap;
		
		public RoomTransitionState(RoomPlacer roomPlacer){
			_roomPlacer = roomPlacer;
		}
		
		private readonly RoomPlacer _roomPlacer;
		
		public override void Tick(){
			// if the new map is 'close enough' to the 0 position, we'll go ahead and just snap it there
			if (Vector2.Distance(_newMap.transform.position, Vector2.zero) < 2){
				_newMap.transform.position = Vector2.zero;
			}
			else{
				// new map slides towards the origin
				SlideTo(_newMap, Vector2.zero);
			}

			// old map just keeps it's distance from the new map
			_oldMap.transform.position = KeepAtDistance(_distanceBetweenMaps, _newMap, _oldMap);
			_player.transform.position = KeepAtDistance(_distanceBetweenPlayerAndMap, _oldMap, _player);
		}

		public override void OnEnter(){
			_oldMap = GameObject.FindGameObjectWithTag(AllTags.ROOM);
			_newMap = _roomPlacer.SpawnRoomInMovementDirection();
			
			// now let's move the player in the map SO
			_roomPlacer.MovePlayerInMapSO();

			_player = GameObject.FindWithTag(AllTags.PLAYER);
			_player.SetActive(false);

			_distanceBetweenMaps = Vector2.Distance(_oldMap.transform.position, _newMap.transform.position);
			_distanceBetweenPlayerAndMap = Vector2.Distance(_oldMap.transform.position, _player.transform.position);
		}

		public override void OnExit(){
			GameObject.Destroy(_oldMap);
			
			_player.SetActive(true);
		}

		private void SlideTo(GameObject obj, Vector2 finalDestination){
			float speed = Time.deltaTime * _slideSpeed;
			obj.transform.position = Vector2.Lerp(obj.transform.position, finalDestination, speed);
		}
		
		public bool HasTransitionFinished(){
			return _newMap.transform.position.Equals(Vector2.zero);
		}

		private Vector2 KeepAtDistance(float distance, GameObject stayNear, GameObject actual){
			return (actual.transform.position - stayNear.transform.position).normalized * distance + stayNear.transform.position;
		}
		
	}
}