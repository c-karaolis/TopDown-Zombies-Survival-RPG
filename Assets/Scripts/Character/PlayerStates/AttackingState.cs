
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
            forbiddenTransitions.Add(this);

        }

        public CharacterMovement characterMovement;

        public override void OnStateEnter() 
        {
        }

        public override void OnStateExecute()
        {
            if (!InputHandler.Instance.isFiringButtonDown) 
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.idleState);
            }
            characterMovement.HandleAutoTargetingRotation();
            PlayerManager.Instance.playerEquippedWeapon.DetermineAttack();
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }




    }

}