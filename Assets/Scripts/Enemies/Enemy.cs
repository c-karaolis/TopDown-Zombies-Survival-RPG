using UnityEngine;

namespace Foxlair.Enemies
{
    public class Enemy : MonoBehaviour
    {
        private int _health;


        void Start()
        {

        }

        void Update()
        {

        }

        public virtual void Damage(float weaponDamage)
        {
            Debug.Log($"{this} was hit for {weaponDamage} damage.");
        }

        public virtual void Die()
        {

        }

    }
}