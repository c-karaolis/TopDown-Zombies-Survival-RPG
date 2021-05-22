
using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class AttackingState : State
    {
        public PlayerStateMachine playerStateMachine;

        private void Start()
        {
            ForbiddenTransitions.Add(this);
            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public override void OnStateEnter()
        {
            HandleNotInRangeToAttack();
        }

        private void HandleNotInRangeToAttack()
        {
            if (playerStateMachine.PlayerCharacter.PlayerTargetEnemy != null && !playerStateMachine.PlayerCharacter.GetPlayerWeapon().InRangeToAttack())
            {
                ChangeState(playerStateMachine.MovingToAttackState);
            }
        }

        public override void OnStateExecute()
        {
            playerStateMachine.PlayerCharacter.CharacterMovement.HandleAutoTargetingRotation();

            playerStateMachine.PlayerCharacter.GetPlayerWeapon().DetermineAttack(); 
            if (!InputHandler.Instance.IsFiringButtonDown)
            {
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }




    }

}