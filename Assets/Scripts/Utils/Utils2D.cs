using UnityEngine;

namespace Utils{
	public class Utils2D : ScriptableObject{
		
		/// <summary>
		/// Returns you a rotation in the direction you're requesting!
		/// </summary>
		/// <param name="transform">The transform of the object to rotate</param>
		/// <param name="direction">The direction to face towards</param>
		/// <param name="startingDirection">The starting direction of the sprite</param>
		public static Quaternion GetRotationTowardsDirection(Vector2 direction, FacingDirection startingDirection){
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			return GetRotationForStartingDirection(angle, startingDirection);
		}

		/// <summary>
		/// Given an angle threshold, tells you if the second rotation is higher than the threshold rotation
		/// from the first.
		/// </summary>
		/// <param name="first">The first rotation</param>
		/// <param name="second">The second rotation (often the rotation to go to)</param>
		/// <param name="angleThreshold">A minimum difference of rotation allowed between 2 rotations</param>
		public static bool AreRotationsLargerThanAngle(Quaternion first, Quaternion second, int angleThreshold){
			return Quaternion.Angle(first, second) >= angleThreshold;
		}
		
		public static Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction){
			return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
		}
	}
}