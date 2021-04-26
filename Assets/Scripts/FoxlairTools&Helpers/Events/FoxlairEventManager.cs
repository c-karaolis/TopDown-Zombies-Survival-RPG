using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Foxlair.Inventory;

namespace Foxlair.Tools.Events
{
    public class FoxlairEventManager : PersistentSingletonMonoBehaviour<FoxlairEventManager>
    {

        #region Inventory Events
        public event Action OnMouseEndHoverItem;
        public event Action<Item> OnMouseStartHoverItem;
        public event Action OnInventoryItemsUpdated;

        public void onMouseStartHoverItem(Item item) => OnMouseStartHoverItem?.Invoke(item);
        public void onMouseEndHoverItem() => OnMouseEndHoverItem?.Invoke();
        public void onInventoryItemsUpdated() => OnInventoryItemsUpdated?.Invoke();
        #endregion

        #region Currency Events
        public event Action OnCurrencyChanged;
        public void onCurrencyChanged() => OnCurrencyChanged?.Invoke();
        #endregion

    }

    /// <summary>
    /// Event listener basic interface
    /// </summary>
    public interface FoxlairEventListenerBase { };

    /// <summary>
    /// A public interface you'll need to implement for each type of event you want to listen to.
    /// </summary>
    public interface FoxlairEventListener<T> : FoxlairEventListenerBase
    {
        void OnFoxlairEvent(T eventType);
    }
}