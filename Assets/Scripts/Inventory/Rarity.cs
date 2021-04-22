using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Inventory
{
    [CreateAssetMenu(fileName ="New Rarity", menuName = "Inventory System/Items/Rarity")]
    public class Rarity : ScriptableObject
    {
        [SerializeField] private new string name = "New Rarity Name";
        [SerializeField] private Color colour = new Color(1f, 1f, 1f, 1f);

        public string Name { get { return name; } }

        public Color Colour { get { return colour; } }



    }
}