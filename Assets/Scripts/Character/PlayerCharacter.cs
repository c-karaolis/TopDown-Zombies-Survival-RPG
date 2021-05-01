using Foxlair.Character.Movement;
using Opsive.UltimateInventorySystem.Core.InventoryCollections;
using UnityEngine;

namespace Foxlair.Character
{
    public class PlayerCharacter : Actor
    {
        public float health = 10f;

        private void Start()
        {
            PlayerManager.Instance.MainPlayerCharacter = this;
            PlayerManager.Instance.MainPlayerCharacterMovement = GetComponent<CharacterMovement>();
            PlayerManager.Instance.MainPlayerCharacterInventory = GetComponent<Inventory>();
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

    }
}