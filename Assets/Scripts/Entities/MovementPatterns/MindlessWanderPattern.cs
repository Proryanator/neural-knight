using UnityEngine;

namespace Entities.MovementPatterns{
	/// <summary>
	/// Movement pattern of moving in a random direction for an amount of time,
	/// then switching your direction.
	///
	/// NOTE: this could be the basis of most random movement scripts, with adding
	/// variations of changing direction slightly over time (to move in a circle), etc.
	///
	/// TODO: Might want to pass in speed values from the using class, but this works for now. 
	/// </summary>
	public class MindlessWanderPattern : AbstractAIMovementPattern{

		[Tooltip("True; moves the object freely in any random direction, not constricted to up/down/left/right.")]
		[SerializeField]
		private bool _moveFreeForm = false;

		[Tooltip(
			"The speed at which this object wanders across the screen. Has no affect on the wander time, just movement speed.")]
		[SerializeField]
		private float _wanderSpeed = 5f;

		[Tooltip(
			"The maximum amount of time this data will wander around in a given direction. Used to randomly decide when to change directions.")]
		[SerializeField]
		private float _wanderMaxInterval = 10f;

		[Tooltip(
			"The minimum amount of time this data will wander around in a given direction. Used to randomly decide when to change directions.")]
		[SerializeField]
		private float _wanderMinInterval = 5f;

		// stores the current amount of time to wander around
		private float _currentWanderTime;

		// the time we started wandering
		private float _wanderStartTime;

		// the current movement direction of the 
		private Vector2 _direction;

		private void Start(){
			// starts us off in a certain direction
			ChooseDirectionAndTime();
		}

		public override void Move(){
			transform.Translate(Time.deltaTime * _wanderSpeed * _direction);

			// will change direction and wander time if it's time to change
			ChangeDirectionAndTime();
		}

		/// <summary>
		/// The only method to call to get your random direction.
		/// </summary>
		private Vector2 GetRandomDirection(){
			return _moveFreeForm ? GetRandomFreeFormDirection() : GetRandom8WayDirection();
		}

		/// <summary>
		/// A Vector2 that represents a random direction in the 8-way movement, much
		/// like that of using a WASD keyboard for movement.
		/// </summary>
		private Vector2 GetRandom8WayDirection(){
			// TODO: generate this!
			return Vector2.up;
		}

		private Vector2 GetRandomFreeFormDirection(){
			float random = Random.Range(0f, 360f);
			return new Vector2(Mathf.Cos(random), Mathf.Sin(random));
		}

		/// <summary>
		/// How long to move in the current direction for.
		///
		/// A random time picked between 2 numbers.
		///
		/// In a separate method just in case we want to add more logic to how
		/// this time is decided.
		/// </summary>
		/// <returns></returns>
		private float GetHowLongToWander(float min, float max){
			return Random.Range(min, max);
		}

		/// <summary>
		/// Called to shake up the direction and the time we'll wander in this direction.
		/// </summary>
		private void ChooseDirectionAndTime(){
			_direction = GetRandomDirection();
			_currentWanderTime = GetHowLongToWander(_wanderMinInterval, _wanderMaxInterval);
			_wanderStartTime = Time.time;
		}

		/// <summary>
		/// Handles when to change direction of the object.
		/// </summary>
		private void ChangeDirectionAndTime(){
			if (Time.time - _wanderStartTime >= _currentWanderTime){
				ChooseDirectionAndTime();
			}
		}
	}
}