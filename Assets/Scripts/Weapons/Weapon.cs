using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.PlayerInput;
using Opsive.UltimateInventorySystem.Core;
using Opsive.UltimateInventorySystem.Core.DataStructures;
using System;
using System.Collections;
using UnityEngine;


namespace Foxlair.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        //protected WeaponRarity weaponRarity;
        public float weaponDamage;
        public float armorPenetration;

        protected float durability = 10f;
        protected float durabilityLossPerShot = 1f;

        public bool isCoolingDown;

        public AudioSource weaponAudioSource;
        public AudioClip attackSoundEffect;

        public float fireRate = 0.25f;
        public float weaponRange = 50f;
        public float nextFire;
        public float weaponAttackDuration = 0.07f;


        public PlayerCharacter playerCharacter;
        public CharacterTargetingHandler characterTargetingHandler;
        public InputHandler input;


        public virtual void Start()
        {
            input = InputHandler.Instance;
            characterTargetingHandler = PlayerManager.Instance.MainPlayerCharacterTargetingHandler;
            playerCharacter = PlayerManager.Instance.MainPlayerCharacter;
            weaponAudioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            isCoolingDown = !(Time.time > nextFire);

            //Debug.DrawRay(weaponEnd.position, weaponEnd.forward, Color.yellow);
        }

        public void DetermineAttack()
        {
            if ( !isCoolingDown)
            {
                Attack();
            }
        }

        public bool InRangeToAttack()
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerTargetEnemy.transform.position, PlayerManager.Instance.MainPlayerCharacter.transform.position) <= weaponRange)
            {
                Debug.Log("weapon in range to attack");
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void Attack()
        {
            if (PlayerManager.Instance.PlayerTargetEnemy == null) { return; }

            Debug.Log("parent weapon");

            //TODO: this is fire delay not fire rate. 
            //find a way to normalise firerate for humans. e.g. thisfirerate = humanfirerate * (1/100)
            nextFire = Time.time + fireRate;
            // Start our ShotEffect coroutine to turn our laser line on and off
            //StartCoroutine(AttackEffect());
            PlayerManager.Instance.PlayerTargetEnemy.Damage(weaponDamage);

            durability -= durabilityLossPerShot;

        }

        public virtual IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            weaponAudioSource.Play();
            //Wait for .07 seconds
            yield return new WaitForSeconds(weaponAttackDuration);
        }

        public virtual void HandleWeaponDurability()
        {
            if ((durability -= durabilityLossPerShot) <= 0)
            {
                Debug.Log("Durability Depleted");
                DestroyWeapon();
            }
        }

        private void DestroyWeapon()
        {
            ItemInfo equippedItemInfo = GetComponent<ItemObject>().ItemInfo;
            playerCharacter.Inventory.RemoveItem(equippedItemInfo);

            Destroy(this.gameObject, 0.3f);
        }
    }
}