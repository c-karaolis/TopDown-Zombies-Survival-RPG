
using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;

namespace Foxlair.Character.States
{
    public class AttackingState : State
    {

        PlayerStateMachine playerStateMachine;
        private void Start()
        {
            playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(this);

        }

        public CharacterMovement characterMovement;

        public override void OnStateEnter() 
        {
        }

        public override void OnStateExecute()
        {
            if (!InputHandler.Instance.IsFiringButtonDown) 
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.IdleState);
            }
            characterMovement.HandleAutoTargetingRotation();
            PlayerManager.Instance.PlayerEquippedWeapon.DetermineAttack();
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }




    }

}