using UnityEngine;
using Utils;

namespace Entities.SpecificControllers.EnemyDataPoint {
	public class MoveTowardsPlayerController : MonoBehaviour {

		private Transform _playerTransform;
		private bool _enabled;

		private float _lerpAmount;
		[SerializeField] private float _lerpFactor = 0.1f;

		private void Start(){
			_playerTransform = GameObject.FindGameObjectWithTag(AllTags.PLAYER).transform;
		}

		private void Update(){
			if (_enabled){
				_lerpAmount += Time.deltaTime;
				transform.position = Vector2.Lerp(transform.position, _playerTransform.position, _lerpAmount * _lerpFactor);
			}
			else{
				_lerpAmount = 0f;
			}
		}

		private void OnTriggerEnter2D(Collider2D other){
			if (other.gameObject.tag.Equals(AllTags.PLAYER)){
				_enabled = true;
			}
		}
	}
}