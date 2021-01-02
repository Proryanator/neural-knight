using Systems.Rooms;
using POCO.StateMachines;

namespace Systems.Levels.LevelStates{
	public class RoomTransitionState : State {

		public RoomTransitionState(RoomPlacer roomPlacer){
			_roomPlacer = roomPlacer;
		}
		
		private RoomPlacer _roomPlacer;
		
		public override void Tick(){
			// TODO: tick along, calling the movement function for the room?
		}

		public override void OnEnter(){
			// TODO: where do we get this information from?
			_roomPlacer.SpawnRoomInMovementDirection();
		}

		public override void OnExit(){
			
		}
	}
}