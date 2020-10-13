using UnityEngine;

namespace Weapons.Ammo {
	public class AmmoBox : MonoBehaviour {
		[SerializeField] private int _ammo;

		public int Ammo => _ammo;
	}
}