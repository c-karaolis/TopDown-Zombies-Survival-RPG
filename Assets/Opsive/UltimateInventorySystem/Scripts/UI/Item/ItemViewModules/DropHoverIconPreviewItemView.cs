﻿/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Item.ItemViewModules
{
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.UI.Item.DragAndDrop;
    using Opsive.UltimateInventorySystem.UI.Views;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A Item View UI component that lets you bind an icon to the item icon attribute.
    /// </summary>
    public class DropHoverIconPreviewItemView : ItemViewModule, IItemViewSlotDropHoverSelectable, IViewModuleSelectable
    {
        [Tooltip("The icon image.")]
        [SerializeField] protected Image m_ItemIcon;
        [Tooltip("The icon image.")]
        [SerializeField] protected Image m_ColorFilter;
        [Tooltip("The color when not condition have passed.")]
        [SerializeField] protected Color m_NoConditionsPassed;
        [Tooltip("The preview color when at least one condition has passed.")]
        [SerializeField] protected Color m_ConditionsPassed;

        /// <summary>
        /// Set the item info.
        /// </summary>
        /// <param name="info">The item .</param>
        public override void SetValue(ItemInfo info)
        {
            if (info.Item == null || info.Item.IsInitialized == false) {
                Clear();
                return;
            }
        }

        /// <summary>
        /// Clear the component.
        /// </summary>
        public override void Clear()
        {
            m_ItemIcon.enabled = false;
            m_ColorFilter.enabled = false;
        }

        /// <summary>
        /// Select with a drop handler.
        /// </summary>
        /// <param name="dropHandler">The drop handler.</param>
        public virtual void SelectWith(ItemViewDropHandler dropHandler)
        {
            var dropIndex =
                dropHandler.ItemViewSlotDropActionSet.GetFirstPassingConditionIndex(dropHandler);
            m_ColorFilter.color = dropIndex == -1 ? m_NoConditionsPassed : m_ConditionsPassed;

            var sourceItemInfo = dropHandler.SlotCursorManager.SourceItemViewSlot.ItemInfo;

            PreviewIcon(sourceItemInfo);

            var sourceBoxModules = dropHandler.SlotCursorManager.SourceItemViewSlot.ItemView.Modules;

            for (int i = 0; i < sourceBoxModules.Count; i++) {
                if (sourceBoxModules[i] is DropHoverIconPreviewItemView sourcePreviewModules) {

                    sourcePreviewModules.PreviewIcon(ItemInfo);
                }
            }

            m_ColorFilter.enabled = true;
        }

        /// <summary>
        /// Preview the icon.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        protected virtual void PreviewIcon(ItemInfo itemInfo)
        {
            if (itemInfo.Item != null && itemInfo.Item.TryGetAttributeValue<Sprite>("Icon", out var icon)) {
                m_ItemIcon.sprite = icon;
                m_ItemIcon.enabled = true;
                return;
            }

            m_ItemIcon.enabled = false;
        }

        /// <summary>
        /// Deselect with the item view drop handler.
        /// </summary>
        /// <param name="dropHandler">The drop handler.</param>
        public virtual void DeselectWith(ItemViewDropHandler dropHandler)
        {
            if (dropHandler.SlotCursorManager.SourceItemViewSlot == m_View) {
                m_ItemIcon.gameObject.SetActive(true);
                return;
            }

            Select(false);
        }

        /// <summary>
        /// Simple select/deselect.
        /// </summary>
        /// <param name="select">Select?</param>
        public virtual void Select(bool select)
        {
            if (select) { return; }
            if (ItemInfo.Item == null || ItemInfo.Item.IsInitialized == false) {
                Clear();
                return;
            }

            m_ColorFilter.enabled = false;
            if (ItemInfo.Item.TryGetAttributeValue<Sprite>("Icon", out var icon)) {
                m_ItemIcon.sprite = icon;
                m_ItemIcon.enabled = true;
                return;
            }

            m_ItemIcon.enabled = false;
        }
    }
}