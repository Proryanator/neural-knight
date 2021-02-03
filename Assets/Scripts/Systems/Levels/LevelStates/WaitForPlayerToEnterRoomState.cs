using Systems.Rooms;
using Systems.Spawning;
using Entities.Movement;
using Maps.PlayerBoundaries;
using Player;
using POCO.StateMachines;
using UnityEngine;
using Utils;

namespace Systems.Levels.LevelStates{
	public class WaitForPlayerToEnterRoomState : State{

		private PlayArea _playArea;
		private GameObject _player;
		private PlayerControllerSwapper _playerControllerSwapper;

		private PlayerBoundaryTrigger[] _allPlayerWallTriggers;
		
		public WaitForPlayerToEnterRoomState(){
			
		}
		
		public override void Tick(){
			
		}

		public override void OnEnter(){
			_playArea = GameObject.FindObjectOfType<PlayArea>();
			_player = GameObject.FindGameObjectWithTag(AllTags.PLAYER);
			_playerControllerSwapper = _player.GetComponent<PlayerControllerSwapper>();
			
			_allPlayerWallTriggers = GameObject.FindObjectsOfType<PlayerBoundaryTrigger>();
			SetTriggers(false);

			// we set the direction of the player to walk towards the center
			_playerControllerSwapper.EnableAutoWalk((Vector2.zero - (Vector2)_playerControllerSwapper.gameObject.transform.position).normalized);
		}

		public override void OnExit(){
			SetTriggers(true);
			_playerControllerSwapper.EnablePlayerControl();
			
			// do a re-lookup of all spawn points in the new map
			SpawnManager[] managers = GameObject.FindObjectsOfType<SpawnManager>();
			foreach (SpawnManager manager in managers){
				manager.UseSpawnPointsIn(GameObject.FindGameObjectWithTag(AllTags.ROOM));
			}
			
			// close all doors behind the player (and after a new map spawn)
			RoomPlacer.GetInstance().EnableDoorsInCorrectPlaces(GameObject.FindGameObjectWithTag(AllTags.ROOM));
		}

		private void SetTriggers(bool enable){
			foreach (PlayerBoundaryTrigger trigger in _allPlayerWallTriggers){
				if (trigger == null){
					continue;
				}
				trigger.gameObject.SetActive(enable);
			}
		}

		public bool HasPlayerEnteredPlayArea(){
			return _playArea.IsInPlayArea(_player);
		}
	}
}