using Foxlair.Tools.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Enemies.States
{
    public class EnemyMovingToAttackState : State
    {

        public EnemyStateMachine enemyStateMachine;

        public override void OnStateEnter()
        {
            enemyStateMachine.enemyCharacter.enemyCharacterMovement.MoveTo(enemyStateMachine.enemyCharacter.playerTarget.transform.position);
            //zombieStateMachine.enemyCharacter.PlayerAnimator.SetTrigger("IDLE");
        }

        public override void OnStateExecute()
        {
            if (enemyStateMachine.enemyCharacter.playerTarget == null)
            {
                ChangeState(enemyStateMachine.enemyIdleState);
            }
            else
            {
                enemyStateMachine.enemyCharacter.enemyCharacterMovement.MoveTo(enemyStateMachine.enemyCharacter.playerTarget.transform.position);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit() { }

    }
}