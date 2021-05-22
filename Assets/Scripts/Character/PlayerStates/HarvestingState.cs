using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class HarvestingState : State
    {
        public PlayerStateMachine playerStateMachine;

        private void Start()
        {
            ForbiddenTransitions.Add(this);
            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public override void OnStateEnter()
        {
            HandleNotInRangeToHarvest();
        }

        private void HandleNotInRangeToHarvest()
        {

            if (PlayerManager.Instance.PlayerTargetResourceNode != null && !PlayerManager.Instance.MainPlayerCharacter.InRangeToHarvest())
            {
                ChangeState(playerStateMachine.MovingToHarvestState);
            }
        }

        public override void OnStateExecute()
        {

            playerStateMachine.PlayerCharacter.CharacterMovement.HandleAutoTargetingRotation();
           // PlayerManager.Instance.PlayerEquippedWeapon.DetermineAttack();
            if (!InputHandler.Instance.IsInteractionButtonDown)
            {
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }


    }
}