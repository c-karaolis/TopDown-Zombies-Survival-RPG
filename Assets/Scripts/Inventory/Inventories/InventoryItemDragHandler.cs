using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxlair.Inventory
{
    public class InventoryItemDragHandler : ItemDragHandler
    {
        [SerializeField] private ItemDestroyer itemDestroyer = null;
        public override void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                base.OnPointerUp(eventData);

                if (eventData.hovered.Count == 0)
                {
                    InventorySlotUI thisSlot = ItemSlotUI as InventorySlotUI;
                    itemDestroyer.Activate(thisSlot.ItemStackInSlot, thisSlot.SlotIndex);
                }
            }
        }
    }
}