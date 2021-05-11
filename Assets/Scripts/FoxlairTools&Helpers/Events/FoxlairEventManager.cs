using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
namespace Foxlair.Tools.Events
{
    public class FoxlairEventManager : PersistentSingletonMonoBehaviour<FoxlairEventManager>
    {

        #region Inventory Events
        public Action<BaseItemSlot> Inventory_OnRightClick_Event;
        public Action<BaseItemSlot> Inventory_OnPointerEnter_Event;
        public Action Inventory_OnPointerExit_Event;
        public Action<BaseItemSlot> Inventory_OnBeginDrag_Event;
        public Action<BaseItemSlot> Inventory_OnEndDrag_Event;
        public Action<BaseItemSlot> Inventory_OnDrag_Event;
        public Action<BaseItemSlot> Inventory_OnDrop_Event;
        #endregion

        public Action<EquippableItem> Player_OnItemEquipped_Event;
        public Action<EquippableItem> Player_OnItemUnEquipped_Event;

        #region DropItemArea Events
        public Action DropItemArea_OnDrop_Event;
        #endregion

        #region EquipmentPanel Events
        public Action<BaseItemSlot> EquipmentPanel_OnRightClick_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnPointerEnter_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnPointerExit_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnBeginDrag_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnEndDrag_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnDrag_Event;
        public Action<BaseItemSlot> EquipmentPanel_OnDrop_Event;
        #endregion

        #region StatPanel Events
        public Action StatPanel_OnValuesUpdated_Event;
        #endregion

        #region CraftingWindow Events
        public Action<BaseItemSlot> CraftingWindow_OnPointerEnter_Event;
        public Action<BaseItemSlot> CraftingWindow_OnPointerExit_Event;
        #endregion

        #region Currency Events
        public Action Currency_OnCurrencyChanged_Event;
        #endregion

    }


}