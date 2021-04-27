using Foxlair.Tools.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Foxlair.Inventory
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ItemDragHandler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IDragHandler, IPointerUpHandler,IPointerExitHandler
    {
        [SerializeField] protected ItemSlotUI itemSlotUI = null;

        private CanvasGroup canvasGroup = null;
        private Transform originalParent = null;
        private bool isHovering = false;


        public ItemSlotUI ItemSlotUI => itemSlotUI;

        private void Start() => canvasGroup = GetComponent<CanvasGroup>();

        private void OnDisable()
        {
            if (isHovering)
            {
                FoxlairEventManager.Instance.onMouseEndHoverItem();
                //onMouseEndHoverItem.Raise();
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
            FoxlairEventManager.Instance.onMouseStartHoverItem(ItemSlotUI.SlotItem);
            //onMouseStartHoverItem.Raise(ItemSlotUI.SlotItem);
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            FoxlairEventManager.Instance.onMouseEndHoverItem();
            //onMouseEndHoverItem.Raise();
            isHovering = false;
        }

    }
}
