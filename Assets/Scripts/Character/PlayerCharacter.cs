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

        public bool InRangeToHarvest()
        {
            if (Vector3.Distance(PlayerManager.Instance.PlayerTargetResourceNode.transform.position, transform.position) <= 2)
            {
                Debug.Log("player in range to harvest");
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}