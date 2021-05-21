using System;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
	public EquipmentSlot[] EquipmentSlots;
	[SerializeField] Transform equipmentSlotsParent;


	public event Action<BaseItemSlot> OnRightClickEvent;

	private void Start()
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			EquipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
		}
	}

	private void OnValidate()
	{
		EquipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
	}

	public bool AddItem(EquippableItem item, out EquippableItem previousItem)
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			if (EquipmentSlots[i].EquipmentType == item.EquipmentType)
			{
				//TODO: if weapon send equipped event
				previousItem = (EquippableItem)EquipmentSlots[i].Item;
				EquipmentSlots[i].Item = item;
				EquipmentSlots[i].Amount = 1;
				return true;
			}
		}
		previousItem = null;
		return false;
	}

	public bool RemoveItem(EquippableItem item)
	{
		for (int i = 0; i < EquipmentSlots.Length; i++)
		{
			if (EquipmentSlots[i].Item == item)
			{
				//TODO: if weapon send unequipped event
				EquipmentSlots[i].Item = null;
				EquipmentSlots[i].Amount = 0;
				return true;
			}
		}
		return false;
	}
}
