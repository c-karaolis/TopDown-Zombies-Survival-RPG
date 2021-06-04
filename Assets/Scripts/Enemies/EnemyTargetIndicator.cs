using Foxlair.Character;
using Foxlair.Enemies;
using Foxlair.Tools.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetIndicator : MonoBehaviour
{
    EnemyCharacter PlayerTargetEnemy;

    private void Start()
    {
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event += SetTarget;
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event += UnsetTarget;
    }

    private void UnsetTarget()
    {
        PlayerTargetEnemy = null;
    }

    private void SetTarget(EnemyCharacter obj)
    {
        PlayerTargetEnemy = obj;
    }

    void Update()
    {
        if (PlayerTargetEnemy != null)
        {
            Bounds bounds = PlayerTargetEnemy.GetComponentInChildren<Renderer>().bounds;

            float diameter = bounds.size.z;
            diameter *= 2.50f;

            this.transform.position = new Vector3(bounds.center.x, 0.5f, bounds.center.z);
            this.transform.localScale = new Vector3(diameter, diameter, diameter);
        }
        else
        {
            this.transform.position = new Vector3(0, -100f, 0);
        }
    }

    private void OnDestroy()
    {
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event -= SetTarget;
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event -= UnsetTarget;
    }
}
