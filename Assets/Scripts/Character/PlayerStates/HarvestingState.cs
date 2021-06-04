using Foxlair.Character.Movement;
using Foxlair.Harvesting;
using Foxlair.PlayerInput;
using Foxlair.Tools.Events;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class HarvestingState : State
    {
        public PlayerStateMachine playerStateMachine;
        IInteractable PlayerTargetInteractable;

        private void Start()
        {
            ForbiddenTransitions.Add(this);
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event += SetResourceNode;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event += UnsetResourceNode;

            //ForbiddenTransitions.Add(playerStateMachine.MovingToAttackState);
        }

        public override void OnStateEnter()
        {
            HandleNotInRangeToHarvest();
        }

        private void HandleNotInRangeToHarvest()
        {

            if (PlayerTargetInteractable != null && !playerStateMachine.PlayerCharacter.InRangeToHarvest())
            {
                ChangeState(playerStateMachine.MovingToHarvestState);
            }
        }

        public override void OnStateExecute()
        {

            playerStateMachine.PlayerCharacter.characterMovement.HandleAutoTargetingRotation();
           // PlayerManager.Instance.PlayerEquippedWeapon.DetermineAttack();
            if (!InputHandler.Instance.IsInteractionButtonDown)
            {
                ChangeState(playerStateMachine.IdleState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }

        private void UnsetResourceNode()
        {
            PlayerTargetInteractable = null;
        }

        private void SetResourceNode(IInteractable obj)
        {
            PlayerTargetInteractable = obj;
        }
    }
}