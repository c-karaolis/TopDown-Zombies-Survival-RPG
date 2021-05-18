using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Foxlair.Character.SurvivalSystem
{
    public class SurvivalSystem : MonoBehaviour
    {
        public int playerCharacterHealth;

        [Header("Player Hunger")]
        public float MaxHunger = 100f;
        public float Hunger = 0f;
        public float HungerRate;
        public Image HungerSlider;

        [Header("Player Thirst")]
        public float MaxThirst = 100f;
        public float Thirst = 0f;
        public float ThirstRate;
        public Image ThirstSlider;

        private void Update()
        {
            CalculateHunger();
            CalculateThirst();
        }

        public void Eat(float amount)
        {
            if (Hunger + amount <= MaxHunger)
            {
                Hunger += amount;
            }
            else
            {
                Hunger = MaxHunger;
            }
        }

        public void Drink(float amount)
        {
            if (Thirst + amount <= MaxThirst)
            {
                Thirst += amount;
            }
            else
            {
                Thirst = MaxThirst;
            }
        }

        private void CalculateThirst()
        {
            Thirst -= ThirstRate * Time.deltaTime;

            if (ThirstSlider == null) return;
            ThirstSlider.fillAmount = Thirst / MaxThirst;
        }

        private void CalculateHunger()
        {
            Hunger -= HungerRate * Time.deltaTime;

            if (HungerSlider == null) return;
            HungerSlider.fillAmount = Hunger / MaxHunger;
        }

    }
}