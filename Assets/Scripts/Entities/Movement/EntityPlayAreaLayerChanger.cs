using UnityEngine;
using Utils;

namespace Entities.Movement{
	public class EntityPlayAreaLayerChanger : MonoBehaviour{
		[SerializeField] private string _insideOfPlayAreaLayer = AllLayers.ENTITY;

		public void SetToInsidePlayAreaLayer(){
			gameObject.layer = LayerMask.NameToLayer(_insideOfPlayAreaLayer);
		}
	}
}