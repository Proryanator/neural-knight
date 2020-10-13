using Entities.Movement.Controllers;
using POCO.StateMachines;
using UnityEngine;
using Utils;

namespace Entities.Movement.EntityStates{
	public class DamagedEntityState : State{

		private readonly EnemyMovementController _enemyMovementController;
		private readonly int _originalLayer;
		private readonly Rigidbody2D _rigidbody2D;

		public DamagedEntityState(EnemyMovementController controller){
			_enemyMovementController = controller;

			_originalLayer = _enemyMovementController.gameObject.layer;
			_rigidbody2D = _enemyMovementController.GetComponent<Rigidbody2D>();
		}
		
		public override void Tick(){
		}

		public override void OnEnter(){
			_enemyMovementController.gameObject.layer = LayerMask.NameToLayer(AllLayers.DAMAGED_ENEMY);
		}

		public override void OnExit(){
			_enemyMovementController.gameObject.layer = _originalLayer;
			
			// if this game object has any rigid bodies on it, let's zero out any forces
			if (_rigidbody2D != null){
				_rigidbody2D.velocity = Vector2.zero;
			}
		}
	}
}