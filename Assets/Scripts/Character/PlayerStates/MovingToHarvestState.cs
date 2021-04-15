using Foxlair.Character.Movement;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class MovingToHarvestState : State
    {

        private void Start()
        {

        }
        public override void OnStateEnter()
        {
            PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(playerStateMachine.HarvestingState);
        }

        public override void OnStateExecute()
        {

            if (!PlayerManager.Instance.MainPlayerCharacter.InRangeToHarvest())
            {
                PlayerManager.Instance.MainPlayerCharacterMovement.HandleAutoMoveToHarvest(PlayerManager.Instance.PlayerTargetResourceNode.transform);
            }
            else
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.HarvestingState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}