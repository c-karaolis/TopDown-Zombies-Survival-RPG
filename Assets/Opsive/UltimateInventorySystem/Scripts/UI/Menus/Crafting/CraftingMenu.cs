/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Menus.Crafting
{
    using Opsive.UltimateInventorySystem.Core.InventoryCollections;
    using Opsive.UltimateInventorySystem.Crafting;
    using Opsive.UltimateInventorySystem.UI.Panels;
    using Opsive.UltimateInventorySystem.UI.Panels.Crafting;
    using Opsive.UltimateInventorySystem.UI.Panels.ItemViewSlotContainers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The Crafting menu.
    /// </summary>
    public class CraftingMenu : InventoryPanelBinding
    {
        [Tooltip("The crafter has the list of available recipes to craft.")]
        [SerializeField] internal Crafter m_Crafter;
        [Tooltip("The recipes display.")]
        [SerializeField] internal CraftingRecipeGrid m_CraftingRecipeGrid;
        [Tooltip("The selected recipe panel.")]
        [SerializeField] internal RecipePanel m_RecipePanel;
        [Tooltip("The quantity picker panel.")]
        [SerializeField] internal QuantityPickerPanel m_QuantityPickerPanel;
        [Tooltip("The Exit button.")]
        [SerializeField] protected Button m_ExitButton;
        [Tooltip("Draw recipes on open.")]
        [SerializeField] protected bool m_DrawRecipesOnOpen = true;

        private CraftingRecipe m_SelectedRecipe;

        /// <summary>
        /// Initialize the Crafting Menu.
        /// </summary>
        /// <param name="display">The display.</param>
        /// <param name="force">Force the initialize.</param>
        public override void Initialize(DisplayPanel display, bool force)
        {
            var wasInitialized = m_IsInitialized;
            if (wasInitialized && !force) { return; }
            base.Initialize(display, force);

            if (wasInitialized == false) {
                //Only do it once even if forced.
                if (m_Inventory == null) {
                    m_Inventory = GameObject.FindWithTag("Player")?.GetComponent<Inventory>();
                }

                if (m_Crafter != null) { m_Crafter.Initialize(false); }

                m_CraftingRecipeGrid.SetParentPanel(m_DisplayPanel);
                m_CraftingRecipeGrid.Initialize(false);

                m_CraftingRecipeGrid.OnElementSelected += CraftingRecipeSelected;
                m_CraftingRecipeGrid.OnEmptySelected += (x) => CraftingRecipeSelected(null, x);
                m_CraftingRecipeGrid.OnElementClicked += CraftingRecipeClicked;

                m_QuantityPickerPanel.OnAmountChanged += CraftingAmountChanged;
                m_QuantityPickerPanel.ConfirmCancelPanel.OnConfirm += CraftSelectedQuantity;

                if (m_ExitButton != null) {
                    m_ExitButton.onClick.AddListener(() => m_DisplayPanel.Close(true));
                }

                var tabControl = m_CraftingRecipeGrid.TabControl;

                if (tabControl != null) {
                    tabControl.Initialize(false);
                    tabControl.OnTabChange += HandleTabChange;

                    for (int i = 0; i < tabControl.TabCount; i++) {
                        var tab = tabControl.TabToggles[i];
                        var craftingTabData = tab.GetComponent<CraftingTabData>();
                        if (craftingTabData != null) {
                            craftingTabData.Initialize(false);
                        }
                    }

                    HandleTabChange(-1, tabControl.TabIndex, false);
                }
            }
        }

        /// <summary>
        /// Set the crafter.
        /// </summary>
        /// <param name="crafter">The crafter to set.</param>
        public void SetCrafter(Crafter crafter)
        {
            m_Crafter = crafter;
            m_Crafter.Initialize(false);
            DrawRecipes();
        }

        /// <summary>
        /// Set the inventory.
        /// </summary>
        protected override void OnInventoryBound()
        {
            m_RecipePanel.SetInventory(m_Inventory);
        }

        /// <summary>
        /// Handle the On Open event.
        /// </summary>
        public override void OnOpen()
        {
            base.OnOpen();

            m_RecipePanel.SetInventory(m_Inventory);

            if (m_DrawRecipesOnOpen) {
                var tabControl = m_CraftingRecipeGrid.TabControl;
                if (tabControl != null) {
                    HandleTabChange(-1, tabControl.TabIndex, true);
                } else {
                    DrawRecipes();
                }
            }

            m_CraftingRecipeGrid.SelectButton(0);
        }

        /// <summary>
        /// Refresh the display.
        /// </summary>
        protected void DrawRecipes()
        {
            m_CraftingRecipeGrid.SetElements(m_Crafter.GetRecipes());
            m_CraftingRecipeGrid.Draw();
        }

        /// <summary>
        /// Handle a tab change.
        /// </summary>
        /// <param name="previousIndex">The previous tab index.</param>
        /// <param name="newIndex">The new tab index.</param>
        private void HandleTabChange(int previousIndex, int newIndex)
        {
            HandleTabChange(previousIndex, newIndex, true);
        }

        /// <summary>
        /// Handle the tab change.
        /// </summary>
        /// <param name="previousIndex">The previous tab index.</param>
        /// <param name="newIndex">The new tab index.</param>
        /// <param name="draw">Should the recipes be drawn?</param>
        private void HandleTabChange(int previousIndex, int newIndex, bool draw)
        {
            if (previousIndex == newIndex) { return; }

            var craftingTabData = m_CraftingRecipeGrid.TabControl.CurrentTab.GetComponent<CraftingTabData>();

            if (craftingTabData == null) {
                Debug.LogWarning("The selected tab is either null or does not have an CraftingTabData", gameObject);
                return;
            }

            if (craftingTabData.CraftingFilter != null) {
                m_CraftingRecipeGrid.BindGridFilterSorter(craftingTabData.CraftingFilter);
            }

            if (draw) {
                DrawRecipes();
            }
        }

        /// <summary>
        /// update when the crafting amount changes.
        /// </summary>
        /// <param name="amount">The new amount.</param>
        private void CraftingAmountChanged(int amount)
        {
            var canCraft = m_Crafter.Processor.CanCraft(m_SelectedRecipe, m_Inventory, amount);
            if (canCraft == false) {
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = amount;
                m_QuantityPickerPanel.ConfirmCancelPanel.EnableConfirm(false);
            } else {
                m_QuantityPickerPanel.QuantityPicker.MaxQuantity = amount + 1;
                m_QuantityPickerPanel.ConfirmCancelPanel.EnableConfirm(true);
            }

            m_RecipePanel.SetQuantity(amount);
            m_RecipePanel.Refresh();
        }

        /// <summary>
        /// A recipe is selected.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        /// <param name="index">The index.</param>
        private void CraftingRecipeSelected(CraftingRecipe recipe, int index)
        {
            m_RecipePanel.SetRecipe(recipe);

            if (m_QuantityPickerPanel.IsOpen == false) { return; }
            if (m_SelectedRecipe == recipe) { return; }

            m_SelectedRecipe = recipe;
            m_QuantityPickerPanel.SetPreviousSelectable(m_CraftingRecipeGrid.GetButton(index));
            m_QuantityPickerPanel.QuantityPicker.MinQuantity = 1;
            m_QuantityPickerPanel.QuantityPicker.MaxQuantity = 2;

            m_QuantityPickerPanel.ConfirmCancelPanel.SetConfirmText("Craft");
            m_QuantityPickerPanel.QuantityPicker.SetQuantity(1);
            CraftingAmountChanged(1);
        }

        /// <summary>
        /// Recipe is clicked.
        /// </summary>
        /// <param name="recipe">The recipe.</param>
        /// <param name="index">The index.</param>
        private void CraftingRecipeClicked(CraftingRecipe recipe, int index)
        {
            m_SelectedRecipe = recipe;

            m_QuantityPickerPanel.Open(m_DisplayPanel, m_CraftingRecipeGrid.GetButton(index));

            m_QuantityPickerPanel.QuantityPicker.MinQuantity = 1;
            m_QuantityPickerPanel.QuantityPicker.MaxQuantity = 2;

            m_QuantityPickerPanel.ConfirmCancelPanel.SetConfirmText("Craft");
            m_QuantityPickerPanel.QuantityPicker.SetQuantity(1);
            CraftingAmountChanged(1);
        }

        /// <summary>
        /// Wait for the player to select a quantity.
        /// </summary>
        /// <returns>The task.</returns>
        private void CraftSelectedQuantity()
        {
            var quantity = m_QuantityPickerPanel.QuantityPicker.Quantity;

            if (quantity >= 1) {
                m_Crafter.Processor.Craft(m_SelectedRecipe, m_Inventory, quantity);
            }

            m_RecipePanel.SetQuantity(1);
            m_RecipePanel.Refresh();
        }
    }
}
