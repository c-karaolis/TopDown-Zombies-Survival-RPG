using System.Collections.Generic;
using UnityEngine;

public class ItemSaveManager : MonoBehaviour
{
    [SerializeField] ItemDatabase itemDatabase;

    private const string InventoryFileName = "Inventory";
    private const string EquipmentFileName = "Equipment";

    public void LoadInventory(InventoryController character)
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(InventoryFileName);
        if (savedSlots == null) return;
        character.Inventory.Clear();

        for (int i = 0; i < savedSlots.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = character.Inventory.ItemSlots[i];
            ItemSlotSaveData savedSlot = savedSlots.SavedSlots[i];

            if (savedSlot == null)
            {
                itemSlot.Item = null;
                itemSlot.Amount = 0;
            }
            else
            {
                int itemDurability = savedSlot.Durability;
                itemSlot.Item = itemDatabase.GetItemCopy(savedSlot.ItemID);

                if (itemDurability != 0)
                {
                    (itemSlot.Item as EquippableItem).durability = itemDurability;
                    (itemSlot.Item as EquippableItem).isDirty = true;
                }
                itemSlot.Amount = savedSlot.Amount;
            }
        }
    }

    public void LoadEquipment(InventoryController character)
    {
        ItemContainerSaveData savedSlots = ItemSaveIO.LoadItems(EquipmentFileName);
        if (savedSlots == null) return;

        foreach (ItemSlotSaveData savedSlot in savedSlots.SavedSlots)
        {
            if (savedSlot == null)
            {
                continue;
            }
            int itemDurability = savedSlot.Durability;
            Item item = itemDatabase.GetItemCopy(savedSlot.ItemID);

            if (itemDurability != 0)
            {
                (item as EquippableItem).durability = itemDurability;
                (item as EquippableItem).isDirty = true;
            }

            character.Inventory.AddItem(item);
            character.Equip((EquippableItem)item);
        }
    }

    public void SaveInventory(InventoryController character)
    {
        SaveItems(character.Inventory.ItemSlots, InventoryFileName);
    }

    public void SaveEquipment(InventoryController character)
    {
        SaveItems(character.EquipmentPanel.EquipmentSlots, EquipmentFileName);
    }

    private void SaveItems(IList<ItemSlot> itemSlots, string fileName)
    {
        var saveData = new ItemContainerSaveData(itemSlots.Count);

        for (int i = 0; i < saveData.SavedSlots.Length; i++)
        {
            ItemSlot itemSlot = itemSlots[i];

            if (itemSlot.Item == null)
            {
                saveData.SavedSlots[i] = null;
            }
            else
            {
                if (itemSlot.Item is EquippableItem)
                {
                    saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount, (itemSlot.Item as EquippableItem).durability);
                }
                else
                {
                    saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount);
                }
            }
        }

        ItemSaveIO.SaveItems(saveData, fileName);
    }
}
