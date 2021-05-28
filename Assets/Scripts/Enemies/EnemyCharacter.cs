using Foxlair.Enemies.HealthSystem;
using UnityEngine;

namespace Foxlair.Enemies
{
    public class EnemyCharacter : MonoBehaviour
    {
        public float _health=30f;
        public EnemyHealthSystem healthSystem;
        public string enemyName;

        void Start()
        {
         
        }

        void Update()
        {

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