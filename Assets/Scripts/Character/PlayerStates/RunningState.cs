using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class RunningState : State
    {
        public PlayerStateMachine playerStateMachine;

        public override void OnStateEnter()
        {
            ForbiddenTransitions.Add(this);
            playerStateMachine.PlayerCharacter.PlayerAnimator.SetTrigger("RUNNING");
        }


        public override void OnStateExecute()
        {
            playerStateMachine.PlayerCharacter.CharacterMovement.UpdateCharacterMovement();
            CheckForMovementInput();
        }


        private void CheckForMovementInput()
        {
            if (playerStateMachine.PlayerCharacter.CharacterMovement.Direction.magnitude <= 0.1f)
            {
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}