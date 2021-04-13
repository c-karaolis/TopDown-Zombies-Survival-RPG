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
            if (InputHandler.Instance.IsMovementButtonsDown)
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;  //ChangeState(((PlayerStateMachine)StateMachine).RunningState);
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