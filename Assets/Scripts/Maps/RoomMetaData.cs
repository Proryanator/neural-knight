namespace Maps{
	// contains meta data about each room, what is to be spawned, what doors to close, etc.
	public class RoomMetaData {
		// all the closed doors, or at least, what are supposed to be closed
		private DoorDirection[] _closedDoors;

		public void InitDoors(DoorDirection[] closedDoors){
			_closedDoors = closedDoors;
		}

		public DoorDirection[] GetClosedDoors(){
			return _closedDoors;
		}
	}
}