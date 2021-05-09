using Foxlair.Tools.Events;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
	[SerializeField] Text ItemNameText;
	[SerializeField] Text ItemTypeText;
	[SerializeField] Text ItemDescriptionText;

    private void Start()
    {
		SubscribeToEvents();
		DisableTooltipOnStart();
	}

	private void OnDestroy()
    {
		UnsubscribeFromEvents();
    }
    public void ShowTooltip(BaseItemSlot itemSlot)
	{
		if (itemSlot == null) return;

		ItemNameText.text = itemSlot.Item.ItemName;
		ItemTypeText.text = itemSlot.Item.GetItemType();
		ItemDescriptionText.text = itemSlot.Item.GetDescription();
		gameObject.SetActive(true);
	}

	public void HideTooltip()
	{
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
	}
	private void DisableTooltipOnStart()
	{
		gameObject.SetActive(false);
	}

	private void SubscribeToEvents()
    {
		Debug.Log("show");

		// Pointer Enter
		FoxlairEventManager.Instance.Inventory_OnPointerEnter_Event += ShowTooltip;
		// Pointer Exit
		FoxlairEventManager.Instance.Inventory_OnPointerExit_Event += HideTooltip;
	}

	private void UnsubscribeFromEvents()
	{
		// Pointer Enter
		FoxlairEventManager.Instance.Inventory_OnPointerEnter_Event -= ShowTooltip;
		// Pointer Exit
		FoxlairEventManager.Instance.Inventory_OnPointerExit_Event -= HideTooltip;
	}
}
