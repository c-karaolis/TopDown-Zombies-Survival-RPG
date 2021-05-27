using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using System.Collections;
using UnityEngine;


namespace Foxlair.Weapons
{
    public class DirectHitWeapon : RangedWeapon
    {

        public override void Attack()
        {
            if (!playerCharacter.PlayerAnimator.GetBool("ATTACKING"))
            {
                playerCharacter.PlayerAnimator.SetBool("ATTACKING", true);
            }
           // Debug.Log($"Durability: {durability} , Loss per Shot: {durabilityLossPerShot}");
            nextFire = Time.time + FireDelay;
            //PlayerManager.Instance.PlayerTargetEnemy = _characterTargetingHandler.EnemyTarget;
            StartCoroutine(AttackEffect());
            if (!(playerCharacter.PlayerTargetEnemy == null))
            {
                playerCharacter.PlayerTargetEnemy.Damage(weaponDamage);
            }
            else
            {
               // Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

            HandleWeaponDurability();

            //weaponAudioSource.PlayOneShot(attackSoundEffect);

        }

        public override IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            weaponAudioSource.PlayOneShot(attackSoundEffect);
            //Wait for .07 seconds
            yield return new WaitForSeconds(weaponAttackDuration);

            GameObject muzzleFlash = Instantiate(muzzleFlashEffect, gunBarrelEnd);
            Destroy(muzzleFlash, weaponAttackDuration);

            playerCharacter.PlayerAnimator.SetBool("ATTACKING", false);
            playerCharacter.isExecutingAnAttackMove = false;
        }
    }
}