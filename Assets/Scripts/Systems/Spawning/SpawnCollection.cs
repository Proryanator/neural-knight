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

	public Transform[] GetSpawnPoints(){
		return _spawnPointObjects;
	}

}