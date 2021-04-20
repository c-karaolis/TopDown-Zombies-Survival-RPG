using Foxlair.Events.CustomEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxlair.Inventory
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IDragHandler, IPointerUpHandler,IPointerExitHandler
    {
        [SerializeField] protected ItemSlotUI itemSlotUI = null;
        [SerializeField] protected HotbarItemEvent onMouseStartHoverItem = null;
        [SerializeField] protected VoidEvent onMouseEndHoverItem = null; 


        private CanvasGroup canvasGroup = null;
        private Transform originalParent = null;
        private bool isHovering = false;


        public ItemSlotUI ItemSlotUI => itemSlotUI;

        private void Start() => canvasGroup = GetComponent<CanvasGroup>();

        private void OnDisable()
        {
            if (isHovering)
            {
                onMouseEndHoverItem.Raise();
                isHovering = false;
            }
        }


        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                //onMouseEndHoverItem.Raise();

                originalParent = transform.parent;

                transform.SetParent(transform.parent.parent);

                canvasGroup.blocksRaycasts = false;
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                transform.position = Input.mousePosition;
            }
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"Pointer entered at slot: {ItemSlotUI.SlotItem}");
            onMouseStartHoverItem.Raise(ItemSlotUI.SlotItem);
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log($"Pointer exited from slot: {ItemSlotUI.SlotItem}");
            onMouseEndHoverItem.Raise();
            isHovering = false;
        }

    }
}
