﻿using UnityEngine;

namespace Systems.Levels{
	/// <summary>
	/// Attach me to any objects that may need to trigger a
	/// 'is the level over' type check.
	/// </summary>
	public class LevelStateNotifier : MonoBehaviour{

		private void OnDestroy(){
			// after destroying itself, lets the level manager know the level might be over
			LevelManager.GetInstance().LevelStateChangeCheck();
		}
	}
}