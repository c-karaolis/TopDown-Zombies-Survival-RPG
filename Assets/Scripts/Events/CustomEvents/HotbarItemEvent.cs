using Foxlair.Inventory;
using UnityEngine;

namespace Foxlair.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Hotbar Item Event", menuName = "Game Events/Hotbar Item Event")]
    public class HotbarItemEvent : BaseGameEvent<Item> { }
}