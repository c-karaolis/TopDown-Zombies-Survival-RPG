using UnityEngine;

namespace Foxlair.Inventory
{
    public interface IItemContainer 
    {
        ItemStack AddItem(ItemStack itemStack);
        void RemoveItem(ItemStack itemStack);
        void RemoveAt(int slotIndex);
        void Swap(int indexOne, int indexTwo);
        bool HasItem(InventoryItem item);
        int GetTotalQuantity(InventoryItem item);



    }
}