using System;
using UnityEngine;

namespace Entities.Events{
	public class DeSpawnable : MonoBehaviour{

		private bool _isQuitting;
		
		// an event meant to attach any void method to when this object is despawned
		public Action OnDeSpawn;

		private void OnApplicationQuit(){
			_isQuitting = true;
		}

		private void OnDestroy(){
			// only call this if we're not exiting the application (this could cause some weird side-effects
			if (!_isQuitting){
				OnDeSpawn?.Invoke();
			}
		}
	}
}