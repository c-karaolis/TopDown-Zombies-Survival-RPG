using Foxlair.Events.CustomEvents;
using Foxlair.Events.UnityEvents;
using Foxlair.Inventory.Hotbars;

namespace Foxlair.Events.Listeners
{
    public class HotbarItemListener : BaseGameEventListener<HotbarItem, HotbarItemEvent, UnityHotbarItemEvent> { }
}
