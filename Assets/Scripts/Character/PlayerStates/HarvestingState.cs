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
        ResourceNode PlayerTargetResourceNode;

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

            if (PlayerTargetResourceNode != null && !playerStateMachine.PlayerCharacter.InRangeToHarvest())
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

        private void UnsetResourceNode()
        {
            PlayerTargetResourceNode = null;
        }

        private void SetResourceNode(ResourceNode obj)
        {
            PlayerTargetResourceNode = obj;
        }
    }
}