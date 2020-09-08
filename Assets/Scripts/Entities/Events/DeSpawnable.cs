using System;
using UnityEngine;

namespace Entities.Events{
	public class DeSpawnable : MonoBehaviour{

		// an event meant to attach any void method to when this object is despawned
		public Action OnDeSpawn;
		
		private void OnDestroy(){
			OnDeSpawn?.Invoke();
		}
	}
}