﻿using UnityEngine;

namespace Foxlair.Items
{
    public abstract class InventoryItem : HotbarItem
    {
        [Header("Item Data")]
        [Min(0)]private int sellPrice = 1;
        [Min(1)] private int maxStack = 1;

        public override string ColouredName
        {
            get
            {
                return Name;
            }
        }

        public int SellPrice => SellPrice;
        public int MaxStack => MaxStack;

    }
}