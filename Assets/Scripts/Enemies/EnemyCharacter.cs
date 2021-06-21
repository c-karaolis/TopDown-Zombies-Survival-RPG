using Foxlair.Character;
using Foxlair.Enemies.Health;
using Foxlair.Enemies.Movement;
using Foxlair.Enemies.States;
using Foxlair.Enemies.Targeting;
using Foxlair.Tools.Events;
using System;
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

        public bool isAngry = false;
        public float secondsToCalm;

        public override Actor Target
        {
            get { return target; }
            set
            {
                if (isAngry)
                {
                    target = lastAttacker;
                }
                if (!isAngry)
                {
                    
                    target = value;
                }
            }
        }

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
            if (Vector3.Distance(Target.transform.position, transform.position) <= enemyAttack.attackRange)
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
            if (_playerTarget == Target || _playerTarget == null  ) { return; }

            Target = _playerTarget;
            enemyStateMachine.ChangeState(enemyStateMachine.enemyMovingToAttackState);
        }

        public void PlayerLost(PlayerCharacter _playerCharacter)
        {
            Target = null;
            if (!(_playerCharacter == null))
            {
                lastKnownPlayerPosition = _playerCharacter.transform.position;

                enemyStateMachine.ChangeState(enemyStateMachine.enemyMovingToLastPlayerLocationState);

            }
            //enemyStateMachine.ChangeState(enemyStateMachine.enemyIdleState);
        }

        public override void OnActorHealthLost(float damage)
        {
            StartCoroutine(AggroAfterGettingShot());
            FoxlairEventManager.Instance.EnemyHealthSystem_OnHealthLost_Event?.Invoke(damage);
        }

        IEnumerator AggroAfterGettingShot()
        {
            isAngry = true;
            PlayerSpotted((PlayerCharacter)lastAttacker);
            yield return new WaitForSeconds(secondsToCalm);
            isAngry = false;
        }

        public override void OnActorHealthGained(float healAmount)
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnHealthGained_Event?.Invoke(healAmount);
        }

        public override void OnActorDeath()
        {
            FoxlairEventManager.Instance.EnemyHealthSystem_OnDeath_Event?.Invoke(this);
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}