using System;
using UnityEngine;

namespace Player{
	public class PlayerPoints : MonoBehaviour{

		// stores the player's points!
		[SerializeField] private int _points;

		/// <summary>
		/// Subscribe to this if you want to know when the score changes!
		/// </summary>
		public Action<int> OnScoreChange;

		/// <summary>
		/// Adds the given points to the current score.
		/// </summary>
		public void AddPoints(int points){
			_points += points;

			if (OnScoreChange != null){
				OnScoreChange(_points);
			}
		}
	}
}