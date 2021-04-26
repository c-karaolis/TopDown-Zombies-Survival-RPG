using Foxlair.Tools.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Foxlair.Currencies
{
    public class Wallet:MonoBehaviour
    {
        [SerializeField]private  Currency _coins;
        [SerializeField]private  PremiumCurrency _platinumBars;

        [SerializeField] TextMeshProUGUI goldText;
        [SerializeField] TextMeshProUGUI silverText;
        [SerializeField] TextMeshProUGUI copperText;

        //public Wallet(long coins, int platinumBars)
        //{
        //    _coins = new Currency(coins);
        //    _platinumBars = new PremiumCurrency(platinumBars);
        //}
        private void Start()
        {
            _coins = new Currency();
            _platinumBars = new PremiumCurrency();
            FoxlairEventManager.Instance.OnCurrencyChanged += UpdateCurrencyText;

            FoxlairEventManager.Instance.onCurrencyChanged();
        }

        public Currency Currency { get { return _coins; }  }

        public PremiumCurrency PremiumCurrency { get { return _platinumBars; } }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            FoxlairEventManager.Instance.OnCurrencyChanged -= UpdateCurrencyText;
        }

        private void UpdateCurrencyText()
        {
            goldText.text = Currency.Gold.ToString();
            silverText.text = Currency.Silver.ToString();
            copperText.text = Currency.Copper.ToString();
        }
    }
}