﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
	public Item Item;
	[Range(1, 999)]
	public int Amount;
}

[CreateAssetMenu(menuName = "Foxlair/Inventory/Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
	public List<ItemAmount> Materials;
	public List<ItemAmount> Results;

	public bool CanCraft(IItemContainer itemContainer)
	{
		return HasMaterials(itemContainer) && HasSpace(itemContainer);
	}

	private bool HasMaterials(IItemContainer itemContainer)
	{
		foreach (ItemAmount itemAmount in Materials)
		{
			if (itemContainer.ItemCount(itemAmount.Item.ID) < itemAmount.Amount)
			{
				Debug.LogWarning("You don't have the required materials.");
				return false;
			}
		}
		return true;
	}

	private bool HasSpace(IItemContainer itemContainer)
	{
		foreach (ItemAmount itemAmount in Results)
		{
			if (!itemContainer.CanAddItem(itemAmount.Item, itemAmount.Amount))
			{
				Debug.LogWarning("Your inventory is full.");
				return false;
			}
		}
		return true;
	}

	public void Craft(IItemContainer itemContainer)
	{
		if (CanCraft(itemContainer))
		{
			RemoveMaterials(itemContainer);
			AddResults(itemContainer);
		}
	}

	private void RemoveMaterials(IItemContainer itemContainer)
	{
		foreach (ItemAmount itemAmount in Materials)
		{
			for (int i = 0; i < itemAmount.Amount; i++)
			{
				Item oldItem = itemContainer.RemoveItem(itemAmount.Item.ID);
				oldItem.Destroy();
			}
		}
	}

	private void AddResults(IItemContainer itemContainer)
	{
		foreach (ItemAmount itemAmount in Results)
		{
			for (int i = 0; i < itemAmount.Amount; i++)
			{
				itemContainer.AddItem(itemAmount.Item.GetCopy());
			}
		}
	}
}
