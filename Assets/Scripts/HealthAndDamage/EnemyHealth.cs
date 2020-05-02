using UnityEngine;

public class EnemyHealth : BaseHealth{
	
	protected new void Start(){
		OnDeath += EnemyDeath;
	}

	private void EnemyDeath(){
		Destroy(gameObject);
	}
}