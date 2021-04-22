using UnityEngine;
using UnityEngine.Events;

namespace Foxlair.Inventory
{
    public class Inventory : MonoBehaviour, IItemContainer
    {
        [SerializeField] private int size = 20;
        [SerializeField] private UnityEvent onInventoryItemsUpdated = null;

        [Header("testing stuff")]
        bool open = true;
        [SerializeField] GameObject inventoryUIPanel;
        [SerializeField] ItemSlot itemToTest;

        public ItemSlot GetSlotByIndex(int index) => itemSlots[index];

        private ItemSlot[] itemSlots = new ItemSlot[20];

        public void Start()
        {
            itemSlots = new ItemSlot[size];
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

        public ItemSlot AddItem(ItemSlot requestedItemSlot)
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

                            onInventoryItemsUpdated.Invoke();

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

                        onInventoryItemsUpdated.Invoke();

                        return requestedItemSlot;
                    }
                    else
                    {
                        itemSlots[i] = new ItemSlot(requestedItemSlot.item, requestedItemSlot.item.MaxStack);

                        requestedItemSlot.quantity -= requestedItemSlot.item.MaxStack;
                    }
                }
            }

            onInventoryItemsUpdated.Invoke();
            
            return requestedItemSlot;
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

            onInventoryItemsUpdated.Invoke();

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

                                onInventoryItemsUpdated.Invoke();

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

            if (firstSlot.Equals(secondSlot)) { return; }

            if (secondSlot.item != null)
            {
                if (firstSlot.item == secondSlot.item)
                {
                    int secondSlotRemainingSpace = secondSlot.item.MaxStack - secondSlot.quantity;

                    if (firstSlot.quantity <= secondSlotRemainingSpace)
                    {
                        itemSlots[indexTwo].quantity += firstSlot.quantity;

                        itemSlots[indexOne] = new ItemSlot();

                        onInventoryItemsUpdated.Invoke();

                        return;
                    }
                }
            }

            itemSlots[indexOne] = secondSlot;
            itemSlots[indexTwo] = firstSlot;

            onInventoryItemsUpdated.Invoke();

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