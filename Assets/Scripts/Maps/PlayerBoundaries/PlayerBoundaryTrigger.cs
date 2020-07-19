using System;
using UnityEngine;

/// <summary>
/// Trigger for whether the player enters in the space to wall off the entrance for players.
///
/// When the player enters the trigger on this object, then we'll enable the boundary.
///
/// When the player leaves, we'll disable the boundary.
/// </summary>
public class PlayerBoundaryTrigger : MonoBehaviour{

	// holds reference of player boundary located as child
	private PlayerBoundary _playerBoundary;

	private void Awake(){
		_playerBoundary = GetComponentInChildren<PlayerBoundary>();
	}

	private void OnTriggerEnter2D(Collider2D other){
		TriggerWallIfPlayer(other, true);
	}

	private void OnTriggerExit2D(Collider2D other){
		TriggerWallIfPlayer(other, false);
	}

	private void TriggerWallIfPlayer(Collider2D other, bool enableCollider){
		if (other.gameObject.tag.Equals(AllTags.PLAYER)){
			if (enableCollider){
				_playerBoundary.EnableCollider();
			}
			else{
				_playerBoundary.DisableCollider();
			}
		}
	}
}