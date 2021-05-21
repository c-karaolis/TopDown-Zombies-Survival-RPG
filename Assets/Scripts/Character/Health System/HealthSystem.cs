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

        public float Armor;

        PlayerCharacter PlayerCharacter;

        public Image HealthBar;

        private void Start()
        {
            PlayerCharacter = GetComponent<PlayerCharacter>();
            MaxHealth = 50f;
            Health = MaxHealth;
            HealthRegeneration = 0.1f;
        }



        public void TakeDamage(float damage)
        {
            damage -= Armor;

            if (Health - damage <= 0)
            {
                Health = 0;
                FoxlairEventManager.Instance.HealthSystem_OnHealthLost_Event?.Invoke(damage);
                Die();
            }
            else
            {
                Health -= damage;
                FoxlairEventManager.Instance.HealthSystem_OnHealthLost_Event?.Invoke(damage);
            }
        }

        public void Heal(float healAmount)
        {
            if (Health + healAmount <= MaxHealth)
            {
                Health += healAmount;
                FoxlairEventManager.Instance.HealthSystem_OnHealthChanged_Event?.Invoke();
            }
            else
            {
                Health = MaxHealth;
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
            HealthBar.fillAmount = Health / MaxHealth;
        }

        private void RegenerateHealth()
        {
            if (Health == MaxHealth)
            {
                return;
            }
            else if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += HealthRegeneration * Time.deltaTime;
            }

            FoxlairEventManager.Instance.HealthSystem_OnHealthChanged_Event?.Invoke();

        }
    }
}