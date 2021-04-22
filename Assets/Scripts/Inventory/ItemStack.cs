using System;
using System.Collections.Generic;

namespace Foxlair.Inventory
{
    /// <summary>
    /// Struct comprised of an Inventory Item and its quantity.
    /// </summary>
    [Serializable]
    public struct ItemStack
    {
        public InventoryItem item;
        public int quantity;

        public ItemStack(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

    }
}