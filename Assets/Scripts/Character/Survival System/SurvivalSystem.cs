using Foxlair.Tools.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Foxlair.Character.SurvivalSystem
{
    public class SurvivalSystem : MonoBehaviour
    {
        public int playerCharacterHealth;

        [Header("Player Hunger")]
        public float maxHunger = 100f;
        public float hunger = 0f;
        public float hungerRate;
        public Image hungerSlider;
        public TextMeshProUGUI hungerCounter;

        [Header("Player Thirst")]
        public float maxThirst = 100f;
        public float thirst = 0f;
        public float thirstRate;
        public Image thirstSlider;
        public TextMeshProUGUI thirstCounter;

        private void Start()
        {
            //if not saved
            hunger = maxHunger;
            thirst = maxThirst;

            SetHungerCounterText();
            SetThirstCounterText();
        }

        private void SetThirstCounterText()
        {
          //  if (thirst <= 0) return;

            thirstCounter.text = Mathf.Clamp(thirst, 0, maxThirst).ToString("F0");
        }

        private void SetHungerCounterText()
        {
           // if (hunger <= 0) return;
           
            hungerCounter.text = Mathf.Clamp(hunger, 0, maxHunger).ToString("F0");
        }

        private void Update()
        {
            CalculateHunger();
            CalculateThirst();
        }

        public void Eat(float amount)
        {
            if (hunger + amount <= maxHunger)
            {
                hunger += amount;
            }
            else
            {
                hunger = maxHunger;
            }
            FoxlairEventManager.Instance.SurvivalSystem_OnEat_Event?.Invoke();
        }

        public void Drink(float amount)
        {
            if (thirst + amount <= maxThirst)
            {
                thirst += amount;
            }
            else
            {
                thirst = maxThirst;
            }
            FoxlairEventManager.Instance.SurvivalSystem_OnDrink_Event?.Invoke();
        }

        private void CalculateThirst()
        {
            thirst -= thirstRate * Time.deltaTime;
            SetThirstCounterText();

            if (thirstSlider == null) return;
            thirstSlider.fillAmount = thirst / maxThirst;
        }

        private void CalculateHunger()
        {
            hunger -= hungerRate * Time.deltaTime;
            SetHungerCounterText();

            if (hungerSlider == null) return;
            hungerSlider.fillAmount = hunger / maxHunger;
        }

    }
}