using System.Collections.Generic;
using UnityEngine;

namespace Utils{
	public class UnityUtilities : ScriptableObject{

		public static Transform[] GetAllChildren(Transform trans){
			List<Transform> allChildren = new List<Transform>();
			foreach (Transform child in trans){
				allChildren.Add(child);
			}

			return allChildren.ToArray();
		}
	}	
}