using Foxlair.Tools.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Enemies.States { 

    public class EnemyMovingToLastPlayerLocationState : State
    {
        public EnemyStateMachine enemyStateMachine;
        EnemyCharacter enemyCharacter;

        public override void OnStateEnter()
        {
            enemyCharacter = enemyStateMachine.enemyCharacter;
            //enemyStateMachine.enemyCharacter.enemyCharacterMovement.StopMoving();

            if (!enemyStateMachine.enemyCharacter.animator.GetBool("RUNNING"))
            {
                enemyStateMachine.enemyCharacter.animator.SetBool("RUNNING", true);
            }            //zombieStateMachine.enemyCharacter.PlayerAnimator.SetTrigger("IDLE");

            enemyCharacter.enemyCharacterMovement.MoveTo(enemyCharacter.lastKnownPlayerPosition);
            //TODO: check if below is better performance or worse.
            //enemyStateMachine.enemyCharacter.enemyCharacterMovement.MoveTo(enemyStateMachine.enemyCharacter.lastKnownPlayerPosition);


        }

        public override void OnStateExecute()
        {
            if (enemyStateMachine.enemyCharacter.Target != null)
            {
                ChangeState(enemyStateMachine.enemyMovingToAttackState);
            }

            if(Vector3.Distance(enemyCharacter.transform.position, enemyCharacter.lastKnownPlayerPosition) < 1)
            {
                ChangeState(enemyStateMachine.enemyIdleState);
            }

        }

        public override void OnStatePhysicsExecute() { }

        public override void OnStatePostExecute() { }

        public override void OnStateExit()
        {
            enemyStateMachine.enemyCharacter.animator.SetBool("RUNNING", false);
        }


    }
}