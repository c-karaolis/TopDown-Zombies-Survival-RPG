using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System;
using UnityEngine;

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
            if (Input.GetKeyDown(KeyCode.J))
            {
                foreach(string testingItem in PlayerManager.Instance.testingItems)
                {
                    //PlayerManager.Instance.MainPlayerCharacterInventory.AddItem(testingItem, 1);
                }
                
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }


    }
}