using Foxlair.Character;
using Foxlair.Character.Movement;
using Foxlair.Character.States;
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
        [System.NonSerialized]
        public Weapon PlayerEquippedWeapon;
        [System.NonSerialized]
        public Enemy PlayerTargetEnemy;
        [System.NonSerialized]
        public ResourceNode PlayerTargetResourceNode;
        [System.NonSerialized]
        public PlayerCharacter MainPlayerCharacter;
        [System.NonSerialized]
        public CharacterMovement MainPlayerCharacterMovement;

        // Start is called before the first frame update
        void Start()
        {
            PlayerEquippedWeapon = FindObjectOfType<Weapon>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}