using Foxlair.Inventory.Hotbars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxlair.Inventory
{
    public class InventorySlot : ItemSlotUI, IDropHandler
    {

        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI itemQuantityText;

        public override Item SlotItem 
        {
            get { return ItemStackInSlot.item; }
            set { }
        }

        public ItemStack ItemStackInSlot => inventory.GetSlotByIndex(SlotIndex);

        public override void OnDrop(PointerEventData eventData)
        {
            ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

            if(itemDragHandler == null) { return; }

            if((itemDragHandler.ItemSlotUI as InventorySlot) != null)
            {
                inventory.Swap(itemDragHandler.ItemSlotUI.SlotIndex, SlotIndex);
            }
        }

        public override void UpdateSlotUI()
        {
            if(ItemStackInSlot.item == null)
            {
                EnableSlotUI(false);
                return;
            }

            EnableSlotUI(true);

            itemIconImage.sprite = ItemStackInSlot.item.Icon;
            itemQuantityText.text = ItemStackInSlot.quantity > 1 ? ItemStackInSlot.quantity.ToString() : "";
        }


        protected override void EnableSlotUI(bool enable)
        {
            base.EnableSlotUI(enable);

            itemQuantityText.enabled = enable;
        }


    }
}
