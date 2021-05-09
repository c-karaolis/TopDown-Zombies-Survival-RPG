using Foxlair.Tools.Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : BaseItemSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{

	private bool isDragging;
	private Color dragColor = new Color(1, 1, 1, 0.5f);

	public override bool CanAddStack(Item item, int amount = 1)
	{
		return base.CanAddStack(item, amount) && Amount + amount <= item.MaximumStacks;
	}

	public override bool CanReceiveItem(Item item)
	{
		return true;
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		if (isDragging) {
			OnEndDrag(null);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;

		if (Item != null)
			image.color = dragColor;

		FoxlairEventManager.Instance.Inventory_OnBeginDrag_Event?.Invoke(this);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;

		if (Item != null)
			image.color = normalColor;

		FoxlairEventManager.Instance.Inventory_OnEndDrag_Event?.Invoke(this);
	}

	public void OnDrag(PointerEventData eventData)
	{
		FoxlairEventManager.Instance.Inventory_OnDrag_Event?.Invoke(this);
	}

	public void OnDrop(PointerEventData eventData)
	{
		FoxlairEventManager.Instance.Inventory_OnDrop_Event?.Invoke(this);
	}
}
