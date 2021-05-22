using Foxlair.Character.Movement;
using Foxlair.Character.Targeting;
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
        public float Health = 10f;

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
        private Weapon PlayerEquippedWeapon;

        [Header("Weapon Related")]
        public GameObject PunchDefaultWeaponPrefabInstance;
        public PunchDefaultWeapon PunchDefaultWeapon;
        public GameObject weaponEquipPoint;

        [Header("Will be set through code")]
        public Inventory Inventory;

        

        //public CharacterAttribute[] CharacterAttributes;
        public Dictionary<AttributeType, CharacterAttribute> CharacterAttributes;


        private void Awake()
        {
            InitializeAttributesDictionary();
        }

        private void Start()
        {
            PlayerManager.Instance.MainPlayerCharacter = this;

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
        //public Inventory Inventory => GetComponent<Inventory>();

        private void SetEquippedWeapon(Weapon weapon)
        {
            Debug.Log($"Equipping {weapon}");

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
                PunchDefaultWeaponPrefabInstance.SetActive(false);

                PlayerEquippedWeapon = _currentlyEquippedWeapon;
            }

            PunchDefaultWeaponPrefabInstance.SetActive(true);
            PlayerEquippedWeapon = PunchDefaultWeapon;
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



    }
}