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



        private void Start()
        {
            currentExperienceText.text = currentXP.ToString();
            targetExperienceText.text = targetXP.ToString();
            currentLevelText.text = currentLevel.ToString();


            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event += RefreshLevelingUI;
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChangedAmount_Event += FloatingExperienceGainedText;
            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event += RefreshLevelingUI;
        }



        public void AddExperience(int amount)
        {

            currentXP += amount;

            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChangedAmount_Event?.Invoke(amount);

            while (currentXP >= targetXP)
            {
                currentXP -= targetXP;
                LevelUp();
                switch (currentLevel)
                {
                    case int n when n < 10:
                        targetXP += (int)Math.Round((float)targetXP * 0.5);
                        break;

                    case int n when n < 20:
                        targetXP += (int)Math.Round((float)targetXP * 0.3);
                        break;

                    case int n when n < 50:
                        targetXP += (int)Math.Round((float)targetXP * 0.05);
                        break;

                    case int n when n < 80:
                        targetXP += (int)Math.Round((float)targetXP * 0.02);
                        break;

                    default:
                        targetXP += (int)Math.Round((float)targetXP * 0.002);
                        break;
                }
                //targetXP += (int)Math.Round((float)targetXP * 0.09);
                FoxlairEventManager.Instance.LevelingSystem_OnExperienceChanged_Event?.Invoke();
            }
           
        }

        private void LevelUp()
        {
            currentLevel++;
         
            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event?.Invoke();
        }

        private void FloatingLevelUpText()
        {

        }

        private void FloatingExperienceGainedText(int xpGained)
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
            FoxlairEventManager.Instance.LevelingSystem_OnExperienceChangedAmount_Event -= FloatingExperienceGainedText;
            FoxlairEventManager.Instance.LevelingSystem_OnLevelChanged_Event -= RefreshLevelingUI;
        }
    }
}