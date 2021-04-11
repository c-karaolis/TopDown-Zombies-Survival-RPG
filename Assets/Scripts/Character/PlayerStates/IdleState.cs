using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System;

namespace Foxlair.Character.States
{
    public class IdleState : State
    {
        public override void OnStateEnter() 
        {
                 
        }

        private void CheckForMovementInput()
        {
            if (InputHandler.Instance.isMovementButtonsDown)
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;

                ChangeState(playerStateMachine.runningState);

                //ChangeState(((PlayerStateMachine)StateMachine).runningState);
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