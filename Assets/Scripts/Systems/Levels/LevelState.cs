namespace Systems.Levels{
	
	/// <summary>
	/// Holds information about what state the level is in.
	///
	/// Useful for controlling states that occur during gameplay and
	/// allowing for transitions.
	/// </summary>
	public enum LevelState{
		// the initial state that the level begins with
		WaitingToStart,
		// the default state of a new level
		Started,
		// all data has been collected, and we need to kill all remaining data
		EnemyCleanup,
		// the state waiting for the player to upgrade their gear, or to go to the next map
		WaitForPlayerAction,
		// level has ended, may not need this?
		Ended
	}
}