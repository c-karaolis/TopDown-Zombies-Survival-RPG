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
        foreach (AttributeModifier attributeModifier in attributeModifiers)
        {
            CharacterAttribute characterStat = character.CharacterAttributes[attributeModifier.AttributeType];
            AttributeModifier statModifier = new AttributeModifier(attributeModifier.Value, attributeModifier.Type, this);
            Debug.Log($"Adding mod: +{statModifier.Value} {attributeModifier.AttributeType} to character attribute: {characterStat.Name}");
            characterStat.AddModifier(statModifier);
        }
    }

    public void Unequip(PlayerCharacter character)
    {
        foreach (KeyValuePair<AttributeType, CharacterAttribute> attribute in character.CharacterAttributes)
        {
            attribute.Value.RemoveAllModifiersFromSource(this);
            Debug.Log($"Removing all mods from {attribute.Value.Name} related to item: {this.name}");
        }
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        foreach (AttributeModifier attributeModifier in attributeModifiers)
        {
            if (attributeModifier.Type == StatModType.Flat)
            {
                AddStat(attributeModifier.Value, attributeModifier.AttributeType.ToString());
            }
            else
            {
                AddStat(attributeModifier.Value, attributeModifier.AttributeType.ToString(), true);
            }
        }

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

            if (isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            sb.Append(statName);
        }
    }
}
