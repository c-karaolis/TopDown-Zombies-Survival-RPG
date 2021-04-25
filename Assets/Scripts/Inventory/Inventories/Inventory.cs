using Foxlair.Tools.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Foxlair.Inventory
{
    public class Inventory : MonoBehaviour, IItemContainer
    {
        [SerializeField] private int size = 20;

        [Header("testing stuff")]
        bool open = true;
        [SerializeField] GameObject inventoryUIPanel;
        [SerializeField] ItemStack itemToTest;

        public ItemStack GetSlotByIndex(int index) => itemSlots[index];

        private ItemStack[] itemSlots = new ItemStack[20];

        public void Start()
        {
            itemSlots = new ItemStack[size];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                AddItem(itemToTest);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryUIPanel.SetActive(open);
                open = !open;
            }
        }

        public ItemStack AddItem(ItemStack requestedItemSlot)
        {

            //check for stacks iteration
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                {
                    if (itemSlots[i].item == requestedItemSlot.item)
                    {
                        int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                        if (requestedItemSlot.quantity <= slotRemainingSpace)
                        {
                            itemSlots[i].quantity += requestedItemSlot.quantity;

                            requestedItemSlot.quantity = 0;

                            FoxlairEventManager.Instance.onInventoryItemsUpdated();


                            return requestedItemSlot;
                        }
                        else if (slotRemainingSpace > 0)
                        {
                            itemSlots[i].quantity += slotRemainingSpace;

                            requestedItemSlot.quantity -= slotRemainingSpace;
                        }
                    }
                }
            }

            //empty slots iteration. if quantity transferred is more than max stack, add it and continue iteration with the remaining amount
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item == null)
                {
                    if (requestedItemSlot.quantity <= requestedItemSlot.item.MaxStack)
                    {
                        itemSlots[i] = requestedItemSlot;

                        requestedItemSlot.quantity = 0;

                        FoxlairEventManager.Instance.onInventoryItemsUpdated();

                        return requestedItemSlot;
                    }
                    else
                    {
                        itemSlots[i] = new ItemStack(requestedItemSlot.item, requestedItemSlot.item.MaxStack);

                        requestedItemSlot.quantity -= requestedItemSlot.item.MaxStack;
                    }
                }
            }

            FoxlairEventManager.Instance.onInventoryItemsUpdated();

            return requestedItemSlot;
        }


        public bool HasItem(InventoryItem item)
        {
            foreach (ItemStack itemStack in itemSlots)
            {
                if (itemStack.item == null) { continue; }
                //If this gets passed it means we found the item.
                if (itemStack.item != item) { continue; }

                return true;
            }
            return false;
        }

        public void RemoveAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }

            itemSlots[slotIndex] = new ItemStack();

            FoxlairEventManager.Instance.onInventoryItemsUpdated();

        }

        public void RemoveItem(ItemStack itemStackToRemove)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                {
                    if (itemSlots[i].item == itemStackToRemove.item)
                    {
                        if (itemSlots[i].quantity < itemStackToRemove.quantity)
                        {
                            itemStackToRemove.quantity -= itemSlots[i].quantity;

                            itemSlots[i] = new ItemStack();
                        }
                        else
                        {
                            itemSlots[i].quantity -= itemStackToRemove.quantity;

                            if (itemSlots[i].quantity == 0)
                            {
                                itemSlots[i] = new ItemStack();

                                FoxlairEventManager.Instance.onInventoryItemsUpdated();

                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Swap(int indexOne, int indexTwo)
        {
            ItemStack firstSlot = itemSlots[indexOne];
            ItemStack secondSlot = itemSlots[indexTwo];

            if (firstSlot.Equals(secondSlot)) { return; }

            if (secondSlot.item != null)
            {
                if (firstSlot.item == secondSlot.item)
                {
                    int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;

                    if (firstSlot.quantity <= secondSlotRemainingSpace)
                    {
                        itemSlots[indexTwo].quantity += firstSlot.quantity;

                        itemSlots[indexOne] = new ItemStack();

                        FoxlairEventManager.Instance.onInventoryItemsUpdated();

                        return;
                    }
                }
            }

            itemSlots[indexOne] = secondSlot;
            itemSlots[indexTwo] = firstSlot;

            FoxlairEventManager.Instance.onInventoryItemsUpdated();

        }

        /// <summary>
        /// Returns the total quantity of a specific item in our inventory.
        /// </summary>
        /// <param name="item">The item to get the Quantity for</param>
        /// <returns>Item's quanity</returns>
        public int GetTotalQuantity(InventoryItem item)
        {
            int totalCount = 0;
            foreach (ItemStack itemSlot in itemSlots)
            {
                if (itemSlot.item == null) { continue; }
                if (itemSlot.item != item) { continue; }

                totalCount += itemSlot.quantity;
            }
            return totalCount;

        }



    }
}