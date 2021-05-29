using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class IdleState : State
    {
        public PlayerStateMachine playerStateMachine;

        public override void OnStateEnter()
        {
            playerStateMachine.PlayerCharacter.PlayerAnimator.SetTrigger("IDLE");
        }

        private void CheckForMovementInput()
        {
            if (InputHandler.Instance.IsMovementButtonsDown)
            {
                ChangeState(playerStateMachine.RunningState);
            }
        }

        public override void OnStateExecute() 
        {
            CheckForMovementInput();
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }


    }
}