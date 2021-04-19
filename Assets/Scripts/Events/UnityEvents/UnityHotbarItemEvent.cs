using Foxlair.Inventory;
using System;
using UnityEngine.Events;

namespace Foxlair.Events.UnityEvents
{
    [Serializable] public class UnityHotbarItemEvent : UnityEvent<HotbarItem> { }
}