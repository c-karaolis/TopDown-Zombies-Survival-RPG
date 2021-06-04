using Foxlair.Character;
using Foxlair.Enemies.Health;
using Foxlair.Enemies.Movement;
using Foxlair.Enemies.States;
using Foxlair.Enemies.Targeting;
using Foxlair.Tools.Events;
using System.Collections;
using UnityEngine;

namespace Foxlair.Enemies
{
    public class EnemyCharacter : Actor
    {
        public EnemyHealthSystem healthSystem;
        public EnemyCharacterTargetingHandler enemyCharacterTargetingHandler;
        public EnemyCharacterMovement enemyCharacterMovement;
        public Animator animator;

        public Vector3 lastKnownPlayerPosition;

        public EnemyAttack enemyAttack;

        [Header("Need to be set manually")]
        public EnemyStateMachine enemyStateMachine;

        public string enemyName;

        void Awake()
        {
            enemyCharacterTargetingHandler = gameObject.GetComponent<EnemyCharacterTargetingHandler>();
            healthSystem = gameObject.GetComponent<EnemyHealthSystem>();
            enemyCharacterMovement = gameObject.GetComponent<EnemyCharacterMovement>();
            animator = gameObject.GetComponent<Animator>();
        }

        void Update()
        {

        }

        public bool InRangeToAttack()
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= enemyAttack.attackRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void PlayerSpotted(PlayerCharacter _playerTarget)
        {
            if (_playerTarget == target) { return; }

            target = _playerTarget;
            //need to change to chasing/attacking state and use enemycharactermovement moveto..
            enemyStateMachine.ChangeState(enemyStateMachine.enemyMovingToAttackState);
        }

        public void PlayerLost(PlayerCharacter _playerCharacter)
        {

            target = null;
            if (!(_playerCharacter == null))
            {
                lastKnownPlayerPosition = _playerCharacter.transform.position;

                enemyStateMachine.ChangeState(enemyStateMachine.enemyMovingToLastPlayerLocationState);

            }
            //enemyStateMachine.ChangeState(enemyStateMachine.enemyIdleState);
        }

        public override void OnActorHealthLost(float damage)
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnHealthLost_Event?.Invoke(damage);
        }

        public override void OnActorHealthGained(float healAmount)
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnHealthGained_Event?.Invoke(healAmount);
        }

        public override void OnActorDeath()
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnDeath_Event?.Invoke();
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}