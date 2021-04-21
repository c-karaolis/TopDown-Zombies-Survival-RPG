using Foxlair.Inventory;
using Foxlair.Inventory.Hotbars;
using System;
using UnityEngine.Events;

namespace Foxlair.Events.UnityEvents
{
    [Serializable] public class UnityHotbarItemEvent : UnityEvent<Item> { }
}