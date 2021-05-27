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
        public Animator PlayerAnimator;
        public CharacterMovement CharacterMovement;
        public InventoryController InventoryController;
        public CharacterTargetingHandler CharacterTargetingHandler;
        public HealthSystem HealthSystem;
        public PlayerStateMachine PlayerStateMachine;

        [Header("Weapon Related")]
        public GameObject PunchDefaultWeaponPrefabInstance;
        public PunchDefaultWeapon PunchDefaultWeapon;
        public GameObject weaponEquipPoint;
        public bool isExecutingAnAttackMove = false;

        [Header("Will be set through code")]
        public Inventory Inventory;
        public IInteractable PlayerTargetInteractable;
        public EnemyCharacter PlayerTargetEnemy;
        #endregion


        public Dictionary<AttributeType, CharacterAttribute> CharacterAttributes;

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
            if ((InventoryController.GetEquipmentSlotByType(EquipmentType.Weapon).Item as EquippableItem) != null)
            {
                (InventoryController.GetEquipmentSlotByType(EquipmentType.Weapon).Item as EquippableItem).InstanceOfPhysicalItemPrefab.TryGetComponent<Weapon>(out currentlyEquippedWeapon);
                return true;
            }
            currentlyEquippedWeapon = null;
            return false;
        }

        private void SetEquippedWeapon(Weapon weapon)
        {
            Debug.Log($"Equipping {weapon}");
            weapon.playerCharacter = this;
            PunchDefaultWeaponPrefabInstance.SetActive(false);
            PlayerEquippedWeapon = weapon;
            SetAnimatorBasedOnWeaponType((int)weapon.WeaponType);
        }

        private void UnsetEquippedWeapon(Weapon weapon) 
        {
            Debug.Log($"Unequipping {weapon}");
            PunchDefaultWeaponPrefabInstance.SetActive(true);
            PlayerEquippedWeapon = PunchDefaultWeapon;
            SetAnimatorBasedOnWeaponType((int)PunchDefaultWeapon.WeaponType);
        }

        public bool InRangeToHarvest()
        {
            if (Vector3.Distance(PlayerTargetInteractable.ImplementingMonoBehaviour().transform.position, transform.position) <= 2)
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

            PlayerAnimator.SetLayerWeight(index, 1);

            for (int i = 0; i < PlayerAnimator.layerCount; i++)
            {
                if (i != index)
                    PlayerAnimator.SetLayerWeight(i, 0);
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
            PlayerTargetInteractable = null;
        }

        private void SetResourceNode(IInteractable obj)
        {
            PlayerTargetInteractable = obj;
        }

        private void OnDestroy()
        {
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event -= SetEquippedWeapon;
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event -= UnsetEquippedWeapon;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event -= SetResourceNode;
            FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event -= UnsetResourceNode;
        }

        private void UnsetEnemyTarget()
        {
            PlayerTargetEnemy = null;
        }

        private void SetEnemyTarget(EnemyCharacter obj)
        {
            PlayerTargetEnemy = obj;
        }
    }
}