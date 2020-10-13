using Entities.Movement.EntityStates;

namespace Entities.Movement.Controllers{
	public class NonEnemyMovementController : AbstractEntityMovementController{

		private NormalMovementState _normalMovementState;
		
		private void Awake(){
			base.Awake();
			
			_normalMovementState = new NormalMovementState(GetNormalMovementPattern());
			moveToPlayAreaState.AddTransition(_normalMovementState, IsInPlayArea());
		}
		
		private void Update(){
			base.Update();
		}
	}
}