using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class PlayerStateMachine : StateMachine
    {
       public InputHandler _inputHandler;

        public IdleState idleState;
        public RunningState runningState;
        public MovingToAttackState movingToAttackState;
        public MovingToHarvestState movingToHarvestState;
        public AttackingState attackingState;
        public HarvestingState harvestingState;
        
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
            if (InputHandler.Instance.isFiringButtonDown && !(CurrentState.forbiddenTransitions.Contains(attackingState)) ) 
            {
                Debug.Log(CurrentState.forbiddenTransitions);
                ChangeState(attackingState);
            }
        }


    }
}