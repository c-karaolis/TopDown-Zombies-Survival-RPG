using Foxlair.Character;
using Foxlair.Character.Movement;
using Foxlair.Character.States;
using Foxlair.Character.Targeting;
using Foxlair.Enemies;
using Foxlair.Harvesting;
using Foxlair.Tools;
using Foxlair.Tools.StateMachine;
using Foxlair.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Character
{

    public class PlayerManager : PersistentSingletonMonoBehaviour<PlayerManager>
    {
        public GameObject punchWeapon;


        [System.NonSerialized]
        public Enemy PlayerTargetEnemy;
        [System.NonSerialized]
        public ResourceNode PlayerTargetResourceNode;
        [System.NonSerialized]
        public PlayerCharacter MainPlayerCharacter;
        [System.NonSerialized]
        public CharacterMovement MainPlayerCharacterMovement;
        [System.NonSerialized]
        public CharacterTargetingHandler MainPlayerCharacterTargetingHandler;
        [System.NonSerialized]
       //public Inventory MainPlayerCharacterInventory;

        private Weapon playerEquippedWeapon;

        [Header("TESTING ITEMS")]
        public List<string> testingItems;


        public Weapon PlayerEquippedWeapon
        {
            get
            {
                if (MainPlayerCharacter.PlayerWeapon != null)
                {
                    Debug.Log($"found equipped weapon: {MainPlayerCharacter.PlayerWeapon.name}");
                    return MainPlayerCharacter.PlayerWeapon;
                }
                else return punchWeapon.GetComponent<Weapon>();
            }

            set
            {
                playerEquippedWeapon = value;
            }
        }


    }
}