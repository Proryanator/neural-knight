using UnityEngine;
using Utils;

namespace HealthAndDamage{
	public class PlayerHealth : AbstractBaseHealth{

		protected new void Start(){
			base.Start();

			// we want the player to die this way!
			OnDeath += PlayerDeath;
		}

		private void PlayerDeath(){
			Debug.Log("Player Died!");
		
			// detach the AITracker object attached
			// TODO: this will need to be modified for more than 1 player
			GameObject aiTracker = GameObject.FindGameObjectWithTag(AllTags.AI_TRACKER);
			aiTracker.transform.parent = null;
		
			Destroy(gameObject);
		
			// TODO: a game manager would most likely hook themselves into the player's OnDeath mechanic
		}
	}
}