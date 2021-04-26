using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using UnityEngine;


namespace Foxlair.Weapons
{
    public class DirectHitWeapon : Weapon
    {
        //CharacterTargetingHandler _characterTargetingHandler;
        //Enemy weaponEnemyTarget;

        //// Start is called before the first frame update
        //public override void Awake()
        //{

        //}

        //// Update is called once per frame
        //public override void Update()
        //{
        //    //weaponEnemyTarget = _characterTargetingHandler.EnemyTarget;
        //}




        public override void Attack()
        {
            Debug.Log($"Durability: {_durability} , Loss per Shot: {_durabilityLossPerShot}");
            _nextFire = Time.time + _fireRate;
            //PlayerManager.Instance.PlayerTargetEnemy = _characterTargetingHandler.EnemyTarget;

            if (!(PlayerManager.Instance.PlayerTargetEnemy == null))
            {
                PlayerManager.Instance.PlayerTargetEnemy.Damage(_weaponDamage);
            }
            else
            {
                Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

            HandleWeaponDurability();

        }

        //private void HandleWeaponDurability()
        //{
        //    if ((_durability -= _durabilityLossPerShot) <= 0)
        //    {
        //        Debug.Log("Durability Depleted");
        //        Destroy(this, 0.3f);
        //    }
        //}
    }
}