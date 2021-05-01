using Opsive.UltimateInventorySystem.Core;
using UnityEngine;

public class DynamicPanelOwner : MonoBehaviour
{
    [Tooltip("Use the Display Panel Manager ID to find the manager and set this game object as the panel owner.")]
    [SerializeField] protected uint m_DisplayPanelManagerID = 1;

    /// <summary>
    /// Initialize.
    /// </summary>
    private void Awake()
    {
        var displayPanelManager = InventorySystemManager.GetDisplayPanelManager(m_DisplayPanelManagerID);
        if (displayPanelManager == null) {
            return;
        }

        displayPanelManager.SetPanelOwner(gameObject);
    }
}
