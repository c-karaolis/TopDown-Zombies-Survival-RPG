using Foxlair.Harvesting;
using Foxlair.PlayerInput;
using Foxlair.Tools.Events;
using Foxlair.Tools.StateMachine;
using System;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class PlayerStateMachine : StateMachine
    {
        public IdleState IdleState;
        public RunningState RunningState;
        public MovingToAttackState MovingToAttackState;
        public MovingToHarvestState MovingToHarvestState;
        public AttackingState AttackingState;
        public HarvestingState HarvestingState;

        public PlayerCharacter PlayerCharacter;
        //Note: Can add on state enter an enum representation for each state as well if we need it in the codebase somewhere else.

        public override void Start()
        {
            base.Start();
            FoxlairEventManager.Instance.WeaponSystem_OnEquippedWeaponDestroyed_Event += OnWeaponDestroyedGoToIdleState;
        }


        public override void Update()
        {
            base.Update();
            HandleCommonStateTransitions();
        }

        void HandleCommonStateTransitions()
        {
            RunningStateTransition();
            AttackingStateTransition();
            HarvestingStateTransition();
        }

        private void RunningStateTransition()
        {
            if (InputHandler.Instance.IsMovementButtonsDown && !(CurrentState.ForbiddenTransitions.Contains(RunningState)))
            {
                ChangeState(RunningState);
            }
        }

        private void HarvestingStateTransition()
        {
            if (InputHandler.Instance.IsInteractionButtonDown && !(CurrentState.ForbiddenTransitions.Contains(HarvestingState)) && PlayerCharacter.PlayerTargetInteractable is ResourceNode)
            {
                ChangeState(HarvestingState);
            }
        }

        private void AttackingStateTransition()
        {
            if (InputHandler.Instance.IsFiringButtonDown && !(CurrentState.ForbiddenTransitions.Contains(AttackingState)) )
            {
                ChangeState(AttackingState);
            }
        }

        private void OnWeaponDestroyedGoToIdleState()
        {
            ChangeState(IdleState);
        }

        private void OnDestroy()
        {
            FoxlairEventManager.Instance.WeaponSystem_OnEquippedWeaponDestroyed_Event -= OnWeaponDestroyedGoToIdleState;
        }
    }
}