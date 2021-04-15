using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedCustomizableSystem
{
    public class RootMotion : MonoBehaviour
    {
        Transform parentTransform;

        Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            GetRootObject();
        }
        public void GetRootObject()
        {
            parentTransform = transform;
            while (parentTransform.parent != null)
            {
                parentTransform = parentTransform.parent;
                if (parentTransform.GetComponent<LODGroup>() != null)
                    return;
            }
        }
        void OnAnimatorMove()
        {
            parentTransform.position += animator.deltaPosition;
            parentTransform.rotation = animator.rootRotation;
        }
    }
}