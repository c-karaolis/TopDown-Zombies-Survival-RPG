using Foxlair.Tools.Events;
using System;
using TMPro;
using UnityEngine;

namespace Foxlair.Character.LevelingSystem
{

    public class LevelingSystem : MonoBehaviour
    {
        public TextMeshProUGUI currentExperienceText;
        public TextMeshProUGUI targetExperienceText;
        public TextMeshProUGUI currentLevelText;
        public int currentXP;
        public int targetXP;
        public int currentLevel = 1;
        private int experience;




        private void Start()
        {
            currentExperienceText.text = currentXP.ToString();
            targetExperienceText.text = targetXP.ToString();
            currentLevelText.text = currentLevel.ToString();


            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event += RefreshLevelingUI;
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event += FloatingExperienceGainedText;
            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event += RefreshLevelingUI;
        }



        public void AddExperience(int amount)
        {

            currentXP += amount;
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();

            while (currentXP >= targetXP)
            {
                currentXP = currentXP - targetXP;
                LevelUp();
                targetXP += targetXP / 20;
                FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();
            }

            
            // FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event?.Invoke();

            // FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();

        }

        private void LevelUp()
        {
            currentLevel++;
            //TODO: set this in a refresh UI method that also gets invoked through events?

            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event?.Invoke();
        }

        private void FloatingLevelUpText()
        {

        }

        private void FloatingExperienceGainedText()
        {

        }

        private void RefreshLevelingUI()
        {

            currentLevelText.text = currentLevel.ToString();
            currentExperienceText.text = currentXP.ToString();
            targetExperienceText.text = targetXP.ToString();

        }


        private void OnDestroy()
        {
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event -= RefreshLevelingUI;
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event -= FloatingExperienceGainedText;
            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event -= RefreshLevelingUI;
        }
    }
}