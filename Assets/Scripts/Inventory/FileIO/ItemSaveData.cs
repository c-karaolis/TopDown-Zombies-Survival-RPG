using System;

[Serializable]
public class ItemSlotSaveData
{
	public string ItemID;
	public int Amount;
	public int Durability;
	public ItemSlotSaveData(string id, int amount, int durability = 0)
	{
		ItemID = id;
		Amount = amount;
		Durability = durability;
	}
}

[Serializable]
public class ItemContainerSaveData
{
	public ItemSlotSaveData[] SavedSlots;

	public ItemContainerSaveData(int numItems)
	{
		SavedSlots = new ItemSlotSaveData[numItems];
	}
}
