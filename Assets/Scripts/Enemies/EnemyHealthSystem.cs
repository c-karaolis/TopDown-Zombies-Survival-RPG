using Foxlair.Tools.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Foxlair.Enemies.HealthSystem
{
    public class EnemyHealthSystem : MonoBehaviour
    {

        public float health;
        public float maxHealth;
        public float healthRegeneration;

        public float armor;

        EnemyCharacter enemyCharacter;

        public Image healthBar;

        private void Start()
        {
            enemyCharacter = GetComponent<EnemyCharacter>();
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
                FoxlairEventManager.Instance.HealthSystem_OnHealthLost_Event?.Invoke(damage);
                Die();
            }
            else
            {
                health -= damage;
                FoxlairEventManager.Instance.HealthSystem_OnHealthLost_Event?.Invoke(damage);
            }
        }

        public void Heal(float healAmount)
        {
            if (health + healAmount <= maxHealth)
            {
                health += healAmount;
                FoxlairEventManager.Instance.HealthSystem_OnHealthChanged_Event?.Invoke();
            }
            else
            {
                health = maxHealth;
                FoxlairEventManager.Instance.HealthSystem_OnHealthChanged_Event?.Invoke();
            }
        }

        public void Die()
        {
            FoxlairEventManager.Instance.HealthSystem_OnPlayerDeath_Event?.Invoke();
            Debug.LogWarning("Player Died");
        }

        private void Update()
        {
            RegenerateHealth();

           // HealthBar.fillAmount = Health / MaxHealth;
        }

        private void RegenerateHealth()
        {
            if (health == maxHealth)
            {
                return;
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += healthRegeneration * Time.deltaTime;
            }

            FoxlairEventManager.Instance.HealthSystem_OnHealthChanged_Event?.Invoke();

        }
    }
}