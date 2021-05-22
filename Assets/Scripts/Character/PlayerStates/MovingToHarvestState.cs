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
        }

        public override void OnStateExecute()
        {

            if (!PlayerManager.Instance.MainPlayerCharacter.InRangeToHarvest())
            {
                playerStateMachine.PlayerCharacter.CharacterMovement.HandleAutoMoveToHarvest(PlayerManager.Instance.PlayerTargetResourceNode.transform);
            }
            else
            {
                ChangeState(playerStateMachine.HarvestingState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}