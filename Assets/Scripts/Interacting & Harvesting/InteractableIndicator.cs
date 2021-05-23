using Foxlair.Character;
using Foxlair.Harvesting;
using Foxlair.Tools.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    IInteractable PlayerTargetInteractable;

    private void Start()
    {
        FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event += SetResourceNode;
        FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event += UnsetResourceNode;
    }

    private void UnsetResourceNode()
    {
        PlayerTargetInteractable = null;
    }

    private void SetResourceNode(IInteractable obj)
    {
        PlayerTargetInteractable = obj;
    }

    void Update()
    {
        if (PlayerTargetInteractable != null)
        {
            Bounds bounds = (PlayerTargetInteractable as MonoBehaviour).GetComponentInChildren<Renderer>().bounds;

            float diameter = bounds.size.z;
            diameter *= 2.50f;

            this.transform.position = new Vector3(bounds.center.x, 0.1f, bounds.center.z);
            this.transform.localScale = new Vector3(diameter, diameter, diameter);
        }
        else
        {
            this.transform.position = new Vector3(100000, 0.1f, 100000);
        }
    }


    private void OnDestroy()
    {
        FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeFound_Event -= SetResourceNode;
        FoxlairEventManager.Instance.InteractionSystem_OnResourceNodeLost_Event -= UnsetResourceNode;
    }
}
