using System;
using UnityEngine;

/// <summary>
/// Intended to hold scores of single (and eventually mutli-player) players.
///
/// Currently only holds single player's score, and acts as a singleton.
/// Don't access me everywhere!
/// </summary>
public class ScoreSystem : MonoBehaviour{
	
	[Tooltip("The players current score; incremented by killing enemies and collecting good data.")]
	[SerializeField] private int _score = 0;

	private static ScoreSystem _instance = null;
	
	private void Awake(){
		if (_instance == null){
			_instance = this;
		}else if (_instance != this){
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Subscribe to this if you want to know when the score changes!
	/// </summary>
	public Action<int> OnScoreChange;

	public static ScoreSystem GetInstance(){
		return _instance;
	}
	
	/// <summary>
	/// Adds the given points to the current score.
	/// </summary>
	public void AddPoints(int points){
		_score += points;

		if (OnScoreChange != null){
			OnScoreChange(_score);
		}
	}
}