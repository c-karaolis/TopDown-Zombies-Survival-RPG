using Foxlair.PlayerInput;
using Foxlair.Tools.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Enemies.States
{
    public class EnemyIdleState : State
    {
        public EnemyStateMachine enemyStateMachine;

        public override void OnStateEnter()
        {
            enemyStateMachine.enemyCharacter.enemyCharacterMovement.StopMoving();

            if (!enemyStateMachine.enemyCharacter.animator.GetBool("IDLE"))
            {
                enemyStateMachine.enemyCharacter.animator.SetBool("IDLE", true);
            }            //zombieStateMachine.enemyCharacter.PlayerAnimator.SetTrigger("IDLE");
        }

        public override void OnStateExecute()
        {
            if (enemyStateMachine.enemyCharacter.playerTarget != null)
            {
                ChangeState(enemyStateMachine.enemyMovingToAttackState);
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit()
        {
            enemyStateMachine.enemyCharacter.animator.SetBool("IDLE", false);
        }



    }
}