using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using UnityEngine;


namespace Foxlair.Weapons
{
    public class DirectHitWeapon : RangedWeapon
    {

        public override void Attack()
        {
            Debug.Log($"Durability: {durability} , Loss per Shot: {durabilityLossPerShot}");
            nextFire = Time.time + FireDelay;
            //PlayerManager.Instance.PlayerTargetEnemy = _characterTargetingHandler.EnemyTarget;

            if (!(playerCharacter.PlayerTargetEnemy == null))
            {
                playerCharacter.PlayerTargetEnemy.Damage(weaponDamage);
            }
            else
            {
                //Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

            HandleWeaponDurability();

            GameObject muzzleFlash = Instantiate(muzzleFlashEffect, gunBarrelEnd);
            Destroy(muzzleFlash, weaponAttackDuration);
            weaponAudioSource.PlayOneShot(attackSoundEffect);

        }
    }
}