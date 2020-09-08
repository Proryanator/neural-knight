﻿using Entities.MovementPatterns;
using UnityEngine;

namespace Entities.Movement{
	[RequireComponent(typeof(MoveToCenterMovementPattern))]
	[RequireComponent(typeof(EntityPlayAreaLayerChanger))]
	public class EntityMovementController : MonoBehaviour{

		private AbstractMovementPattern _initialMovementPattern;
		private AbstractMovementPattern _movementPatternInUse;
		
		private MoveToCenterMovementPattern _moveToCenterMovementPattern;

		private void Awake(){
			_moveToCenterMovementPattern = GetComponent<MoveToCenterMovementPattern>();
			_initialMovementPattern = GetNonMoveToCenterPattern();
			_movementPatternInUse = _moveToCenterMovementPattern;

			Debug.LogWarning("You did not attach any sub-class of 'AbstractMovementPattern' to this controller, therefore it will have no movement.");
		}

		private void Update(){
			CallMovementPatternIfNotNull(_movementPatternInUse);
		}

		private AbstractMovementPattern GetNonMoveToCenterPattern(){
			AbstractMovementPattern nonMoveToCenterPattern = null;
			
			foreach (AbstractMovementPattern pattern in GetComponents<AbstractMovementPattern>()){
				if (pattern is MoveToCenterMovementPattern){
				}
				else{
					nonMoveToCenterPattern = pattern;
					break;
				}
			}

			return nonMoveToCenterPattern;
		}
		
		private void CallMovementPatternIfNotNull(AbstractMovementPattern pattern){
			pattern?.Move();
		}

		public void DisableMovementPattern(){
			_movementPatternInUse = null;
		}

		public void StartInitialMovementPattern(){
			_movementPatternInUse = _initialMovementPattern;
		}
	}
}