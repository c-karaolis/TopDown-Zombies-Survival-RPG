using Foxlair.Inventory.Hotbars;
using UnityEngine;

namespace Foxlair.Inventory
{
    public abstract class Item : ScriptableObject
    {
        [Header("Item Data")]
        [SerializeField] private Rarity rarity = null;
        [SerializeField][Min(0)]private int sellPrice = 1;
        [SerializeField][Min(1)] private int maxStack = 1;
        [Header("Basic Info")]
        [SerializeField] private new string name = "New Item Name";
        [SerializeField] private Sprite icon = null;
        public string ColouredName
        {
            get
            {
                string hexColour = ColorUtility.ToHtmlStringRGB(rarity.Colour);
                return $"<color=#{hexColour}>{Name}</color>";
            }
        }
        public string Name => name;
        public Sprite Icon => icon;

        public int SellPrice => sellPrice;
        public int MaxStack => maxStack;
        public Rarity Rarity => rarity;
        public abstract string GetInfoDisplayText();

    }
}