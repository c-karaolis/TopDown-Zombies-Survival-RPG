using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Character
{
    public class PlayerCharacter : Actor
    {
        public float health = 10f;

        private void Start()
        {
            PlayerManager.Instance.MainPlayerCharacter = this;
        }
    }
}