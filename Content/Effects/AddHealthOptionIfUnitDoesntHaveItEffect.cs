using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddHealthOptionIfUnitDoesntHaveItEffect : EffectSO
    {
        public List<ManaColorSO> healthColorsToAdd;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach(var t in targets)
            {
                if (t.HasUnit)
                {
                    var success = false;
                    var ext = t.Unit.UnitExt();
                    foreach(var hc in healthColorsToAdd)
                    {
                        if (!ext.HealthColors.Contains(hc))
                        {
                            ext.AddHealthColor(hc);
                            success = true;
                        }
                    }
                    if (success)
                    {
                        exitAmount++;
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
