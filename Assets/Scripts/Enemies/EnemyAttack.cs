using Foxlair.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        public float damage;
        public float armorPenetration;

        public bool isCoolingDown;

        public AudioSource enemyAudioSource;
        public AudioClip attackSoundEffect;

        public float attackRange = 50f;
        public float nextFire;
        public float weaponAttackDuration = 0.07f;


        public EnemyCharacter enemyCharacter;
        [Range(1, 25)]
        public int attackRate;

        //public float FireDelay {get => 1/fireRate;}
        //1/(x / (1/x) * (1/60))
        public float AttackDelay { get => attackRateToDelayValues[attackRate]; }

        private Dictionary<int, float> attackRateToDelayValues = new Dictionary<int, float>
        {
            {1,2.90f},
            {2,2.81f},
            {3,2.71f},
            {4,2.61f},
            {5,2f},
            {6,1.74f},
            {7,1.55f},
            {8,1.25f},
            {9,1.16f},
            {10,1.06f},
            {11,0.96f},
            {12,0.87f},
            {13,0.77f},
            {14,0.67f},
            {15,0.58f},
            {16,0.48f},
            {17,0.38f},
            {18,0.28f},
            {19,0.19f},
            {20,0.14f},
            {21,0.12f},
            {22,0.09f},
            {23,0.06f},
            {24,0.03f},
            {25,0.02f},
            {26,0.01f},
        };


        public virtual void Start()
        {
            enemyAudioSource = GetComponent<AudioSource>();
        }


        public virtual void Update()
        {
            isCoolingDown = !(Time.time > nextFire);
        }

        public void DetermineAttack()
        {
            if (!isCoolingDown)
            {
                Attack();
            }
        }

        public virtual void Attack()
        {

            //if (playerCharacter.PlayerTargetEnemy == null) { return; }

            if (!enemyCharacter.animator.GetBool("ATTACKING"))
            {
                enemyCharacter.animator.SetBool("ATTACKING", true);
            }
            //Debug.Log("parent weapon");

            //TODO: this is fire delay not fire rate. 
            //find a way to normalise firerate for humans. e.g. thisfirerate = humanfirerate * (1/100)
            nextFire = Time.time + AttackDelay;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(AttackEffect());
            if (!(enemyCharacter.Target == null))
            {
                (enemyCharacter.Target as PlayerCharacter).healthSystem.TakeDamage(damage);
            }
            else
            {
                //Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

        }

        public virtual IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            enemyAudioSource.PlayOneShot(attackSoundEffect);
            //Wait for .07 seconds
            yield return new WaitForSeconds(weaponAttackDuration);
            enemyCharacter.animator.SetBool("ATTACKING", false);
        }

    }
}