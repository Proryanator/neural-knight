using System;
using UnityEngine;

/// <summary>
/// Intended to hold scores of single (and eventually mutli-player) players.
///
/// Currently only holds single player's score.
/// </summary>
public class ScoreSystem : MonoBehaviour{
	
	[Tooltip("The players current score; incremented by killing enemies and collecting good data.")]
	[SerializeField] private int _score;
	
	private void Awake(){
		_score = 0;
	}

	/// <summary>
	/// Subscribe to this if you want to know when the score changes!
	/// </summary>
	public Action<int> OnScoreChange;
	
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