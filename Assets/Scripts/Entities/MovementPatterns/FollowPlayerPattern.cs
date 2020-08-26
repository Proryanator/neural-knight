using Pathfinding;
using UnityEngine;
using Utils;

namespace Entities.MovementPatterns{
	/// <summary>
	/// Forces this data object to follow the player using A* path-finding.
	/// </summary>
	[RequireComponent(typeof(Seeker))]
	[RequireComponent(typeof(Rigidbody2D))]
	public class FollowPlayerPattern : AbstractMovementPattern{

		[Tooltip("Speed at which to walk towards the player.")] 
		[SerializeField] private float _speed = 5f;

		[Tooltip("The interval at which to update the path. A higher value will be more realistic but more costly.")]
		[SerializeField] private float _repeatRate = .5f;
		
		[Tooltip("The distance between way-points for the AI system to generate")] 
		[SerializeField] private float _nextWayPointDistance = 3f;
		
		// references to other scripts
		private Seeker _seeker;
		private Rigidbody2D _rigidbody2D;

		// the player object that we'll be following
		// NOTE: we might need to adjust this to find the 'nearest' player instead of the default player,
		// when working with multiple players
		private Transform _playerTarget;

		// the current path we're following from the path-finding system
		private Path _path;

		// this is the current way point we're travelling to?
		private int _currentWayPoint = 0;
		
		private bool _reachedEndOfPath;
		
		// if your rotation is inverted or larger than a threshold for more than this amount of frames,
		// then apply the rotation. Otherwise, skip it
		private int _frameSkipForInvertedRotation = 2;
		private int _frameSkipCount = 0;
		
		// keeps track of the last waypoint, we may need to use it to get around an odd bug
		private Vector2 _lastDirection;

		private void Start(){
			// determine if we're allowed to agro the player or not right now
			// called when this script is first added
			_seeker = GetComponent<Seeker>();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_playerTarget = GetPlayerTarget();

			if (_playerTarget == null){
				Debug.LogError(
					"No player object was found, did you change the implementation of how a player is found?");
			}
			else{
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

			// if this is the opposite direction, this is most likely a problem
			// just use the last direction from before
			if (Vector2.Dot(transform.position, directionToMove) == 0){
				// just move in the direction you were last time and return
				directionToMove = _lastDirection;
			}else{
				// store this as the last direction you moved
				_lastDirection = directionToMove;
			}
			
			// now we simply translate in that direction based on the speed!
			_rigidbody2D.velocity = (_speed * directionToMove);

			// determine if we've reached the next way-point, track the next one
			float distance = Vector2.Distance(transform.position, _path.vectorPath[_currentWayPoint]);

			if (distance < _nextWayPointDistance){
				_currentWayPoint++;
			}
			
			RotateTowardsDirectionIfNotInvertedBug(directionToMove);
		}

		private void UpdatePath(){
			// only if you're done calculating your previous path, do this again!
			if (_seeker.IsDone()){
				_seeker.StartPath(transform.position, _playerTarget.position, OnPathComplete);
			}
		}

		/// <summary>
		/// Rotates the sprite towards the direction of movement.
		///
		/// However, if a complete inverse rotation is detected, will skip this for a few frames.
		/// This fixes a problem where every X number of frames, a sprite just completely flips around
		/// and faces the wrong direction.
		///
		/// This logic might need to be repeated for other controllers but, for now this will do.
		/// </summary>
		/// <param name="directionToMove"></param>
		private void RotateTowardsDirectionIfNotInvertedBug(Vector2 directionToMove){
			// the rotation to apply
			Quaternion rotationToApply = Utils2D.GetRotationTowardsDirection(directionToMove, startingDirection);
			
			// is this rotation a complete flip of the previous one? If so, skip it if we can
			// seems like 170 is the correct amount to fix this!
			if (Utils2D.AreRotationsLargerThanAngle(transform.rotation, rotationToApply, 170)){
				// if this counter is less than the maximum, skip the frame
				if (_frameSkipCount < _frameSkipForInvertedRotation){
					Debug.Log("Angles are too large, skipping the frame.");
					// count this frame as 'skipped'
					_frameSkipCount++;
					return;
				}
			}
			
			// if the rotation was not a complete flip, or we already skipped enough frames, then apply the rotation
			transform.rotation = rotationToApply;
			_frameSkipCount = 0;
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