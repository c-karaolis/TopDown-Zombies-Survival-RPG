using Foxlair.CharacterStats;
using Foxlair.Tools.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Foxlair.Character.HealthSystem
{
    public class HealthSystem : MonoBehaviour
    {
        public float Health;
        public float MaxHealth;
        public float HealthRegeneration;

        public Image HealthBar;

        private void Start()
        {
            MaxHealth = 50f;
            Health = MaxHealth;
            HealthRegeneration = 0.1f;
        }



        public void TakeDamage(float damage)
        {
            if (Health - damage <= 0)
            {
                Health = 0;
                Die();
            }
            else
            {
                Health -= damage;
            }
        }

        public void Heal(float healAmount)
        {
            if (Health + healAmount <= MaxHealth)
            {
                Health += healAmount;
            }
            else
            {
                Health = MaxHealth;
            }
        }

        public void Die()
        {
            Debug.LogWarning("Player Died");
        }

        private void Update()
        {
            RegenerateHealth();

            if (HealthBar == null) return;
            HealthBar.fillAmount = Health / MaxHealth;
        }

        private void RegenerateHealth()
        {
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += HealthRegeneration * Time.deltaTime;
            }
        }
    }
}