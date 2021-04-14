using Foxlair.Character.Movement;
using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class HarvestingState : State
    {

        private void Start()
        {
            PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(this);
            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public CharacterMovement characterMovement;

        public override void OnStateEnter()
        {
            HandleNotInRangeToHarvest();
        }

        private void HandleNotInRangeToHarvest()
        {

            if (PlayerManager.Instance.PlayerTargetResourceNode != null && !PlayerManager.Instance.MainPlayerCharacter.InRangeToHarvest())
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.MovingToHarvestState);
            }
        }

        public override void OnStateExecute()
        {

            characterMovement.HandleAutoTargetingRotation();
            PlayerManager.Instance.PlayerEquippedWeapon.DetermineAttack();
            if (!InputHandler.Instance.IsInteractionButtonDown)
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }


    }
}