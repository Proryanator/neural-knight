using HealthAndDamage;
using UnityEngine;

namespace UI.Mechanics{
	public class GridIconManager : MonoBehaviour{
		private GridIconSwapper[] _healthIconSwappers;

		private PlayerHealth _playerHealth;

		[SerializeField] private GameObject _healthIconPrefab;
		
		private void Awake(){
			GetAllHealthIconChildren();
			
			// TODO: coordinate getting this not this way
			_playerHealth = FindObjectOfType<PlayerHealth>();

			// call this once to initialize things
			_playerHealth.OnHealthChanged += UpdateHealthIcons;
		}
		
		private void UpdateHealthIcons(int currentHealth, int maxHealth){
			DestroyOrAddHealthIcons(maxHealth);
			
			DisableAllHealthSwappers();

			EnableForMaxHealth(maxHealth);

			TurnOnOrOff(currentHealth, maxHealth);
		}

		private void DestroyOrAddHealthIcons(int maxHealth){
			bool hasChanged = maxHealth != _healthIconSwappers.Length;
			
			// destroy any excess objects
			for (int i = maxHealth; i < _healthIconSwappers.Length; i++){
				Destroy(_healthIconSwappers[i].gameObject);
			}
			
			// or, spawn any objects that are needed in-scene
			for (int i = 0; i < maxHealth - _healthIconSwappers.Length; i++){
				Instantiate(_healthIconPrefab, transform.position, Quaternion.identity, transform);
			}

			if (hasChanged){
				GetAllHealthIconChildren();
			}
		}

		private void GetAllHealthIconChildren(){
			_healthIconSwappers = GetComponentsInChildren<GridIconSwapper>();
		}
		
		private void TurnOnOrOff(int currentHealth, int maxHealth){
			for (int i = 0; i < maxHealth; i++){
				if (i < currentHealth){
					_healthIconSwappers[i].TurnSpriteOn();
				}
				else{
					_healthIconSwappers[i].TurnOffSprite();
				}
			}
		}
		
		private void EnableForMaxHealth(int maxHealth){
			for (int i = 0; i < maxHealth; i++){
				_healthIconSwappers[i].enabled = true;
			}
		}
		
		private void DisableAllHealthSwappers(){
			foreach (var healthIconSwapper in _healthIconSwappers){
				healthIconSwapper.enabled = false;
			}
		}
	}
}