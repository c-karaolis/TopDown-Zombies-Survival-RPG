using Foxlair.Tools.Events;
using System;
using TMPro;
using UnityEngine;

namespace Foxlair.Character.LevelingSystem
{

    public class LevelingSystem : MonoBehaviour
    {
        public TextMeshProUGUI currentXPtext;
        public TextMeshProUGUI targetXPtext;
        public TextMeshProUGUI levelText;
        public int currentXP;
        public int targetXP;
        public int level = 1;
        private int experience;




        private void Start()
        {
            currentXPtext.text = currentXP.ToString();
            targetXPtext.text = targetXP.ToString();
            levelText.text = level.ToString();



        }



        public void AddExperience(int amount)
        {

            currentXP += amount;
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();

            currentXPtext.text = currentXP.ToString();

            while (currentXP >= targetXP)
            {
                currentXP = currentXP - targetXP;
                LevelUp();
                targetXP += targetXP / 20;
                FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();
            }

            currentXPtext.text = currentXP.ToString();
            targetXPtext.text = targetXP.ToString();
            // FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event?.Invoke();

            // FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();

        }

        private void LevelUp()
        {
            level++;
            //TODO: set this in a refresh UI method that also gets invoked through events?
            levelText.text = level.ToString();

            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event?.Invoke();
        }

        private void RefreshLevelingUI()
        {

        }

    }
}