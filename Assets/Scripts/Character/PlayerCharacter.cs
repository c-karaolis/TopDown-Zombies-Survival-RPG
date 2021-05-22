using Foxlair.Character.Movement;
using Foxlair.Character.Targeting;
using Foxlair.Character.Health;
using Foxlair.CharacterStats;
using Foxlair.Tools.Events;
using Foxlair.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Character
{
    public class PlayerCharacter : Actor
    {
        #region Fields & Properties
        [Header("Player Attributes")]
        public CharacterAttribute Strength;
        public CharacterAttribute Agility;
        public CharacterAttribute Intelligence;
        public CharacterAttribute Vitality;
        public CharacterAttribute Perception;
        public CharacterAttribute Luck;
        public CharacterAttribute Charisma;

        [Header("Character Systems")]
        public CharacterMovement CharacterMovement;
        public InventoryController InventoryController;
        public CharacterTargetingHandler CharacterTargetingHandler;
        public HealthSystem HealthSystem;
        private Weapon PlayerEquippedWeapon;

        [Header("Weapon Related")]
        public GameObject PunchDefaultWeaponPrefabInstance;
        public PunchDefaultWeapon PunchDefaultWeapon;
        public GameObject weaponEquipPoint;

        [Header("Will be set through code")]
        public Inventory Inventory;
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
        }

        private void UnsetEquippedWeapon(Weapon weapon) 
        {
            Debug.Log($"Unequipping {weapon}");

            PunchDefaultWeaponPrefabInstance.SetActive(true);
            PlayerEquippedWeapon = PunchDefaultWeapon;
        }

        public bool InRangeToHarvest()
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerTargetResourceNode.transform.position, transform.position) <= 2)
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

        private void OnDestroy()
        {
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event -= SetEquippedWeapon;
            FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event -= UnsetEquippedWeapon;
        }
    }
}