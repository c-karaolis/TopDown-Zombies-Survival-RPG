using System;
using System.Collections.Generic;

namespace Foxlair.Inventory
{
    /// <summary>
    /// Struct comprised of an Inventory Item and its quantity.
    /// </summary>
    [Serializable]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    public struct ItemSlot
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public InventoryItem item;
        public int quantity;

        public ItemSlot(InventoryItem item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemSlot slot &&
                   EqualityComparer<InventoryItem>.Default.Equals(item, slot.item) &&
                   quantity == slot.quantity;
        }

        public override int GetHashCode()
        {
            int hashCode = -301187666;
            hashCode = hashCode * -1521134295 + EqualityComparer<InventoryItem>.Default.GetHashCode(item);
            hashCode = hashCode * -1521134295 + quantity.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ItemSlot a, ItemSlot b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ItemSlot a, ItemSlot b)
        {
            return !a.Equals(b);
        }
    }
}