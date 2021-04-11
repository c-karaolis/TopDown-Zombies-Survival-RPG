using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;

namespace Foxlair.Character.States
{
    public class PlayerStateMachine : StateMachine
    {
       public InputHandler _inputHandler;

        public IdleState idleState;
        public RunningState runningState;
        public MovingToAttackState movingToAttackState;
        public MovingToHarvestState MovingToHarvestState;
        public AttackingState AttackingState;
        public HarvestingState HarvestingState;
        
        public override void Start()
        {
            _inputHandler = InputHandler.Instance;

            base.Start();

        }
    }
}