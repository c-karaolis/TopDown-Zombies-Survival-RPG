using Foxlair.Character.Movement;
using Foxlair.Character.Targeting;
using Foxlair.Character.Health;
using Foxlair.CharacterStats;
using Foxlair.Tools.Events;
using Foxlair.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;
using Foxlair.Harvesting;
using Foxlair.Enemies;
using Foxlair.Character.States;

namespace Foxlair.Character
{
    public class PlayerCharacter : Actor
    {
        #region Fields & Properties
        private Weapon PlayerEquippedWeapon;

        [Header("Player Attributes")]
        public CharacterAttribute Strength;
        public CharacterAttribute Agility;
        public CharacterAttribute Intelligence;
        public CharacterAttribute Vitality;
        public CharacterAttribute Perception;
        public CharacterAttribute Luck;
        public CharacterAttribute Charisma;

        [Header("Character Systems")]
        public Animator playerAnimator;
        public CharacterMovement characterMovement;
        public InventoryController inventoryController;
        public PlayerCharacterTargetingHandler characterTargetingHandler;
        public PlayerHealthSystem healthSystem;
        public PlayerStateMachine playerStateMachine;

        [Header("Weapon Related")]
        public GameObject punchDefaultWeaponPrefabInstance;
        public PunchDefaultWeapon punchDefaultWeapon;
        public GameObject weaponEquipPoint;
        public bool isExecutingAnAttackMove = false;

        [Header("Will be set through code")]
        public Inventory inventory;
        public IInteractable playerTargetInteractable;
        //public EnemyCharacter target;
        #endregion


        public Dictionary<AttributeType, CharacterAttribute> CharacterAttributes;

        public override Actor Target { get { return target; } set { target = value; } }

        private void Awake()
        {
            InitializeAttributesDictionary();
        }

        private void Start()
        {
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event += SetEquippedWeapon;
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event += UnsetEquippedWeapon;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event += SetResourceNode;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event += UnsetResourceNode;
            FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event += SetEnemyTarget;
            FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event += UnsetEnemyTarget;

            SceneStartWeaponSetup();
        }


        public Weapon GetPlayerWeapon()
        {
            return PlayerEquippedWeapon;
        }

        private bool HasEquippedWeapon(out Weapon currentlyEquippedWeapon)
        {
            if ((inventoryController.GetEquipmentSlotByType(EquipmentType.Weapon).Item as EquippableItem) != null)
            {
                (inventoryController.GetEquipmentSlotByType(EquipmentType.Weapon).Item as EquippableItem).InstanceOfPhysicalItemPrefab.TryGetComponent<Weapon>(out currentlyEquippedWeapon);
                return true;
            }
            currentlyEquippedWeapon = null;
            return false;
        }

        private void SetEquippedWeapon(Weapon weapon)
        {
            //Debug.Log($"Equipping {weapon}");
            weapon.playerCharacter = this;
            punchDefaultWeaponPrefabInstance.SetActive(false);
            PlayerEquippedWeapon = weapon;
            SetAnimatorBasedOnWeaponType((int)weapon.WeaponType);
        }

        private void UnsetEquippedWeapon(Weapon weapon)
        {
            //Debug.Log($"Unequipping {weapon}");
            punchDefaultWeaponPrefabInstance.SetActive(true);
            PlayerEquippedWeapon = punchDefaultWeapon;
            SetAnimatorBasedOnWeaponType((int)punchDefaultWeapon.WeaponType);
        }

        public bool InRangeToHarvest()
        {
            if (Vector3.Distance(playerTargetInteractable.ImplementingMonoBehaviour().transform.position, transform.position) <= 2)
            {
                //TODO: Rotate towards harvest resource and start harvesting
                Debug.Log("player in range to harvest");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetAnimatorBasedOnWeaponType(int index)
        {

            playerAnimator.SetLayerWeight(index, 1);

            for (int i = 0; i < playerAnimator.layerCount; i++)
            {
                if (i != index)
                    playerAnimator.SetLayerWeight(i, 0);
            }


        }

        private void SceneStartWeaponSetup()
        {
            if (HasEquippedWeapon(out Weapon _currentlyEquippedWeapon))
            {
                SetEquippedWeapon(_currentlyEquippedWeapon);
            }

            UnsetEquippedWeapon(_currentlyEquippedWeapon);
        }

        private void InitializeAttributesDictionary()
        {
            CharacterAttributes = new Dictionary<AttributeType, CharacterAttribute> {
                {AttributeType.Strength, Strength },
                {AttributeType.Agility, Agility },
                {AttributeType.Intelligence, Intelligence },
                {AttributeType.Vitality, Vitality },
                {AttributeType.Perception, Perception },
                {AttributeType.Luck, Luck },
                {AttributeType.Charisma, Charisma },
            };
        }
        private void UnsetResourceNode()
        {
            playerTargetInteractable = null;
        }

        private void SetResourceNode(IInteractable obj)
        {
            playerTargetInteractable = obj;
        }

        private void OnDestroy()
        {
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event -= SetEquippedWeapon;
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event -= UnsetEquippedWeapon;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event -= SetResourceNode;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event -= UnsetResourceNode;
            FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event -= SetEnemyTarget;
            FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event -= UnsetEnemyTarget;
        }

        private void UnsetEnemyTarget()
        {
            Target = null;
        }

        private void SetEnemyTarget(EnemyCharacter obj)
        {
            Target = obj;
        }

        private void OnMeleeWeaponHitAnimationEvent()
        {
            if (Target != null 
                && Target is EnemyCharacter 
                && PlayerEquippedWeapon.WeaponType != WeaponType.PISTOL 
                && PlayerEquippedWeapon.WeaponType != WeaponType.RIFLE)
            {
                (Target as EnemyCharacter).healthSystem.TakeDamage(PlayerEquippedWeapon.weaponDamage, this);
            }
        }

        public override void OnActorHealthLost(float damage)
        {
            FoxlairEventManager.Instance.PlayerHealthSystem_OnHealthLost_Event?.Invoke(damage);
        }

        public override void OnActorHealthGained(float healAmount)
        {
            FoxlairEventManager.Instance.PlayerHealthSystem_OnHealthGained_Event?.Invoke(healAmount);
        }

        public override void OnActorDeath()
        {
            FoxlairEventManager.Instance.PlayerHealthSystem_OnPlayerDeath_Event?.Invoke();
        }

        public override void Die()
        {
            Destroy(gameObject);
        }
    }
}