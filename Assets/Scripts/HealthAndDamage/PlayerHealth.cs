using UnityEngine;

public class PlayerHealth : BaseHealth{

	protected new void Start(){
		base.Start();

		// we want the player to die this way!
		OnDeath += PlayerDeath;
	}

	private static void PlayerDeath(){
		Debug.Log("Player Died!");
	}
}