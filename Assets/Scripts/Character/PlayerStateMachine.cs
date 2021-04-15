using Foxlair.PlayerInput;
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

        //Note: Can add on state enter an enum representation for each state as well if we need it in the codebase somewhere else.

        public override void Start()
        {
            base.Start();
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
            if (InputHandler.Instance.IsInteractionButtonDown && !(CurrentState.ForbiddenTransitions.Contains(HarvestingState)))
            {
                ChangeState(HarvestingState);
            }
        }

        private void AttackingStateTransition()
        {
          //  if (InputHandler.Instance.IsFiringButtonDown && !(CurrentState.ForbiddenTransitions.Contains(AttackingState)) && CurrentState != MovingToAttackState )
            if (InputHandler.Instance.IsFiringButtonDown && !(CurrentState.ForbiddenTransitions.Contains(AttackingState)) )
            {
                ChangeState(AttackingState);
            }
        }
    }
}