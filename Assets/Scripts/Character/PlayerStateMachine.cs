using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class PlayerStateMachine : StateMachine
    {
        public InputHandler _inputHandler;

        public IdleState IdleState;
        public RunningState RunningState;
        public MovingToAttackState MovingToAttackState;
        public MovingToHarvestState MovingToHarvestState;
        public AttackingState AttackingState;
        public HarvestingState HarvestingState;

        //Note: Can add on state enter an enum representation for each state as well if we need it in the codebase somewhere else.

        public override void Start()
        {
            _inputHandler = InputHandler.Instance;
            base.Start();
        }

        public override void Update()
        {
            base.Update();
            HandleCommonStateTransitions();
        }

        void HandleCommonStateTransitions()
        {
            AttackingStateTransition();
        }

        private void AttackingStateTransition()
        {
            if (InputHandler.Instance.IsFiringButtonDown && !(CurrentState.ForbiddenTransitions.Contains(AttackingState)))
            {
                ChangeState(AttackingState);
            }
        }
    }
}