using System.Collections.Generic;
using Entities.Events;
using UnityEngine;
using Random = System.Random;

namespace Systems.Spawning.Rules{
	/// <summary>
	/// Determines where/how to distribute the spawning across spawn points.
	///
	/// Does take in the max spawn count, as well as the current spawn count.
	/// This allows for some more advanced spawn rules, if needed.
	///
	/// These are intended to be passed in and cached, not defined here.
	/// </summary>
	public abstract class AbstractSpawnRule : ScriptableObject{

		private Transform prefab;
		private SpawnPoint[] spawnPoints;
		protected int maxSpawnCount;

		public void Init(Transform prefab, SpawnPoint[] spawnPoints, int maxSpawnCount){
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
		public abstract int Spawn(SpawnProperties props);

		/// <summary>
		/// Simply spawns the prefab at the spawn point location. Handled once in 1 place for
		/// all spawn rules.
		///
		/// Spawns how ever many you ask it to, and returns that number.
		///
		/// Also, attaches a decrement method to the 'OnDeSpawn' method for each of them.
		/// </summary>
		protected int SpawnAt(Transform point, int spawnCount, SpawnProperties props){
			for (int i = 0; i < spawnCount; i++){
				Transform spawned = Instantiate(prefab, point.position, Quaternion.identity);
				DeSpawnable deSpawnable = spawned.GetComponentInChildren<DeSpawnable>();
				if (deSpawnable == null){
					Debug.LogWarning("Not able to attach decrement call to this object, it's missing a 'DeSpawnable' script.");
				}else{
					deSpawnable.OnDeSpawn += props.DecrementSceneCount;
				}
			}

			return spawnCount;
		}

		protected Transform GetRandomSpawnPoint(){
			SpawnPoint[] activePoints = GetOnlyActiveSpawnPoints();
			Random random = new Random();
			return activePoints[random.Next(activePoints.Length)].transform;
		}

		private SpawnPoint[] GetOnlyActiveSpawnPoints(){
			List<SpawnPoint> activePoints = new List<SpawnPoint>();
			foreach (SpawnPoint spawnPoint in spawnPoints){
				if (spawnPoint.gameObject.activeSelf){
					activePoints.Add(spawnPoint);
				}
			}

			return activePoints.ToArray();
		}
	
		/// <summary>
		/// Returns you a new instance of the given spawn rule.
		///
		/// NOTE: it's up to the caller to cleanup their own objects before getting a new one.
		/// </summary>
		public static AbstractSpawnRule GetRule(SpawnRuleEnum ruleEnum, Transform prefab, List<SpawnPoint> spawnPoints, int maxSpawnCount){
			AbstractSpawnRule rule = null;

			if (spawnPoints == null || spawnPoints.Count == 0){
				Debug.LogWarning("You passed in a null or empty array of spawn points, spawning won't work.");
				return null;
			}
		
			switch (ruleEnum){
				case SpawnRuleEnum.Random:
					rule = CreateInstance<RandomSpawnRule>();
					break;
				case SpawnRuleEnum.Bulk:
					rule = CreateInstance<BulkSpawnRule>();
					break;
			}

			if (rule != null){
				rule.Init(prefab, spawnPoints.ToArray(), maxSpawnCount);
			}

			return rule;
		}

		public void ReSetSpawnPoints(SpawnPoint[] points){
			spawnPoints = points;
		}
	}
}