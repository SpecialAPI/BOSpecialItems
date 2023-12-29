using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ShuffleHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            List<(int, int, ManaColorSO)> healths = new();
            List<IUnit> units = new();
            foreach(var t in targets)
            {
                if (t.HasUnit)
                {
                    healths.Add((t.Unit.CurrentHealth, t.Unit.MaximumHealth, t.Unit.HealthColor));
                    units.Add(t.Unit);
                }
            }
            healths.Shuffle();
            for(int i = 0; i < units.Count; i++)
            {
                units[i].MaximizeHealth(healths[i].Item2);
                units[i].ChangeHealthTo(healths[i].Item1);
                units[i].ChangeHealthColor(healths[i].Item3);
            }
            exitAmount = 0;
            return true;
        }
    }
}
