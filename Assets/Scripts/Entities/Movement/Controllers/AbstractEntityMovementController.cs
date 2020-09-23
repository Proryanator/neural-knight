using System;
using Entities.Movement.EntityStates;
using Entities.MovementPatterns;
using POCO.StateMachines;
using UnityEngine;

namespace Entities.Movement.Controllers{
	[RequireComponent(typeof(MoveToCenterMovementPattern))]
	[RequireComponent(typeof(EntityPlayAreaLayerChanger))]
	public abstract class AbstractEntityMovementController : MonoBehaviour{
		
		private MoveToCenterMovementPattern _moveToCenterMovementPattern;

		protected StateMachine stateMachine;
		protected MoveToPlayAreaState moveToPlayAreaState;

		private bool _inPlayArea;
		
		protected void Awake(){
			_moveToCenterMovementPattern = GetComponent<MoveToCenterMovementPattern>();

			CreateStateMachineAndInitialState();
		}

		protected void Update(){
			// tick is called on the state machine, does it do anything?
			stateMachine.Tick();
		}

		private void CreateStateMachineAndInitialState(){
			stateMachine = new StateMachine();
			
			// every entity will have this state, to move to the center!
			moveToPlayAreaState = new MoveToPlayAreaState(_moveToCenterMovementPattern);

			// all entities start in the same state!
			stateMachine.SetState(moveToPlayAreaState);
		}

		protected Func<bool> IsInPlayArea() => () => _inPlayArea;
		
		protected AbstractMovementPattern GetNormalMovementPattern(){
			AbstractMovementPattern nonMoveToCenterPattern = null;
			
			foreach (AbstractMovementPattern pattern in GetComponents<AbstractMovementPattern>()){
				if (pattern is MoveToCenterMovementPattern || pattern is FollowPlayerPattern){
				}
				else{
					nonMoveToCenterPattern = pattern;
					break;
				}
			}

			return nonMoveToCenterPattern;
		}

		public void SetInsidePlayArea(){
			_inPlayArea = true;
		}
	}
}