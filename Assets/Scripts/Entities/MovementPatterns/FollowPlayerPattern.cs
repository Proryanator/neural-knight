using Pathfinding;
using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	/// <summary>
	/// Forces this data object to follow the player using A* path-finding.
	/// </summary>
	[RequireComponent(typeof(Seeker))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class FollowPlayerPattern : AbstractAIMovementPattern{

		[Tooltip("Speed at which to walk towards the player.")] [SerializeField]
		private float _speed = 5f;

		[Tooltip("The speed at which to update the path. A higher value will be more realistic but more costly.")]
		[SerializeField]
		private float _repeatRate = .5f;

		[Tooltip("The starting facing direction of the sprite")] [SerializeField]
		private FacingDirection _startingDirection = FacingDirection.UP;

		private Seeker _seeker;

		private Rigidbody2D _rigidbody2D;

		// the player object that we'll be following
		// NOTE: we might need to adjust this to find the 'nearest' player instead of the default player,
		// when working with multiple players
		private Transform _playerTarget;

		// distance between way points for the AI system
		[Tooltip("The distance between way-points for the AI system to generate")] [SerializeField]
		private float _nextWayPointDistance = 3f;

		// the current path we're following from the path-finding system
		private Path _path;

		// this is the current way point we're travelling to?
		private int _currentWayPoint = 0;

		[SerializeField] private bool _reachedEndOfPath = false;

		private void Start(){
			_seeker = GetComponent<Seeker>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerTarget = GetPlayerTarget();

			if (_playerTarget == null){
				Debug.LogError(
					"No player object was found, did you change the implementation of how a player is found?");
			}
			else{
				// create a path
				InvokeRepeating("UpdatePath", 0f, _repeatRate);
			}
		}

		public override void Move(){
			// if there isn't a path, return
			if (_path == null){
				return;
			}

			// always determine if we've reached the end of the path yet or not
			if (_currentWayPoint >= _path.vectorPath.Count){
				_reachedEndOfPath = true;
				return;
			}

			// you haven't reached the end of your way-point yet!
			_reachedEndOfPath = false;

			// let's calculate the direction we'll want to travel
			Vector2 directionToMove =
				((Vector2) _path.vectorPath[_currentWayPoint] - (Vector2) transform.position).normalized;

			// now we simply translate in that direction based on the speed!
			// transform.Translate(Time.deltaTime * );
			_rigidbody2D.velocity = (_speed * directionToMove);

			// determine if we've reached the next way-point, track the next one
			float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWayPoint]);

			if (distance < _nextWayPointDistance){
				_currentWayPoint++;
			}

			// as part of moving, let's also face the movement direction, only if there's a movement direction
			if (directionToMove != Vector2.zero){
				RotateToFaceDirection(directionToMove);
			}
		}

		// TODO: need to move up into 2D utils
		private void RotateToFaceDirection(Vector2 direction){
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

			// taking initial direction into account, now rotate towards the mouse!
			transform.rotation = GetRotationForStartingDirection(angle, _startingDirection);
		}

		// TODO: need to move up into 2D utils
		protected Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction){
			return Quaternion.AngleAxis((angle + (float) direction) % 360, Vector3.forward);
		}

		private void UpdatePath(){
			// only if you're done calculating your previous path, do this again!
			if (_seeker.IsDone()){
				_seeker.StartPath(transform.position, _playerTarget.position, OnPathComplete);
			}
		}

		/// <summary>
		/// Will return a player that you want to track.
		///
		/// TODO: for multi-player, get nearest player to track
		/// </summary>
		private Transform GetPlayerTarget(){
			return GameObject.FindWithTag(AllTags.AI_TRACKER).transform;
		}

		/// <summary>
		/// A callback method to be used so we don't freeze up the game when looking for the new path.
		/// </summary>
		private void OnPathComplete(Path path){
			// if we didn't get any errors, we'll save the path and track our current waypoint 
			if (!path.error){
				_path = path;
				_currentWayPoint = 0;
			}
		}
	}
}