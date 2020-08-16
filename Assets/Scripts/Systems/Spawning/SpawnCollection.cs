using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Systems.Spawning{
	/// <summary>
	/// All children under this object are spawn points, and all have the same kind of
	/// spawn-able object.
	/// </summary>
	public class SpawnCollection : MonoBehaviour{

		private Transform[] _spawnPointObjects;

		private void Awake(){
			_spawnPointObjects = UnityUtilities.GetAllChildren(transform);
		}

		/// <summary>
		/// Gives you the list of spawn points that are active under the collection.
		/// </summary>
		public Transform[] GetActiveSpawnPoints(){
			List<Transform> _activeSpawnPoints = new List<Transform>();
			foreach (Transform trans in _spawnPointObjects){
				if (trans.gameObject.activeSelf){
					_activeSpawnPoints.Add(trans);
				}
			}
		
			return _activeSpawnPoints.ToArray();
		}

	}
}