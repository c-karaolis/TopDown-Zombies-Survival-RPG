﻿/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Item
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.Serialization;

    /// <summary>
    /// Item View Slot Drag Handler.
    /// </summary>
    public class ItemViewSlotDragHandler : MonoBehaviour
    {
        public event Action<ItemViewSlotPointerEventData> OnDragStarted;
        public event Action<ItemViewSlotPointerEventData> OnDragEnded;

        [FormerlySerializedAs("m_ItemViewSlotCursor")]
        [FormerlySerializedAs("m_ItemViewSlotMouseCursor")]
        [FormerlySerializedAs("m_ItemViewCursorManager")]
        [FormerlySerializedAs("m_ItemBoxCursorManager")]
        [Tooltip("The Item View cursor manager.")]
        [SerializeField] internal ItemViewSlotCursorManager m_ItemViewSlotCursorManager;
        [Tooltip("If true the slots with no items will not be able to be dragged.")]
        [SerializeField] internal bool m_DisableDragOnEmptySlot = true;

        protected ItemViewSlotsContainerBase m_ViewSlotsContainer;
        protected bool m_IsInitialized = false;

        public ItemViewSlotsContainerBase ViewSlotsContainer {
            get => m_ViewSlotsContainer;
            set => m_ViewSlotsContainer = value;
        }

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the grid.
        /// </summary>
        public virtual void Initialize()
        {
            if (m_IsInitialized) { return; }

            if (m_ItemViewSlotCursorManager == null) {
                m_ItemViewSlotCursorManager = GetComponentInParent<ItemViewSlotCursorManager>();
                if (m_ItemViewSlotCursorManager == null) {
                    Debug.LogWarning("The item view cursor manager is missing, please add one on your canvas.");
                }
            }

            m_ViewSlotsContainer = GetComponent<ItemViewSlotsContainerBase>();

            m_ViewSlotsContainer.OnItemViewSlotBeginDragE += HandleItemViewSlotBeginDrag;
            m_ViewSlotsContainer.OnItemViewSlotEndDragE += HandleItemViewSlotEndDrag;
            m_ViewSlotsContainer.OnItemViewSlotDragE += HandleItemViewSlotDrag;

            m_IsInitialized = true;
        }

        /// <summary>
        /// Condition to drag and item view.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        /// <returns>True if the drag can start.</returns>
        public bool DragEventCondition(ItemViewSlotPointerEventData eventData)
        {
            if (m_DisableDragOnEmptySlot && eventData.ItemViewSlot?.ItemInfo.Item == null) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handle the Item View slot beginning to drag.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        protected virtual void HandleItemViewSlotBeginDrag(ItemViewSlotPointerEventData eventData)
        {
            if(DragEventCondition(eventData) == false){return;}
            m_ItemViewSlotCursorManager.StartMove(eventData, eventData.PointerEventData.position);
            OnDragStarted?.Invoke(eventData);
        }

        /// <summary>
        /// Handle the Item View being dragged.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        protected virtual void HandleItemViewSlotDrag(ItemViewSlotPointerEventData eventData)
        {
            if(DragEventCondition(eventData) == false){return;}
            m_ItemViewSlotCursorManager.AddDeltaPosition(eventData.PointerEventData.delta);
        }

        /// <summary>
        /// Handle the item stopped the drag.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        protected virtual void HandleItemViewSlotEndDrag(ItemViewSlotPointerEventData eventData)
        {
            m_ItemViewSlotCursorManager.DragEnded();
            OnDragEnded?.Invoke(eventData);
        }
    }
}