
using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class AttackingState : State
    {
        public PlayerStateMachine playerStateMachine;
        bool firstTimeEnteredAttackPressed;

        private void Start()
        {
            ForbiddenTransitions.Add(this);
            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public override void OnStateEnter()
        {
            HandleNotInRangeToAttack();
            //if (!playerStateMachine.PlayerCharacter.PlayerAnimator.GetBool("Attacking"))
            //{
            //    playerStateMachine.PlayerCharacter.PlayerAnimator.SetBool("ATTACKING", true);
            //}

        }

        private void HandleNotInRangeToAttack()
        {
            if (playerStateMachine.PlayerCharacter.Target != null && !playerStateMachine.PlayerCharacter.GetPlayerWeapon().InRangeToAttack())
            {
                playerStateMachine.PlayerCharacter.isExecutingAnAttackMove = true;
                ChangeState(playerStateMachine.MovingToAttackState);
            }
        }

        public override void OnStateExecute()
        {
            playerStateMachine.PlayerCharacter.characterMovement.HandleAutoTargetingRotation();

            playerStateMachine.PlayerCharacter.GetPlayerWeapon().DetermineAttack();
            if (!InputHandler.Instance.IsFiringButtonDown && !playerStateMachine.PlayerCharacter.isExecutingAnAttackMove)
            {
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit()
        {
            playerStateMachine.PlayerCharacter.playerAnimator.SetBool("ATTACKING", false);
        }


    }

}