﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum RariryType
    {
        Junk,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
    public class Rarity
    {
        [SerializeField] public RariryType name;
        private Color colour;
    public Color Colour
    {
        set
        {
            switch (name)
            {
                case RariryType.Common:
                    colour = CalculateHexColour("#8C8888");
                    break;

                case RariryType.Uncommon:
                    colour = CalculateHexColour("#09AD00");
                    break;

                case RariryType.Rare:
                    colour = CalculateHexColour("#0834FF");
                    break;

                case RariryType.Epic:
                    colour = CalculateHexColour("#FF0093");
                    break;

                case RariryType.Legendary:
                    colour = CalculateHexColour("#FF9500");
                    break;

                case RariryType.Mythic:
                    colour = CalculateHexColour("#00FFFF");
                    break;

                default:
                    colour = CalculateHexColour("#8C8888");
                    break;
            }

        }

        get 
        {
            switch (name)
            {
                case RariryType.Common:
                    colour = CalculateHexColour("#8C8888");
                    break;

                case RariryType.Uncommon:
                    colour = CalculateHexColour("#09AD00");
                    break;

                case RariryType.Rare:
                    colour = CalculateHexColour("#0834FF");
                    break;

                case RariryType.Epic:
                    colour = CalculateHexColour("#FF0093");
                    break;

                case RariryType.Legendary:
                    colour = CalculateHexColour("#FF9500");
                    break;

                case RariryType.Mythic:
                    colour = CalculateHexColour("#00FFFF");
                    break;

                default:
                    colour = CalculateHexColour("#8C8888");
                    break;
            }
            return colour; 
        }
    }


    Color CalculateHexColour(string hexColorString)
    {
        Color calculatedColor;
        ColorUtility.TryParseHtmlString(hexColorString, out calculatedColor);


        return calculatedColor;
    }
    //public RariryType Name { get { return name; } }

    //public Color Colour { get { return colour; } }



}