using Foxlair.Character;
using Foxlair.Character.States;
using Foxlair.Tools;
using Foxlair.Tools.StateMachine;
using Foxlair.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Character
{

    public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
    {

        public Weapon playerEquippedWeapon;

        // Start is called before the first frame update
        void Start()
        {
            playerEquippedWeapon = FindObjectOfType<Weapon>();
        }





        // Update is called once per frame
        void Update()
        {

        }
    }
}