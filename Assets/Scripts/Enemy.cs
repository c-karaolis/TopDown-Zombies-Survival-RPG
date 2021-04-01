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

        internal void Damage(int gunDamage)
        {
            Debug.Log($"Was hit for {gunDamage} damage.");
        }
    }
}