using Foxlair.Tools.StateMachine;

namespace Foxlair.Enemies.States
{
    public class EnemyStateMachine : StateMachine
    {
        public EnemyIdleState enemyIdleState;
        public EnemyMovingToLastPlayerLocationState enemyMovingToLastPlayerLocationState;
        public EnemyMovingToAttackState enemyMovingToAttackState;
        public EnemyAttackingState enemyAttackingState;

        public EnemyCharacter enemyCharacter;

    }


}