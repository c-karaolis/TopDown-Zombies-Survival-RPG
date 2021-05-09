using UnityEngine;
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
	public int PerceptionBonus;
	public int LuckBonus;
	public int CharismaBonus;
	[Space]
	public float StrengthPercentBonus;
	public float AgilityPercentBonus;
	public float IntelligencePercentBonus;
	public float VitalityPercentBonus;
	public int PerceptionPercentBonus;
	public int LuckPercentBonus;
	public int CharismaPercentBonus;
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

	public void Equip(PlayerCharacter character)
	{
        //     foreach (AttributeModifier attributeModifier in attributeModifiers)
        //     {
        //         CharacterStat characterStat = character.CharacterAttributes[attributeModifier.AttributeType];
        //         StatModifier statModifier = new StatModifier(attributeModifier.Value, attributeModifier.Type, this);
        //Debug.Log($"Adding mod: ${statModifier.Value} to character stat: ${characterStat.Value}");
        //         characterStat.AddModifier(statModifier);
        //     }


		//Add flat bonuses
        if (StrengthBonus != 0)
            character.Strength.AddModifier(new StatModifier(StrengthBonus, StatModType.Flat, this));
        if (AgilityBonus != 0)
            character.Agility.AddModifier(new StatModifier(AgilityBonus, StatModType.Flat, this));
        if (IntelligenceBonus != 0)
            character.Intelligence.AddModifier(new StatModifier(IntelligenceBonus, StatModType.Flat, this));
        if (VitalityBonus != 0)
            character.Vitality.AddModifier(new StatModifier(VitalityBonus, StatModType.Flat, this));
		if (PerceptionBonus != 0)
			character.Perception.AddModifier(new StatModifier(PerceptionBonus, StatModType.Flat, this));
		if (LuckBonus != 0)
			character.Luck.AddModifier(new StatModifier(LuckBonus, StatModType.Flat, this));
		if (CharismaBonus != 0)
			character.Charisma.AddModifier(new StatModifier(CharismaBonus, StatModType.Flat, this));


		//Add percent bonuses
		if (StrengthPercentBonus != 0)
            character.Strength.AddModifier(new StatModifier(StrengthPercentBonus, StatModType.PercentMult, this));
        if (AgilityPercentBonus != 0)
            character.Agility.AddModifier(new StatModifier(AgilityPercentBonus, StatModType.PercentMult, this));
        if (IntelligencePercentBonus != 0)
            character.Intelligence.AddModifier(new StatModifier(IntelligencePercentBonus, StatModType.PercentMult, this));
        if (VitalityPercentBonus != 0)
            character.Vitality.AddModifier(new StatModifier(VitalityPercentBonus, StatModType.PercentMult, this));
		if (PerceptionPercentBonus != 0)
			character.Perception.AddModifier(new StatModifier(PerceptionPercentBonus, StatModType.Flat, this));
		if (LuckPercentBonus != 0)
			character.Luck.AddModifier(new StatModifier(LuckPercentBonus, StatModType.Flat, this));
		if (CharismaPercentBonus != 0)
			character.Charisma.AddModifier(new StatModifier(CharismaPercentBonus, StatModType.Flat, this));

	}

	public void Unequip(PlayerCharacter character)
	{
        //foreach (KeyValuePair<AttributeType, CharacterAttribute> attribute in character.CharacterAttributes)
        //{
        //    attribute.Value.RemoveAllModifiersFromSource(this);
        //}
        character.Strength.RemoveAllModifiersFromSource(this);
        character.Agility.RemoveAllModifiersFromSource(this);
        character.Intelligence.RemoveAllModifiersFromSource(this);
        character.Vitality.RemoveAllModifiersFromSource(this);
        character.Perception.RemoveAllModifiersFromSource(this);
        character.Luck.RemoveAllModifiersFromSource(this);
        character.Charisma.RemoveAllModifiersFromSource(this);
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
		AddStat(PerceptionBonus, "Perception");
		AddStat(LuckBonus, "Luck");
		AddStat(CharismaBonus, "Charisma");

		AddStat(StrengthPercentBonus, "Strength", isPercent: true);
		AddStat(AgilityPercentBonus, "Agility", isPercent: true);
		AddStat(IntelligencePercentBonus, "Intelligence", isPercent: true);
		AddStat(VitalityPercentBonus, "Vitality", isPercent: true);
		AddStat(PerceptionPercentBonus, "Perception", isPercent: true);
		AddStat(LuckPercentBonus, "Luck", isPercent: true);
		AddStat(CharismaPercentBonus, "Charisma", isPercent: true);

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
