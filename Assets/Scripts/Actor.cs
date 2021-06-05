using UnityEngine;

namespace Foxlair
{
    public abstract class Actor : MonoBehaviour
    {
        public string faction;
        public Actor target;
        public abstract Actor Target { get; set; }
        public Actor lastAttacker;
        /*
         * name
         * faction
         * 
        */

        public abstract void OnActorHealthLost(float damage);

        public abstract void OnActorHealthGained(float healAmount);

        public abstract void OnActorDeath();

        public abstract void Die();
    }
}