using Foxlair.PlayerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMobileInputHandling : MonoBehaviour
{

    bool isFiringButtonPressed;
    bool isInteractionButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region Interaction button
    public void onInteractButtonPress()
    {
        isInteractionButtonPressed = true;
        InputHandler.Instance.IsInteractionButtonDown = isInteractionButtonPressed;
    }

    public void onInteractButtonRelease()
    {
        isInteractionButtonPressed = false;
        InputHandler.Instance.IsInteractionButtonDown = isInteractionButtonPressed;
    }
    #endregion

    #region Fire button
    public void onFireButtonPress()
    {
        isFiringButtonPressed=true;
        InputHandler.Instance.IsFiringButtonDown = isFiringButtonPressed;

    }

    public void onFireButtonRelease()
    {
        isFiringButtonPressed=false;
        InputHandler.Instance.IsFiringButtonDown = isFiringButtonPressed;

    }
    #endregion

}
