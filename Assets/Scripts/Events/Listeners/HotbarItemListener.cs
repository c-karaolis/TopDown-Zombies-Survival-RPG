using Foxlair.Events.CustomEvents;
using Foxlair.Events.UnityEvents;
using Foxlair.Items;

namespace Foxlair.Events.Listeners
{
    public class HotbarItemListener : BaseGameEventListener<HotbarItem, HotbarItemEvent, UnityHotbarItemEvent> { }
}
