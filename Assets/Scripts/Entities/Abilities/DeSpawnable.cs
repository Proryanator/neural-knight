using System;
using UnityEngine;

namespace Entities.Abilities{
	
	/// <summary>
	/// Simply exposes a method to use that will be called when this object is 'despawned'.
	/// </summary>
	public class DeSpawnable : MonoBehaviour{

		// an event meant to attach any void method to when this object is despawned
		public Action OnDeSpawn;
		
		private void OnDestroy(){
			OnDeSpawn?.Invoke();
		}
	}
}