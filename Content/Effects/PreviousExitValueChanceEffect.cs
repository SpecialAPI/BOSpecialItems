using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class PreviousExitValueChanceEffect : EffectSO
    {
        public int chancePerPreviousExitValue;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = PreviousExitValue;
            return Random.Range(0, 100) < (entryVariable + exitAmount * chancePerPreviousExitValue);
        }
    }
}
