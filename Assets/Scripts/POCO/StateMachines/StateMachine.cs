﻿using System;
using System.Collections.Generic;

namespace POCO.StateMachines{
	public class StateMachine{
		private State currentState;

		private readonly List<Transition> transitionFromAnyState = new List<Transition>();

		public void Tick(){
			Transition transition = GetTransitionIfAvailable(currentState);
			if (transition != null){
				SetState(transition.To);
			}

			currentState?.Tick();
		}

		public void SetState(State state){
			if (IsState(state)){
				return;
			}

			currentState?.OnExit();
			currentState = state;

			currentState.OnEnter();
		}

		public bool IsState(State state){
			return currentState == state;
		}
		
		public void AddAnyTransition(State state, Func<bool> predicate){
			transitionFromAnyState.Add(new Transition(state, predicate));
		}

		private Transition GetTransitionIfAvailable(State currentState){
			Transition transitionIfAny = GetNextTransitionFromList(transitionFromAnyState);

			if (transitionIfAny == null || transitionIfAny.To == currentState){
				transitionIfAny = GetNextTransitionFromList(this.currentState.Transitions);
			}

			return transitionIfAny;
		}

		private static Transition GetNextTransitionFromList(List<Transition> transitions){
			foreach (Transition transition in transitions){
				if (transition.Condition()){
					return transition;
				}
			}

			return null;
		}
	}
}