using Foxlair.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Weapons
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack()
        {

            //if (playerCharacter.PlayerTargetEnemy == null) { return; }


            playerCharacter.playerAnimator.SetBool("ATTACKING", true);

            //Debug.Log("parent weapon");

            nextFire = Time.time + FireDelay;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(AttackEffect());
            if (!(playerCharacter.Target == null))
            {
                (playerCharacter.Target as EnemyCharacter).healthSystem.TakeDamage(weaponDamage, playerCharacter);
            }
            else
            {
                //Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

            HandleWeaponDurability();

        }

        public override IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            weaponAudioSource.PlayOneShot(attackSoundEffect);
            //Wait for .07 seconds
            yield return new WaitForSeconds(weaponAttackDuration);
            playerCharacter.playerAnimator.SetBool("ATTACKING", false);
            playerCharacter.isExecutingAnAttackMove = false;

            playerCharacter.playerStateMachine.ChangeState(playerCharacter.playerStateMachine.IdleState);
        }
    }
}