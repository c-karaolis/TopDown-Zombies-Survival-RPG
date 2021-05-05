/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Item.ItemViewModules
{
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.UI.CompoundElements;
    using UnityEngine;

    /// <summary>
    /// An Item View component to display an amount.
    /// </summary>
    public class AmountItemView : ItemViewModule
    {
        [Tooltip("The amount text.")]
        [SerializeField] protected Text m_AmountText;
        [Tooltip("Hide the game object selected if the amount is below or equal to this threshold.")]
        [SerializeField] protected float m_HideThreshold = 1;
        [Tooltip("The Game object to hide if the item amount is below or equal to the threshold.")]
        [SerializeField] protected GameObject m_DisableBelowThreshold;

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="info">The item info.</param>
        public override void SetValue(ItemInfo info)
        {
            if (m_DisableBelowThreshold != null) {
                m_DisableBelowThreshold.SetActive(info.Amount > m_HideThreshold);
            }

            m_AmountText.text = $"x {info.Amount}";
        }

        /// <summary>
        /// Clear the value.
        /// </summary>
        public override void Clear()
        {
            if (m_DisableBelowThreshold != null) {
                m_DisableBelowThreshold.SetActive(false);
            }
            m_AmountText.text = "";
        }
    }
}