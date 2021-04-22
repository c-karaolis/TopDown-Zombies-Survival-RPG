using System;
using System.Collections.Generic;

namespace Foxlair.Inventory
{
    /// <summary>
    /// Struct comprised of an Inventory Item and its quantity.
    /// </summary>
    [Serializable]
    public struct ItemSlot
    {
        public InventoryItem item;
        public int quantity;

        public ItemSlot(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

    }
}