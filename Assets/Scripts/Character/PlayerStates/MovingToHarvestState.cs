using Foxlair.Character.Movement;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class MovingToHarvestState : State
    {
        public PlayerStateMachine playerStateMachine;

        public override void OnStateEnter()
        {
            ForbiddenTransitions.Add(playerStateMachine.HarvestingState);

            if (!playerStateMachine.PlayerCharacter.playerAnimator.GetBool("RUNNING"))
            {
                playerStateMachine.PlayerCharacter.playerAnimator.SetBool("RUNNING", true);
            }
        }

        public override void OnStateExecute()
        {

            if (!playerStateMachine.PlayerCharacter.InRangeToHarvest())
            {
                playerStateMachine.PlayerCharacter.characterMovement.HandleAutoMoveToHarvest(playerStateMachine.PlayerCharacter.playerTargetInteractable.ImplementingMonoBehaviour().transform);
            }
            else
            {
                ChangeState(playerStateMachine.HarvestingState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit()
        {
            playerStateMachine.PlayerCharacter.playerAnimator.SetBool("RUNNING", false);
        }
    }
}