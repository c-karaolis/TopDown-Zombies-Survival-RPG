﻿using Foxlair.Character;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Foxlair/Inventory/Items/Usable Item")]
public class UsableItem : Item
{
	public bool IsConsumable;

	public List<UsableItemEffect> Effects;

	public virtual void Use(PlayerCharacter character)
	{
		foreach (UsableItemEffect effect in Effects)
		{
			effect.ExecuteEffect(this, character);
		}
	}

	public override string GetItemType()
	{
		return IsConsumable ? "Consumable" : "Usable";
	}

	public override string GetDescription()
	{
		sb.Length = 0;
		foreach (UsableItemEffect effect in Effects)
		{
			sb.AppendLine(effect.GetDescription());
		}
		return sb.ToString();
	}
}
