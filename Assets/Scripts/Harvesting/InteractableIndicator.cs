using Foxlair.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    void Update()
    {
        if (PlayerManager.Instance.PlayerTargetResourceNode != null)
        {
            Bounds bounds = PlayerManager.Instance.PlayerTargetResourceNode.GetComponentInChildren<Renderer>().bounds;

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
}
