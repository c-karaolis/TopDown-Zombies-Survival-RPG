using Foxlair.Character;
using Foxlair.Enemies.HealthSystem;
using Foxlair.Enemies.Movement;
using Foxlair.Enemies.States;
using Foxlair.Enemies.Targeting;
using UnityEngine;

namespace Foxlair.Enemies
{
    public class EnemyCharacter : MonoBehaviour
    {
        public EnemyHealthSystem healthSystem;
        public EnemyCharacterTargetingHandler enemyCharacterTargetingHandler;
        public EnemyCharacterMovement enemyCharacterMovement;
        public PlayerCharacter playerTarget;
        public Animator animator;

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
            if (Vector3.Distance(playerTarget.transform.position, transform.position) <= enemyAttack.attackRange)
            {
                Debug.Log("enemy in range to attack");
                return true;
            }
            else
            {
                return false;
            }
        }
        public void PlayerSpotted(PlayerCharacter _playerTarget)
        {
            if(_playerTarget == playerTarget) { return; }

            Debug.Log("PLAYER SPOTTED");
            playerTarget = _playerTarget;
            //need to change to chasing/attacking state and use enemycharactermovement moveto..
            enemyStateMachine.ChangeState(enemyStateMachine.enemyMovingToAttackState);
        }

        public void PlayerLost()
        {
            playerTarget = null;
            enemyStateMachine.ChangeState(enemyStateMachine.enemyIdleState);
        }

        public virtual void Damage(float weaponDamage)
        {
            Debug.Log($"{this} was hit for {weaponDamage} damage.");
           if ( (healthSystem.health -= weaponDamage) <= 0 )
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Destroy(this.gameObject);
        }

    }
}