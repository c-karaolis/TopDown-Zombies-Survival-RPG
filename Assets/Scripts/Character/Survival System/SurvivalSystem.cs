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
        public const float MaxHunger = 100f;
        public float Hunger = 0f;
        public float HungerRate;
        public Image HungerSlider; 

        [Header("Player Thirst")]
        public const float MaxThirst = 100f;
        public float Thirst = 0f;
        public float ThirstRate;
        public Image ThirstSlider;

        private void Update()
        {
            CalculateHunger();
            CalculateThirst();
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