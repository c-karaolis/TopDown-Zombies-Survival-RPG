using Foxlair.Character.Movement;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class MovingToAttackState : State
    {

        public CharacterMovement characterMovement;


        private void Start()
        {
            
        }
        public override void OnStateEnter() {
            PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
            ForbiddenTransitions.Add(playerStateMachine.AttackingState);
        }

        public override void OnStateExecute()
        {
            if (!PlayerManager.Instance.PlayerEquippedWeapon.InRangeToAttack())
            {
                characterMovement.HandleAutoMoveToAttack(PlayerManager.Instance.PlayerTargetEnemy.transform);
            }
            else
            {
                PlayerStateMachine playerStateMachine = StateMachine as PlayerStateMachine;
                ChangeState(playerStateMachine.AttackingState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}