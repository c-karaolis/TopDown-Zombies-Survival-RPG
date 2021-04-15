
using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;

namespace Foxlair.Character.States
{
    public class AttackingState : State
    {

        private void Start()
        {
            PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(this);
            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public override void OnStateEnter()
        {
            HandleNotInRangeToAttack();
        }

        private void HandleNotInRangeToAttack()
        {
            if (PlayerManager.Instance.PlayerTargetEnemy != null && !PlayerManager.Instance.PlayerEquippedWeapon.InRangeToAttack())
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.MovingToAttackState);
            }
        }

        public override void OnStateExecute()
        {

            PlayerManager.Instance.MainPlayerCharacterMovement.HandleAutoTargetingRotation();
            PlayerManager.Instance.PlayerEquippedWeapon.DetermineAttack();
            if (!InputHandler.Instance.IsFiringButtonDown)
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }




    }

}