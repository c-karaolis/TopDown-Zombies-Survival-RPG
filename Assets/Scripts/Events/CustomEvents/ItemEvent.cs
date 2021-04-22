using Foxlair.Inventory;
using UnityEngine;

namespace Foxlair.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Item Event", menuName = "Game Events/Item Event")]
    public class ItemEvent : BaseGameEvent<Item> { }
}