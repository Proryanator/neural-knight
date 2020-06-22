using System;
using UnityEngine;

/// <summary>
/// Determines where/how to distribute the spawning across spawn points.
///
/// Spawn rate, max spawn, etc. are not handled here. All these
/// rules handle are where to spawn the objects themselves.
/// </summary>
public abstract class AbstractSpawnRule : ScriptableObject{
	
	/// <summary>
	/// Handles whether or not to spawn said prefab, and how to spawn it.
	/// Right now, just spawns 1 of that prefab.
	/// </summary>
	public abstract void Spawn(Transform prefab, SpawnPoint[] points);

	/// <summary>
	/// Simply spawns the prefab at the spawn point location. Handled once in 1 place for
	/// all spawn rules.
	/// </summary>
	protected void SpawnAt(Transform prefab, SpawnPoint point){
		GameObject.Instantiate(prefab, point.transform.position, Quaternion.identity);
	}

	/// <summary>
	/// Returns you a new instance of the given spawn rule.
	///
	/// NOTE: it's up to the caller to cleanup their own objects before getting a new one.
	/// </summary>
	public static AbstractSpawnRule GetRule(SpawnRule rule){
		switch (rule){
			case SpawnRule.Random:
				return ScriptableObject.CreateInstance<RandomSpawnRule>();
			default:
				return null;
		}
	}
}