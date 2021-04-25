using Foxlair.Inventory.Hotbars;
using Foxlair.Tools.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxlair.Inventory
{
    public class InventorySlotUI : ItemSlotUI, IDropHandler
    {

        [SerializeField] private Inventory inventory = null;
        [SerializeField] private TextMeshProUGUI itemQuantityText;

        public override Item SlotItem 
        {
            get { return ItemStackInSlot.item; }
            set { }
        }

        public ItemStack ItemStackInSlot => inventory.GetSlotByIndex(SlotIndex);

        private void Awake()
        {
            FoxlairEventManager.Instance.OnInventoryItemsUpdated += UpdateSlotUI;
        }

        private void OnDisable()
        {
            FoxlairEventManager.Instance.OnInventoryItemsUpdated -= UpdateSlotUI;
        }

        public override void OnDrop(PointerEventData eventData)
        {
            ItemDragHandler itemDragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();

            if(itemDragHandler == null) { return; }

            if((itemDragHandler.ItemSlotUI as InventorySlotUI) != null)
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
