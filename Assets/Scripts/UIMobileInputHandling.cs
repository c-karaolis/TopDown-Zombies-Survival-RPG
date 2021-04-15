using Foxlair.PlayerInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMobileInputHandling : MonoBehaviour
{

    bool isFiringButtonPressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
