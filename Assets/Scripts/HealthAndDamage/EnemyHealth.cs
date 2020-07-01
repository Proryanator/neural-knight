using UnityEngine;

public class EnemyHealth : AbstractBaseHealth{

	// TODO: not the best place to store this point value, can probably
	// figure out where to store this later!
	/*[Tooltip("The points gained by the player upon killing this enemy.")]
	[SerializeField] private int _pointValue = 1;*/
	
	protected new void Start(){
		base.Start();
		OnDeath += EnemyDeath;
	}

	// TODO: not sure how to handle this if we're supposed to pass in the player's id :/
	private void EnemyDeath(){
		// ScoreSystem.GetInstance().AddPoints(_pointValue);
		Destroy(gameObject);
	}
}