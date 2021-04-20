using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Foxlair.Inventory
{
    public class ItemDestroyer : MonoBehaviour
    {
        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI areYouSureText = null;

        private int slotIndex = 0;

        private void OnDisable()
        {
            slotIndex = -1;
        }

        public void Activate(ItemSlot itemSlot, int slotIndex)
        {
            this.slotIndex = slotIndex;

            areYouSureText.text = $"Are you sure you wish tp destroy {itemSlot.quantity}x {itemSlot.item.ColouredName}?";

            gameObject.SetActive(true);
        }


        public void Destroy()
        {
            inventory.ItemContainer.RemoveAt(slotIndex);

            gameObject.SetActive(false);
        }


    }
}