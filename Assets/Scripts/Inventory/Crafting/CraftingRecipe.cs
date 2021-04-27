using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Inventory.Crafting
{
    [CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Inventory System/Crafting/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<ItemStack> Materials;
        public List<ItemStack> Results;


        public bool CanCraft(IItemContainer itemContainer)
        {
            foreach(ItemStack itemStack in Materials)
            {
                if(itemContainer.GetTotalQuantity(itemStack.item) < itemStack.quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public void Craft(IItemContainer itemContainer)
        {
            if (CanCraft(itemContainer))
            {
                foreach(ItemStack itemStack in Materials)
                {
                    //for (int i = 0; i < itemStack.quantity; i++)
                    //{
                        itemContainer.RemoveItem(itemStack);
                    //}
                }

                foreach (ItemStack itemStack in Results)
                {
                    //for (int i = 0; i < itemStack.quantity; i++)
                    //{
                        itemContainer.AddItem(itemStack);
                    //}
                }
            }
        }

    }
}