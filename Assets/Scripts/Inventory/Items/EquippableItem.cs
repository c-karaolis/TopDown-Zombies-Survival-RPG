using UnityEngine;
using Foxlair.CharacterStats;
using System.Collections.Generic;
using Foxlair.Character;
using Foxlair.Weapons;
using Foxlair.Tools.Events;

public enum EquipmentType
{
    Helmet,
    Chest,
    Gloves,
    Shoes,
    Pants,
    Weapon,
    Neck,
    Ring,
}

[CreateAssetMenu(menuName = "Foxlair/Inventory/Items/Equippable Item")]
public class EquippableItem : Item
{
    public EquipmentType EquipmentType;
    [Space]
    public GameObject PhysicalItemPrefab;
    public GameObject InstanceOfPhysicalItemPrefab = null;
    public List<AttributeModifier> attributeModifiers;

    public int durability = 40;
    private string EquipID;
    public bool isDirty=false;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
        //DestroyImmediate(this,t);
    }

    public void Equip(PlayerCharacter character)
    {

        foreach (AttributeModifier attributeModifier in attributeModifiers)
        {
            CharacterAttribute characterStat = character.CharacterAttributes[attributeModifier.AttributeType];
            AttributeModifier statModifier = new AttributeModifier(attributeModifier.Value, attributeModifier.Type, this);
            characterStat.AddModifier(statModifier);
        }

        if (PhysicalItemPrefab != null)
        {
            InstanceOfPhysicalItemPrefab = Instantiate(PhysicalItemPrefab, character.weaponEquipPoint.transform);
            if (isDirty)
            {
                Debug.Log($"Item {this.name} was dirty.");
                Debug.Log($"Durability of physical is {InstanceOfPhysicalItemPrefab.GetComponent<Weapon>().durability}.");
                InstanceOfPhysicalItemPrefab.GetComponent<Weapon>().durability = durability;
            }
            else
            {
                Debug.Log($"Item {this.name} was clean.");
                durability = InstanceOfPhysicalItemPrefab.GetComponent<Weapon>().durability;
            }

            InstanceOfPhysicalItemPrefab.GetComponent<Weapon>().InventoryItemInstance = this;

        }
        isDirty = true;
    }

    public void Unequip(PlayerCharacter character)
    {

        foreach (KeyValuePair<AttributeType, CharacterAttribute> attribute in character.CharacterAttributes)
        {
            attribute.Value.RemoveAllModifiersFromSource(this);
        }

        if (PhysicalItemPrefab != null)
        {
            Destroy(InstanceOfPhysicalItemPrefab, 0.1f);
        }
        //FoxlairEventManager.Instance.Player_OnItemUnEquipped_Event?.Invoke(this);

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
        AddStat(durability, "Dura");
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
