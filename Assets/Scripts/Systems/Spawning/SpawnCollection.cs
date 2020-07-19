using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All children under this object are spawn points, and all have the same kind of
/// spawn-able object.
/// </summary>
public class SpawnCollection : MonoBehaviour{

	private Transform[] _spawnPointObjects;

	private void Awake(){
		List<Transform> _children = new List<Transform>();
		foreach (Transform child in transform){
			_children.Add(child);
		}

		_spawnPointObjects = _children.ToArray();
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