using Foxlair.Inventory.Hotbars;
using System.Text;
using UnityEngine;

namespace Foxlair.Inventory
{

    [CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory System/Items/Consumable Item")]
    public class ConsumableItem : InventoryItem, IHotbarItem
    {
        [Header("Consumable Data")]
        [SerializeField] private string useText = "Does something, maybe?";
        public override string GetInfoDisplayText()
        {
            StringBuilder builder = new StringBuilder();
           // builder.Append(Name).AppendLine();
            builder.Append(Rarity.Name).AppendLine();
            builder.Append("<color=green>Use: ").Append(useText).Append("</color>").AppendLine();
            builder.Append("Max Stack: ").Append(MaxStack).AppendLine();
            builder.Append("Sell Price: ").Append(SellPrice).Append(" Gold");

            return builder.ToString();
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}