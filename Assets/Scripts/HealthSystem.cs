using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Foxlair
{
    public class HealthSystem : MonoBehaviour
    {

        public float health;
        public float maxHealth;
        public float healthRegeneration;

        public float armor;

        Actor actor;

        public Image healthBar;

        private void Start()
        {
            actor = GetComponent<Actor>();
            maxHealth = 50f;
            health = maxHealth;
            healthRegeneration = 0.1f;
        }



        public void TakeDamage(float damage)
        {
            damage -= armor;

            if (health - damage <= 0)
            {
                health = 0;

                actor.OnActorHealthLost(damage);
                Die();
            }
            else
            {
                health -= damage;
                actor.OnActorHealthLost(damage);
            }
        }


        public void TakeDamage(float damage, Actor _actor)
        {

            if(armor>0)
            damage -= armor;

            if (health - damage <= 0)
            {
                health = 0;

                actor.OnActorHealthLost(damage);
                Die();
            }
            else
            {
                health -= damage;
                actor.OnActorHealthLost(damage);
                actor.lastAttacker = _actor;
            }

        }

        public void Heal(float healAmount)
        {
            if (health + healAmount <= maxHealth)
            {
                health += healAmount;
                actor.OnActorHealthGained(healAmount);
            }
            else
            {
                health = maxHealth;
                actor.OnActorHealthGained(healAmount);
            }
        }

        protected void Die()
        {
            actor.OnActorDeath();
            actor.Die();
            //Debug.Log($"Actor({actor.name}) Died");
            //Destroy(gameObject);
        }

        protected void Update()
        {
            RegenerateHealth();
            HandleHealthBar();
        }

        private void HandleHealthBar()
        {
            if(healthBar == null) { return; }

            healthBar.fillAmount = health / maxHealth;
        }

        protected void RegenerateHealth()
        {
            if (healthRegeneration! > 0)
                return;

            float regeneratedAmount;

            if (health == maxHealth)
            {
                return;
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
                return;
            }
            else
            {
                regeneratedAmount = healthRegeneration;
                health += healthRegeneration * Time.deltaTime;
            }

            actor.OnActorHealthGained(regeneratedAmount);

        }
    }
}