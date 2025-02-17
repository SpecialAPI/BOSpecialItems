using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ComparePreviousExitValueEffect : EffectSO
    {
        public ComparisonType comparison;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = PreviousExitValue;
            return comparison switch
            {
                ComparisonType.Equal => PreviousExitValue == entryVariable,
                ComparisonType.NotEqual => PreviousExitValue != entryVariable,
                ComparisonType.GreaterThan => PreviousExitValue > entryVariable,
                ComparisonType.GreaterThanOrEqual => PreviousExitValue >= entryVariable,
                ComparisonType.LessThan => PreviousExitValue < entryVariable,
                ComparisonType.LessThanOrEqual => PreviousExitValue <= entryVariable,
                _ => false
            };
        }

        public enum ComparisonType
        {
            Equal,
            NotEqual,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,
        }
    }
}
