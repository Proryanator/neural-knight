using System;
using UnityEngine;

/// <summary>
/// Responsible for managing what's considered 'levels' in the game.
///
/// For now, can be a singleton!
///
/// Will distribute out information about what changes if needed through events.
/// </summary>
public class LevelManager : MonoBehaviour{
	
	private static LevelManager _instance = null;

	// the current spawn rate; changed when levels start
	private float _currentSpawnRate = 0f;

	// the current spawn count; changed when levels start
	private int _currentSpawnCount = 0;
	
	[Tooltip("How many enemies to start spawning with. Will increase from here.")]
	[SerializeField] private int _initialEnemySpawnCount = 10;
	
	// the current game level, incremented through some game behavior 
	private int _gameLevel = 1;
	
	// notifies when a level ends
	public Action<int> OnLevelFinish;
	
	// notifies when a level starts
	public Action<int> OnLevelStart;

	private void Awake(){
		if (_instance == null){
			_instance = this;
		}else if (_instance != this){
			Destroy(gameObject);
		}
	}

	private void Start(){
		// for now, we just start the game
		// TODO: might add logic here to allow for starting the level when the player is ready (perhaps to allow
		// for upgrades or something?
		StartLevel();
	}

	public static LevelManager GetInstance(){
		return _instance;
	}
	
	/// <summary>
	/// Initiates the start of a new level.
	///
	/// Calls any other methods that need to happen here.
	/// </summary>
	public void StartLevel(){
		Debug.Log("Starting Level [" + _gameLevel + "]!");

		if (OnLevelStart != null){
			OnLevelStart(_gameLevel);
		}
	}

	/// <summary>
	/// Initiates the end of a level. This does any level cleanup,
	/// including preparing for the next level.
	/// </summary>
	public void EndLevel(){
		// first, let's increment the level counter to the next level
		IncrementLevel();
		
		// and for now, just starts the next level!
		StartLevel();
	}

	private void IncrementLevel(){
		_gameLevel++;
		
		// if there are any listeners, notify them that the level has changed!
		if (OnLevelFinish != null){
			OnLevelFinish(_gameLevel);
		}
	}
}