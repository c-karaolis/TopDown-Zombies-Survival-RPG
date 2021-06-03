using Foxlair.Tools.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Enemies.States
{
    public class EnemyAttackingState : State
    {

        public EnemyStateMachine enemyStateMachine;


        public override void OnStateEnter()
        {
            //enemyStateMachine.enemyCharacter.enemyCharacterMovement.MoveTo(enemyStateMachine.enemyCharacter.playerTarget.transform.position);
            //zombieStateMachine.enemyCharacter.PlayerAnimator.SetTrigger("IDLE");
            if (!enemyStateMachine.enemyCharacter.animator.GetBool("ATTACKING"))
            {
                enemyStateMachine.enemyCharacter.animator.SetBool("ATTACKING", true);
            }
        }

        public override void OnStateExecute()
        {
            if (enemyStateMachine.enemyCharacter.playerTarget == null)
            {
                ChangeState(enemyStateMachine.enemyIdleState);
            }
            else
            {
                if (!enemyStateMachine.enemyCharacter.InRangeToAttack())
                {
                    ChangeState(enemyStateMachine.enemyMovingToAttackState);
                }

                enemyStateMachine.enemyCharacter.enemyAttack.DetermineAttack();
            }
        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit()
        {
            enemyStateMachine.enemyCharacter.animator.SetBool("ATTACKING", false);
        }

    }
}