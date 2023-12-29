using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ModifyManaNotOfSpecificColorEffect : EffectSO
    {
        public ManaColorSO[] options;
        public ManaColorSO excludeColor;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = stats.MainManaBar.RandomizeAllButColor(excludeColor, options);
            return exitAmount > 0;
        }
    }
}
