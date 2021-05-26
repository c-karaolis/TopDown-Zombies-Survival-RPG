using Foxlair.Character;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.PlayerInput;
using Foxlair.Tools.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foxlair.Weapons
{
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        //protected WeaponRarity weaponRarity;
        public float weaponDamage;
        public float armorPenetration;

        public int durability = 10;
        protected int durabilityLossPerShot = 1;

        public bool isCoolingDown;

        public AudioSource weaponAudioSource;
        public AudioClip attackSoundEffect;

        public EquippableItem InventoryItemInstance;
        public float weaponRange = 50f;
        public float nextFire;
        public float weaponAttackDuration = 0.07f;


        public PlayerCharacter playerCharacter;
        public InputHandler input;
        [Range(1, 25)]
        public int fireRate;

        //public float FireDelay {get => 1/fireRate;}
        //1/(x / (1/x) * (1/60))
        public float FireDelay { get => fireRateToDelayValues[fireRate]; }

        private Dictionary<int, float> fireRateToDelayValues = new Dictionary<int, float>
        {
            {1,2.90f},
            {2,2.81f},
            {3,2.71f},
            {4,2.61f},
            {5,2f},
            {6,1.74f},
            {7,1.55f},
            {8,1.25f},
            {9,1.16f},
            {10,1.06f},
            {11,0.96f},
            {12,0.87f},
            {13,0.77f},
            {14,0.67f},
            {15,0.58f},
            {16,0.48f},
            {17,0.38f},
            {18,0.28f},
            {19,0.19f},
            {20,0.14f},
            {21,0.12f},
            {22,0.09f},
            {23,0.06f},
            {24,0.03f},
            {25,0.02f},
            {26,0.01f},
        };

        public WeaponType WeaponType;

        public virtual void Start()
        {
            input = InputHandler.Instance;
            weaponAudioSource = GetComponent<AudioSource>();
           playerCharacter.SetAnimatorBasedOnWeaponType((int)WeaponType);
        }

       

        public virtual void Update()
        {
            isCoolingDown = !(Time.time > nextFire);
        }

        public void DetermineAttack()
        {
            if (!isCoolingDown)
            {
                Attack();
            }
        }

        public bool InRangeToAttack()
        {
            if (Vector3.Distance(playerCharacter.PlayerTargetEnemy.transform.position, playerCharacter.transform.position) <= weaponRange)
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

            //if (playerCharacter.PlayerTargetEnemy == null) { return; }

            if (!playerCharacter.PlayerAnimator.GetBool("ATTACKING"))
            {
                playerCharacter.PlayerAnimator.SetBool("ATTACKING", true);
            }
            //Debug.Log("parent weapon");

            //TODO: this is fire delay not fire rate. 
            //find a way to normalise firerate for humans. e.g. thisfirerate = humanfirerate * (1/100)
            nextFire = Time.time + FireDelay;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(AttackEffect());
            if (!(playerCharacter.PlayerTargetEnemy == null))
            {
                playerCharacter.PlayerTargetEnemy.Damage(weaponDamage);
            }
            else
            {
                //Debug.Log("Shooting in the air. Wasting your weapon I get more money $$$$");
            }

            HandleWeaponDurability();

        }

        public virtual IEnumerator AttackEffect()
        {
            // Play the shooting sound effect
            weaponAudioSource.PlayOneShot(attackSoundEffect);
            //Wait for .07 seconds
            yield return new WaitForSeconds(weaponAttackDuration);
            playerCharacter.PlayerAnimator.SetBool("ATTACKING", false);
            playerCharacter.isExecutingAnAttackMove = false;
        }

        public virtual void HandleWeaponDurability()
        {

            if ((durability -= durabilityLossPerShot) <= 0)
            {
                Debug.Log("Durability Depleted");
                DestroyWeapon();
                return;
            }
            InventoryItemInstance.durability = durability;

        }

        public  virtual void DestroyWeapon()
        {
            BaseItemSlot slotToRemove = playerCharacter.InventoryController.GetEquipmentSlotByType(InventoryItemInstance.EquipmentType);
            if (slotToRemove != null)
            {

                playerCharacter.InventoryController.DestroyItemInSlot(slotToRemove);
                FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();
                //TODO: when this gets invoked find where to subscribe to change to idle state so if fire is hold enemy not punched from long range because moving to attack is bypassed already
                FoxlairEventManager.Instance.WeaponSystem_OnEquippedWeaponDestroyed_Event?.Invoke();

            }
            //Destroy(this.gameObject, 0.3f);

        }


    }
    public enum WeaponType
    {
        BASE,
        PISTOL,
        RIFLE,
        ONE_HAND_MELEE,
        TWO_HAND_MELEE,
        
    }
}