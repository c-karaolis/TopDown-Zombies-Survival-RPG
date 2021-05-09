using Foxlair.Character.Movement;
using Foxlair.CharacterStats;
using Foxlair.Weapons;
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

        public InventoryController InventoryController;

        //public CharacterAttribute[] CharacterAttributes;
        public Dictionary<AttributeType, CharacterAttribute> CharacterAttributes;
        private void Start()
        {
            //CharacterAttributes = new CharacterAttribute[] { Strength, Agility, Intelligence, Vitality, Perception, Luck, Charisma };
            InitializeAttributesDictionary();

            PlayerManager.Instance.MainPlayerCharacter = this;
            PlayerManager.Instance.MainPlayerCharacterMovement = GetComponent<CharacterMovement>();
           // PlayerManager.Instance.MainPlayerCharacterInventory = GetComponent<Inventory>();
        }
            
        public Weapon PlayerWeapon => GetComponentInChildren<Weapon>();
        //public Inventory Inventory => GetComponent<Inventory>();

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