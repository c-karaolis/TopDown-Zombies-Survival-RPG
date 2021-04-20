using Foxlair.Events.CustomEvents;
using UnityEngine;

namespace Foxlair.Inventory
{
    [CreateAssetMenu(fileName ="New Inventory", menuName ="Inventory System/Inventory")]
    public class Inventory : ScriptableObject
    {
        [SerializeField] private VoidEvent onInventoryItemsUpdated = null;
        [SerializeField] private ItemSlot testItemSlot = new ItemSlot();
        public ItemContainer ItemContainer { get; } = new ItemContainer(20);

        private void OnEnable()
        {
            ItemContainer.OnItemsUpdated += onInventoryItemsUpdated.Raise;
        }

        private void OnDisable()
        {
            ItemContainer.OnItemsUpdated -= onInventoryItemsUpdated.Raise;
        }

        [ContextMenu("Test Add")]
        public void TestAdd()
        {
            ItemContainer.AddItem(testItemSlot);
        }

    }
}