using System;
using UnityEditorInternal;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Determines where/how to distribute the spawning across spawn points.
///
/// Does take in the max spawn count, as well as the current spawn count.
/// This allows for some more advanced spawn rules, if needed.
///
/// These are intended to be passed in and cached, not defined here.
/// </summary>
public abstract class AbstractSpawnRule : ScriptableObject{

	protected Transform prefab;
	protected SpawnPoint[] spawnPoints;
	protected uint maxSpawnCount;

	public void Init(Transform prefab, SpawnPoint[] spawnPoints, uint maxSpawnCount){
		this.prefab = prefab;
		this.spawnPoints = spawnPoints;
		this.maxSpawnCount = maxSpawnCount;
	}
	
	/// <summary>
	/// Handles whether or not to spawn said prefab, and how to spawn it.
	/// Right now, just spawns 1 of that prefab.
	///
	/// Returns the number of prefabs spawned!
	/// </summary>
	public abstract uint Spawn(uint currentSpawnCount);

	/// <summary>
	/// Simply spawns the prefab at the spawn point location. Handled once in 1 place for
	/// all spawn rules.
	///
	/// Spawns how ever many you ask it to, and returns that number.
	/// </summary>
	protected uint SpawnAt(SpawnPoint point, uint spawnCount){
		for (uint i = 0; i < spawnCount; i++){
			GameObject.Instantiate(prefab, point.transform.position, Quaternion.identity);
		}
		
		return spawnCount;
	}

	protected SpawnPoint GetRandomSpawnPoint(){
		Random random = new Random();
		return spawnPoints[random.Next(spawnPoints.Length)];
	}
	
	/// <summary>
	/// Returns you a new instance of the given spawn rule.
	///
	/// NOTE: it's up to the caller to cleanup their own objects before getting a new one.
	/// </summary>
	public static AbstractSpawnRule GetRule(SpawnRuleEnum ruleEnum, Transform prefab, SpawnPoint[] spawnPoints, uint maxSpawnCount){
		AbstractSpawnRule rule = null;
		
		switch (ruleEnum){
			case SpawnRuleEnum.Random:
				rule = ScriptableObject.CreateInstance<RandomSpawnRule>();
				break;
			case SpawnRuleEnum.Bulk:
				rule = ScriptableObject.CreateInstance<BulkSpawnRule>();
				break;
		}

		if (rule != null){
			rule.Init(prefab, spawnPoints, maxSpawnCount);
		}

		return rule;
	}
}