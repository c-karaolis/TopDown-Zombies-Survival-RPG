using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.CharacterStats
{
    [Serializable]
    public class CharacterAttribute : CharacterStat
    {
        public AttributeType Name;
    }

    public enum AttributeType
    {
        Strength,
        Agility,
        Intelligence,
        Vitality,
        Perception,
        Luck,
        Charisma
    }
}