using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class RunningState : State
    {

        public CharacterMovement _characterMovement;

        public override void OnStateEnter()
        {
            PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(this);
        }


        public override void OnStateExecute()
        {
            _characterMovement.UpdateCharacterMovement();
            CheckForMovementInput();
        }


        private void CheckForMovementInput()
        {
            if (_characterMovement.Direction.magnitude <= 0.1f)
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.IdleState);
                //ChangeState(((PlayerStateMachine)StateMachine).runningState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}