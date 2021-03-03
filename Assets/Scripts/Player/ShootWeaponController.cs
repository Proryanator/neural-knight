using Player.Animations;
using Proryanator.Controllers2D;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;
using Weapons.Projectiles;

namespace Player{
	/// <summary>
	/// Controls when the player takes input, and performs an attack action.
	/// </summary>
	[RequireComponent(typeof(WeaponManager))]
	public class ShootWeaponController : MonoBehaviour{

		private WeaponManager _weaponManager;
		private TDC_FaceMouse _faceMouseController;
		private BodyAnimationController _bodyAnimationController;
		private ProjectileSpawnPoint _projectileSpawnPoint;

		[SerializeField] private GameObject _cursorPrefab;
		
		private void Start(){
			_weaponManager = GetComponent<WeaponManager>();
			_faceMouseController = GetComponent<TDC_FaceMouse>();
			_bodyAnimationController = GetComponentInChildren<BodyAnimationController>();
			_projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>();
			
			// set the player's cursor to that of a unique icon!
			Texture2D cursorTexture = _cursorPrefab.GetComponent<SpriteRenderer>().sprite.texture;
			Vector2 centerOfTexture = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
			Cursor.SetCursor(cursorTexture, centerOfTexture, CursorMode.ForceSoftware);
		}

		/// <summary>
		/// Fires the currently equipped weapon. Accesses the WeaponManager class to gain this information.
		/// </summary>
		public void FireWeapon(InputAction.CallbackContext context){
			_weaponManager.GetEquippedWeapon().Fire(_projectileSpawnPoint.transform, _faceMouseController.GetFacingDirection());
			_bodyAnimationController.ActivateShotAnimation();
		}
	}
}