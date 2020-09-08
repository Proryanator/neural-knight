using System.Collections;
using HealthAndDamage;
using UnityEngine;
using Utils;

namespace Entities.Movement{
	[RequireComponent(typeof(EntityMovementController))]
	public class StunWhenHit : MonoBehaviour{
		[Tooltip("Seconds to stop movement of the current entity")]
		[SerializeField] private float _stunTime = 1f;
		
		private Rigidbody2D _rigidBody2D;

		private bool _isStunned;

		private EntityMovementController _entityMovementController;

		private int _originalLayer;
		
		private void Awake(){
			_entityMovementController = GetComponent<EntityMovementController>();
			_originalLayer = gameObject.layer;
			
			_rigidBody2D = GetComponent<Rigidbody2D>();
			if (_rigidBody2D == null){
				Debug.LogWarning("Not able to stop any forces applied to the object.");
			}
			
			AbstractBaseHealth health = GetComponent<AbstractBaseHealth>();
			if (health != null){
				health.OnDamageTaken += StunEntityIfNotAlreadyStunned;
			}
			else{
				Debug.LogWarning("This entity should have a health object for movement to be stopped properly.");
			}
		}
		
		private void StunEntityIfNotAlreadyStunned(int damage){
			if (!_isStunned){
				StartCoroutine(StunEntityForSetTime(_stunTime));
			}
		}

		private IEnumerator StunEntityForSetTime(float stunTime){
			SetStunnedState();
			yield return new WaitForSeconds(stunTime);
			UndoStunnedState();
		}

		private void SetStunnedState(){
			_isStunned = true;
			gameObject.layer = LayerMask.NameToLayer(AllLayers.DAMAGED_ENEMY);
			_entityMovementController.DisableMovementPattern();
		}

		private void UndoStunnedState(){
			_isStunned = false;
			_entityMovementController.StartInitialMovementPattern();
			gameObject.layer = _originalLayer;

			// remove any forces if any were applied
			_rigidBody2D.velocity = Vector2.zero;
		}
	}
}