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
            if (!playerStateMachine.PlayerCharacter.playerAnimator.GetBool("IDLE"))
            {
                playerStateMachine.PlayerCharacter.playerAnimator.SetBool("IDLE", true);
            }
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

        public override void OnStateExit()
        {
                playerStateMachine.PlayerCharacter.playerAnimator.SetBool("IDLE", false);
        }


    }
}