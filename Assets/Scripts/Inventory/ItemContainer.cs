﻿using System;

namespace Foxlair.Inventory
{
    public class ItemContainer : IItemContainer
    {

        //We can use Lists to make it dynamic. 
        private ItemSlot[] itemSlots = new ItemSlot[20];

        public Action OnItemsUpdated = delegate { };

        public ItemContainer(int size) => itemSlots = new ItemSlot[size];
        public ItemSlot GetSlotByIndex(int index) => itemSlots[index];

        public ItemSlot AddItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if(itemSlots[i].item != null)
                {
                    if(itemSlots[i].item == itemSlot.item)
                    {
                        int slotRemainingSpace = itemSlots[i].item.MaxStack - itemSlots[i].quantity;

                        if(itemSlot.quantity <= slotRemainingSpace)
                        {
                            itemSlots[i].quantity += itemSlot.quantity;

                            itemSlot.quantity =0;

                            OnItemsUpdated.Invoke();

                            return itemSlot;
                        }
                        else if (slotRemainingSpace>0)
                        {
                            itemSlots[i].quantity += slotRemainingSpace;

                            itemSlot.quantity -= slotRemainingSpace;
                        }
                    }
                }
            }


            for(int i=0; i < itemSlots.Length; i++)
            {
                if(itemSlots[i].item == null)
                {
                    if (itemSlot.quantity <= itemSlot.item.MaxStack)
                    {
                        itemSlots[i] = itemSlot;

                        itemSlot.quantity = 0;

                        OnItemsUpdated.Invoke();

                        return itemSlot;
                    }
                    else
                    {
                        itemSlots[i] = new ItemSlot(itemSlot.item, itemSlot.item.MaxStack);

                        itemSlot.quantity -= itemSlot.item.MaxStack;
                    }
                }
            }

            OnItemsUpdated.Invoke();
            return itemSlot;
        }


        public bool HasItem(InventoryItem item)
        {
            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.item == null) { continue; }
                //If this gets passed it means we found the item.
                if (itemSlot.item != item) { continue; }

                return true;
            }
            return false;
        }

        public void RemoveAt(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex > itemSlots.Length - 1) { return; }

            itemSlots[slotIndex] = new ItemSlot();

            OnItemsUpdated.Invoke();
        }

        public void RemoveItem(ItemSlot itemSlot)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (itemSlots[i].item != null)
                {
                    if (itemSlots[i].item == itemSlot.item)
                    {
                        if (itemSlots[i].quantity < itemSlot.quantity)
                        {
                            itemSlot.quantity -= itemSlots[i].quantity;

                            itemSlots[i] = new ItemSlot();
                        }
                        else
                        {
                            itemSlots[i].quantity -= itemSlot.quantity;

                            if (itemSlots[i].quantity == 0)
                            {
                                itemSlots[i] = new ItemSlot();

                                OnItemsUpdated.Invoke();

                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Swap(int indexOne, int indexTwo)
        {
            ItemSlot firstSlot = itemSlots[indexOne];
            ItemSlot secondSlot = itemSlots[indexTwo];

            if (firstSlot == secondSlot) { return; }

            if (secondSlot.item != null)
            {
                if (firstSlot.item == secondSlot.item)
                {
                    int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;

                    if (firstSlot.quantity <= secondSlotRemainingSpace)
                    {
                        itemSlots[indexTwo].quantity += firstSlot.quantity;

                        itemSlots[indexOne] = new ItemSlot();

                        OnItemsUpdated.Invoke();

                        return;
                    }
                }
            }

            itemSlots[indexOne] = secondSlot;
            itemSlots[indexTwo] = firstSlot;

            OnItemsUpdated.Invoke();

        }

        /// <summary>
        /// Returns the total quantity of a specific item in our inventory.
        /// </summary>
        /// <param name="item">The item to get the Quantity for</param>
        /// <returns>Item's quanity</returns>
        public int GetTotalQuantity(InventoryItem item)
        {
            int totalCount = 0;
            foreach (ItemSlot itemSlot in itemSlots)
            {
                if (itemSlot.item == null) { continue; }
                if (itemSlot.item != item) { continue; }

                totalCount += itemSlot.quantity;
            }
            return totalCount;

        }
    }
}