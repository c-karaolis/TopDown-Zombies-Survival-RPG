using Foxlair.Enemies;
using Foxlair.Tools.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTargetUI : MonoBehaviour
{
    EnemyCharacter enemyCharacter = null;
    public Image enemyHealthbar;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nameText;
    public Image healthTextBackground;

    void Start()
    {
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event += SetEnemyTarget;
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event += UnsetEnemyTarget;
    }

    void Update()
    {
        if (enemyCharacter == null) { return; }

        enemyHealthbar.fillAmount = enemyCharacter.healthSystem.health / enemyCharacter.healthSystem.maxHealth;
        healthText.text = enemyCharacter.healthSystem.health.ToString("#");
    }


    private void SetEnemyTarget(EnemyCharacter receivedEnemyCharacter)
    {
        enemyCharacter = receivedEnemyCharacter;
        nameText.text = enemyCharacter.enemyName;
        ShowUI();
    }

    private void ShowUI()
    {
        Debug.Log("SHOW UI");
        enemyHealthbar.enabled = true;
        nameText.enabled = true;
        healthText.enabled = true;
        healthTextBackground.enabled = true;
    }

    private void UnsetEnemyTarget()
    {
        enemyCharacter = null;
        HideUI();
    }

    private void HideUI()
    {
        enemyHealthbar.enabled = false;
        nameText.enabled = false;
        healthText.enabled = false;
        healthTextBackground.enabled = false;
    }

    private void OnDestroy()
    {
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyAcquired_Event -= SetEnemyTarget;
        FoxlairEventManager.Instance.TargetingSystem_OnTargetEnemyLost_Event -= UnsetEnemyTarget;
    }
}
