using Foxlair.Tools.StateMachine;

namespace Foxlair.Enemies.States
{
    public class EnemyStateMachine : StateMachine
    {
        public EnemyIdleState enemyIdleState;
        public EnemyRunningState enemyRunningState;
        public EnemyMovingToAttackState enemyMovingToAttackState;
        public EnemyAttackingState enemyAttackingState;

        public EnemyCharacter enemyCharacter;

    }


}