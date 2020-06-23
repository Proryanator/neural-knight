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
	
	[Tooltip("The initial spawn rate that the game will start with. Increases from here.")]
	[SerializeField] private float _initialEnemySpawnRate = 2;
	
	[Tooltip("The lowest/quickest that the game will progress to, for spawning enemies. Will not get faster than this.")]
	[SerializeField] private float _minEnemySpawnRate = .2f;

	// the current spawn count; changed when levels start
	private int _currentSpawnCount = 0;
	
	[Tooltip("How many enemies to start spawning with. Will increase from here.")]
	[SerializeField] private int _initialEnemySpawnCount = 10;
	
	// the current game level, incremented through some game behavior 
	private int _gameLevel = 1;
	
	// notifies when the level number changes
	public Action<int> OnLevelChange;

	// subscribe to this to get information about when the level starts, including starting spawn rates, counts, etc.
	public Action<float, int> OnLevelStart;
	
	private void Awake(){
		if (_instance == null){
			_instance = this;
		}else if (_instance != this){
			Destroy(gameObject);
		}
	}

	private void Start(){
		_currentSpawnRate = _initialEnemySpawnRate;
		_currentSpawnCount = _initialEnemySpawnCount;
		
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

		// notify everything that needs to know about the current rates
		if (OnLevelStart != null){
			OnLevelStart(_currentSpawnRate, _currentSpawnCount);
		}
	}

	/// <summary>
	/// Initiates the end of a level. This does any level cleanup,
	/// including preparing for the next level.
	/// </summary>
	public void EndLevel(){
		// first, let's increment the level counter to the next level
		IncrementLevel();
		
		// TODO: change the values for the start of the next level!
	}

	private void IncrementLevel(){
		_gameLevel++;
		
		// if there are any listeners, notify them that the level has changed!
		if (OnLevelChange != null){
			OnLevelChange(_gameLevel);
		}

		
	}
}