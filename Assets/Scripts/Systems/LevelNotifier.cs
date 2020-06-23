using System;
using UnityEngine;

/// <summary>
/// Makes a level ended call to the level manager.
///
/// This is expensive, probably want to change this in the future.
/// </summary>
public class LevelNotifier : MonoBehaviour{
	private void OnDestroy(){
		LevelManager.GetInstance().LevelEndCheck();
	}
}