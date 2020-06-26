using UnityEngine;

public class EnemyDamageCauser : MonoBehaviour{

	[Tooltip("How much damage an enemy will cause to you upon colliding with you.")]
	[SerializeField] private int _enemyDamage = 1;
	
	public void OnCollisionEnter2D(Collision2D other){
		// gather it's health object and damage it if we're supposed to
		AbstractBaseHealth health = other.gameObject.GetComponent<AbstractBaseHealth>();
		
		if (health != null && CauseDamage(other)){
			health.Damage(_enemyDamage);
		}
	}
	
	private bool CauseDamage(Collision2D other){
		return other.gameObject.tag.Equals(AllTags.PLAYER);
	}
}