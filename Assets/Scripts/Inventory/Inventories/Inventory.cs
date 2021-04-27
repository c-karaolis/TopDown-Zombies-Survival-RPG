using Foxlair.Tools.Events;
using UnityEngine;
using UnityEngine.Events;

using Foxlair.Currencies;
using Foxlair.Inventory.Crafting;

namespace Foxlair.Inventory
{
    public class Inventory : MonoBehaviour, IItemContainer
    {
        [SerializeField] private int size = 20;


        //TODO: Remove testing stuff when done
        [Header("testing stuff")]
        bool open = true;
        [SerializeField] GameObject inventoryUIPanel;
        [SerializeField] ItemStack itemToTest;
        [SerializeField] ItemStack itemToTest2;
        [SerializeField] Wallet wallet;
        [SerializeField] CraftingRecipe craftingRecipe;



        public ItemStack GetSlotByIndex(int index) => itemSlots[index];

        [SerializeField] private ItemStack[] itemSlots = new ItemStack[20];

        public void Start()
        {
            itemSlots = new ItemStack[size];

            //wallet = gameObject.GetComponent<Wallet>();
        }

        private void Update()
        {
            //TODO: Remove testing stuff when done

            if (Input.GetKeyDown(KeyCode.K))
            {
                AddItem(itemToTest);
                AddItem(itemToTest2);
                wallet.Currency.Add(11);

                Debug.Log($"Gold: {wallet.Currency.Gold}, Silver: {wallet.Currency.Silver},Copper: {wallet.Currency.Copper}");
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                wallet.Currency.Add(4544);
                long[] test = wallet.Currency.ConvertValueExchange(4544);
                craftingRecipe.Craft(this);
                Debug.Log($"Trying to add Gold: {test[0]}, Silver: {test[1]},Copper: {test[2]}");
                Debug.Log($"Gold: {wallet.Currency.Gold}, Silver: {wallet.Currency.Silver},Copper: {wallet.Currency.Copper}");
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                wallet.Currency.Remove(3000);
                Debug.Log($"Gold: {wallet.Currency.Gold}, Silver: {wallet.Currency.Silver},Copper: {wallet.Currency.Copper}");
                inventoryUIPanel.SetActive(open);
                FoxlairEventManager.Instance.onInventoryItemsUpdated();

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
                            //substracting the removed item from the quantity of slot
                            itemSlots[i].quantity -= itemStackToRemove.quantity;

                            //if after substraction slot is empty return.
                            if (itemSlots[i].quantity == 0)
                            {
                                itemSlots[i] = new ItemStack();

                                FoxlairEventManager.Instance.onInventoryItemsUpdated();

                                return;
                            }

                            //if after substraction slot is still not empty again return.
                            FoxlairEventManager.Instance.onInventoryItemsUpdated();
                            return;
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