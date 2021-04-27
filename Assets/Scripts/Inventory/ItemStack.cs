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
        public Item item;
        public int quantity;

        public ItemStack(Item item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

    }
}