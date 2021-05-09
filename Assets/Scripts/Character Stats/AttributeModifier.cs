using System;

namespace Foxlair.CharacterStats
{
    [Serializable]
    public class AttributeModifier : StatModifier
    {
        public AttributeType AttributeType;

        public AttributeModifier(float value, StatModType type, int order, object source) : base( value,  type,  order,  source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public AttributeModifier(float value, StatModType type) : base(value, type, (int)type, null) { }

        public AttributeModifier(float value, StatModType type, int order) : base(value, type, order, null) { }

        public AttributeModifier(float value, StatModType type, object source) : base(value, type, (int)type, source) { }
    }
}
