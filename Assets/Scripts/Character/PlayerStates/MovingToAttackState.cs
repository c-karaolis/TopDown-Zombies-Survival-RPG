using Foxlair.Character.Movement;
using Foxlair.Tools.StateMachine;
using UnityEngine;

namespace Foxlair.Character.States
{
    public class MovingToAttackState : State
    {
        public PlayerStateMachine playerStateMachine;

        public override void OnStateEnter() {
            ForbiddenTransitions.Add(playerStateMachine.AttackingState);
        }

        public override void OnStateExecute()
        {
            
            if (!playerStateMachine.PlayerCharacter.GetPlayerWeapon().InRangeToAttack())
            {
                playerStateMachine.PlayerCharacter.CharacterMovement.HandleAutoMoveToAttack(playerStateMachine.PlayerCharacter.PlayerTargetEnemy.transform);
            }
            else
            {
                ChangeState(playerStateMachine.AttackingState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }
    }
}