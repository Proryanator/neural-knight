using UnityEngine;

namespace Utils{
	public class Utils2D : ScriptableObject{
		
		/// <summary>
		/// Rotates the transform in the direction of the direction, based off of your starting direction.
		/// </summary>
		/// <param name="transform">The transform of the object to rotate</param>
		/// <param name="direction">The direction to face towards</param>
		/// <param name="_startingDirection">The starting direction of the sprite</param>
		public static void RotateToFaceDirection(Transform transform, Vector2 direction, FacingDirection _startingDirection){
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			
			// taking initial direction into account, now rotate towards the mouse!
			transform.rotation = GetRotationForStartingDirection(angle, _startingDirection);
		}
		
		public static Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction){
			return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
		}
	}
}