using UnityEngine;

namespace HealthAndDamage{
	public class EnemyHealth : AbstractBaseHealth{

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