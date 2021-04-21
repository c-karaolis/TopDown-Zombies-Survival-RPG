using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Inventory.Hotbars
{
    public interface IHotbarItem
    {
        string Name { get; }
        Sprite Icon { get; }
        void Use();

    }
}