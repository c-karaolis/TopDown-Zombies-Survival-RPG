using Foxlair.Events.CustomEvents;
using Foxlair.Events.UnityEvents;
using Foxlair.Inventory;

namespace Foxlair.Events.Listeners
{
    public class ItemListener : BaseGameEventListener<Item, ItemEvent, UnityItemEvent> { }
}
