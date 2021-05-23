using Foxlair.PlayerInput;
using Foxlair.Tools.Events;
using Foxlair.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMobileInputHandling : MonoBehaviour
{
    public Sprite DefaultPunchImage;

    public Image AttackButtonImage;
    public Image InteractButtonImage;


    bool isFiringButtonPressed;
    bool isInteractionButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event += AddWeaponSpriteToAttackButton;
        FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event += AddPunchSpriteToAttackButton;
        FoxlairEventManager.Instance.WeaponSystem_OnEquippedWeaponDestroyed_Event += AddPunchSpriteToAttackButton;
        //FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event += AddInteractSpriteToInteractionButton;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void AddPunchSpriteToAttackButton(Weapon obj)
    {
        AddPunchSpriteToAttackButton();
    }
    private void AddPunchSpriteToAttackButton()
    {
        AttackButtonImage.sprite = DefaultPunchImage;
    }

    private void AddWeaponSpriteToAttackButton(Weapon obj)
    {
        AttackButtonImage.sprite = obj.InventoryItemInstance.Icon;
    }

    #region Interaction button
    public void OnInteractButtonPress()
    {
        isInteractionButtonPressed = true;
        InputHandler.Instance.IsInteractionButtonDown = isInteractionButtonPressed;
    }

    public void OnInteractButtonRelease()
    {
        isInteractionButtonPressed = false;
        InputHandler.Instance.IsInteractionButtonDown = isInteractionButtonPressed;
    }
    #endregion

    #region Fire button
    public void OnFireButtonPress()
    {
        isFiringButtonPressed=true;
        InputHandler.Instance.IsFiringButtonDown = isFiringButtonPressed;

    }

    public void OnFireButtonRelease()
    {
        isFiringButtonPressed=false;
        InputHandler.Instance.IsFiringButtonDown = isFiringButtonPressed;

    }
    #endregion

    private void OnDestroy()
    {
        FoxlairEventManager.Instance.WeaponSystem_OnWeaponEquipped_Event -= AddWeaponSpriteToAttackButton;
        FoxlairEventManager.Instance.WeaponSystem_OnWeaponUnEquipped_Event -= AddPunchSpriteToAttackButton;
        FoxlairEventManager.Instance.WeaponSystem_OnEquippedWeaponDestroyed_Event -= AddPunchSpriteToAttackButton;
    }
}
