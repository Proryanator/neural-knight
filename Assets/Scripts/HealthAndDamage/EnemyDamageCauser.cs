using Systems;
using UnityEngine;

public class EnemyDamageCauser : MonoBehaviour{

	public void OnCollisionEnter2D(Collision2D other){
		// gather it's health object and damage it if we're supposed to
		BaseHealth health = other.gameObject.GetComponent<BaseHealth>();
		
		if (health != null && CauseDamage(other)){
			health.Damage();
		}
	}
	
	private bool CauseDamage(Collision2D other){
		return other.gameObject.tag.Equals(AllTags.PLAYER);
	}
}