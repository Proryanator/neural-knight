using Systems;
using UnityEngine;

/**
 * Handles damaging the player.
 */
public class PlayerDamageHandler : BaseCollisionDamageHandler {
	
	protected override bool TakeDamage(Collision2D other){
		return other.gameObject.tag.Equals(AllTags.ENEMY);
	}
}