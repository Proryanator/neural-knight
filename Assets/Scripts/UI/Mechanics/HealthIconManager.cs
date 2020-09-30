using HealthAndDamage;
using UnityEngine;

namespace UI.Mechanics{
	public class HealthIconManager : MonoBehaviour{
		private HealthIconSwapper[] _healthIconSwappers;

		private PlayerHealth _playerHealth;
		
		private void Awake(){
			_healthIconSwappers = GetComponentsInChildren<HealthIconSwapper>();
			
			// TODO: coordinate getting this not this way
			_playerHealth = FindObjectOfType<PlayerHealth>();

			// call this once to initialize things
			_playerHealth.OnHealthChanged += UpdateHealthIcons;
		}
		
		private void UpdateHealthIcons(int currentHealth, int maxHealth){
			DisableAllHealthSwappers();

			EnableForMaxHealth(maxHealth);

			TurnOnOrOff(currentHealth, maxHealth);
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