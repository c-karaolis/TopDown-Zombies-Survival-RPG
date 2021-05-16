using Foxlair.Tools.Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropItemArea : MonoBehaviour, IDropHandler
{
	public void OnDrop(PointerEventData eventData)
	{
		FoxlairEventManager.Instance.DropItemArea_OnDrop_Event?.Invoke();
	}
}
