﻿using System;
using Foxlair.Weapons;
using Foxlair.Enemies;
using Foxlair.Achievements;

namespace Foxlair.Tools.Events
{
    public class FoxlairEventManager : PersistentSingletonMonoBehaviour<FoxlairEventManager>
    {

        #region Achievements
        public Action<Achievement> Achievements_Achieved_Event;
        #endregion

        #region Currency Events
        public Action Currency_OnCurrencyChanged_Event;
        #endregion

        #region CraftingWindow Events
        public Action<BaseItemSlot> CraftingWindow_OnPointerEnter_Event;
        public Action<BaseItemSlot> CraftingWindow_OnPointerExit_Event;
        #endregion

        #region DropItemArea Events
        public Action DropItemArea_OnDrop_Event;
        #endregion

        #region Enemy Health System
        public Action<float> EnemyHealthSystem_OnHealthGained_Event;
        public Action<float> EnemyHealthSystem_OnHealthLost_Event;
        public Action<EnemyCharacter> EnemyHealthSystem_OnDeath_Event;
        #endregion

        #region EquipmentPanel Events
        //public Action<BaseItemSlot> EquipmentPanel_OnRightClick_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnPointerEnter_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnPointerExit_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnBeginDrag_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnEndDrag_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnDrag_Event;
        //public Action<BaseItemSlot> EquipmentPanel_OnDrop_Event;
        #endregion

        #region Interaction, Targeting & Harvest System
        public Action<IInteractable> InteractionSystem_OnResourceNodeFound_Event;
        public Action InteractionSystem_OnResourceNodeLost_Event;

        public Action<EnemyCharacter> TargetingSystem_OnTargetEnemyAcquired_Event;
        public Action TargetingSystem_OnTargetEnemyLost_Event;
        #endregion

        #region Inventory Events
        public Action<BaseItemSlot> Inventory_OnRightClick_Event;
        public Action<BaseItemSlot> Inventory_OnPointerEnter_Event;
        public Action Inventory_OnPointerExit_Event;
        public Action<BaseItemSlot> Inventory_OnBeginDrag_Event;
        public Action<BaseItemSlot> Inventory_OnEndDrag_Event;
        public Action<BaseItemSlot> Inventory_OnDrag_Event;
        public Action<BaseItemSlot> Inventory_OnDrop_Event;

        //public Action<EquippableItem> Player_OnItemEquipped_Event;
        //public Action<EquippableItem> Player_OnItemUnEquipped_Event;

        public Action Inventory_OnItemsChanged_Event;
        #endregion

        #region Leveling System
        public Action LevelingSystem_OnExperienceChanged_Event;
        public Action<int> LevelingSystem_OnExperienceChangedAmount_Event;
        public Action LevelingSystem_OnLevelChanged_Event;
        #endregion

        #region Player Health System
        public Action<float> PlayerHealthSystem_OnHealthGained_Event;
        public Action<float> PlayerHealthSystem_OnHealthLost_Event;
        public Action PlayerHealthSystem_OnPlayerDeath_Event;
        #endregion

        #region StatPanel Events
        public Action StatPanel_OnValuesUpdated_Event;
        #endregion

        #region Survival System
        public Action SurvivalSystem_OnEat_Event;
        public Action SurvivalSystem_OnDrink_Event;
        #endregion

        #region Weapon System
        public Action<Weapon> WeaponSystem_OnWeaponEquipped_Event;
        public Action<Weapon> WeaponSystem_OnWeaponUnEquipped_Event;
        public Action WeaponSystem_OnEquippedWeaponDestroyed_Event;
        #endregion

    }


}