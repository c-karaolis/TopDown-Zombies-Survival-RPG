﻿using UnityEngine;
using Foxlair.CharacterStats;
using System.Collections.Generic;
using Foxlair.Character;

public enum EquipmentType
{
	Helmet,
	Chest,
	Gloves,
	Boots,
	Pants,
	Weapon,
	Neck,
	Ring1,
	Ring2,
}

[CreateAssetMenu(menuName = "Foxlair/Inventory/Items/Equippable Item")]
public class EquippableItem : Item
{
	public int StrengthBonus;
	public int AgilityBonus;
	public int IntelligenceBonus;
	public int VitalityBonus;
	[Space]
	public float StrengthPercentBonus;
	public float AgilityPercentBonus;
	public float IntelligencePercentBonus;
	public float VitalityPercentBonus;
	[Space]
	public EquipmentType EquipmentType;
	[Space]
	public GameObject PhysicalItemPrefab;
	public List<AttributeModifier> attributeModifiers;
	public override Item GetCopy()
	{
		return Instantiate(this);
	}

	public override void Destroy()
	{
		Destroy(this);
	}

	public void Equip(Character c)
	{
		//foreach(AttributeModifier attributeModifier in attributeModifiers)
  //      {
		//	CharacterStat characterStat = c.CharacterAttributes[attributeModifier.AttributeType];
		//	StatModifier statModifier = new StatModifier(attributeModifier.Value, attributeModifier.Type, this);
		//	characterStat.AddModifier(statModifier);
		//}

		if (StrengthBonus != 0)
			c.Strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
		if (AgilityBonus != 0)
			c.Agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
		if (IntelligenceBonus != 0)
			c.Intelligence.AddModifier(new StatModifier(IntelligenceBonus, StatModType.Flat, this));
		if (VitalityBonus != 0)
			c.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));

		if (StrengthPercentBonus != 0)
			c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
		if (AgilityPercentBonus != 0)
			c.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
		if (IntelligencePercentBonus != 0)
			c.Intelligence.AddModifier(new StatModifier(IntelligencePercentBonus, StatModType.PercentMult, this));
		if (VitalityPercentBonus != 0)
			c.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));
	}

	public void Unequip(Character c)
	{
		//foreach (KeyValuePair<AttributeType, CharacterAttribute> attribute in c.CharacterAttributes)
		//{
		//	attribute.Value.RemoveAllModifiersFromSource(this);
		//}
		c.Strength.RemoveAllModifiersFromSource(this);
		c.Agility.RemoveAllModifiersFromSource(this);
		c.Intelligence.RemoveAllModifiersFromSource(this);
		c.Vitality.RemoveAllModifiersFromSource(this);
	}

	public override string GetItemType()
	{
		return EquipmentType.ToString();
	}

	public override string GetDescription()
	{
		sb.Length = 0;
		AddStat(StrengthBonus, "Strength");
		AddStat(AgilityBonus, "Agility");
		AddStat(IntelligenceBonus, "Intelligence");
		AddStat(VitalityBonus, "Vitality");

		AddStat(StrengthPercentBonus, "Strength", isPercent: true);
		AddStat(AgilityPercentBonus, "Agility", isPercent: true);
		AddStat(IntelligencePercentBonus, "Intelligence", isPercent: true);
		AddStat(VitalityPercentBonus, "Vitality", isPercent: true);

		return sb.ToString();
	}

	private void AddStat(float value, string statName, bool isPercent = false)
	{
		if (value != 0)
		{
			if (sb.Length > 0)
				sb.AppendLine();

			if (value > 0)
				sb.Append("+");

			if (isPercent) {
				sb.Append(value * 100);
				sb.Append("% ");
			} else {
				sb.Append(value);
				sb.Append(" ");
			}
			sb.Append(statName);
		}
	}
}
