using UnityEngine;
using Utils;

namespace Entities.Movement{
	public class EntityLayerChanger : MonoBehaviour{
		private string _outsideOfPlayAreaLayer = AllLayers.DEFAULT;

		[SerializeField] private string _insideOfPlayAreaLayer = AllLayers.ENTITY;

		public void SetOutsideOfPlayAreaLayer(){
			gameObject.layer = LayerMask.NameToLayer(_outsideOfPlayAreaLayer);
		}

		public void SetToInsidePlayAreaLayer(){
			gameObject.layer = LayerMask.NameToLayer(_insideOfPlayAreaLayer);
		}
	}
}