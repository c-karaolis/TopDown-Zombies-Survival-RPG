using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Currencies
{
    public class Wallet
    {
        private readonly Currency _coins;
        private readonly PremiumCurrency _platinumBars;

        public Wallet(long coins, int platinumBars)
        {
            _coins = new Currency(coins);
            _platinumBars = new PremiumCurrency(platinumBars);
        }

        public Currency Currency { get { return _coins; }  }

        public PremiumCurrency PremiumCurrency { get { return _platinumBars; } }
    }
}