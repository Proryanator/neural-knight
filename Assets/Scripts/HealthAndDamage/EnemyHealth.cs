using UnityEngine;

namespace HealthAndDamage{
	public class EnemyHealth : AbstractBaseHealth{

		// TODO: not the best place to store this point value, can probably
		// figure out where to store this later!
		/*[Tooltip("The points gained by the player upon killing this enemy.")]
	[SerializeField] private int _pointValue = 1;*/

		[Tooltip("If true, destroys the parent object, not this object.")]
		[SerializeField] private bool _parentDeath = false;
	
		protected new void Start(){
			base.Start();
			OnDeath += EnemyDeath;
		}

		// TODO: not sure how to handle this if we're supposed to pass in the player's id :/
		private void EnemyDeath(){
			if (_parentDeath){
				Destroy(transform.parent.gameObject);
			}
			else{
				Destroy(gameObject);
			}
		}
	}
}