using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class PerformEffectOnRandomTargets : EffectSO
    {
        public EffectInfo effect;
        public int targetsToPerformOn;
        public bool onlyPerformOnUnitSlots;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            var ts = targets.Where(x => !onlyPerformOnUnitSlots || x.HasUnit).ToList();
            var ts2 = new List<TargetSlotInfo>();
            if(targetsToPerformOn >= ts.Count)
            {
                ts2.AddRange(ts);
            }
            else
            {
                for(int i = 0; i < targetsToPerformOn && ts.Count > 0; i++)
                {
                    var r = Random.Range(0, ts.Count);
                    ts2.Add(ts[r]);
                    ts.RemoveAt(r);
                }
            }
            exitAmount = effect.StartEffect(stats, caster, ts2.ToArray(), areTargetSlots, PreviousExitValue);
            return effect.EffectSuccess;
        }
    }
}
