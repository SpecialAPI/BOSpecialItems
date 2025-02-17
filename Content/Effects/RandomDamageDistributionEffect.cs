using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class RandomDamageDistributionEffect : EffectSO
    {
        public int previousExitValueContribution;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var total = entryVariable + PreviousExitValue * previousExitValueContribution;

            var a = Enumerable.Repeat(0, targets.Length - 1)
                  .Select(x => Random.Range(1, total))
                  .Concat(new[] { 0, total })
                  .OrderBy(x => x)
                  .ToArray();

            var b = a.Skip(1).Select((x, i) => x - a[i]).ToArray();

            for(int i = 0; i < b.Length || i < targets.Length; i++)
            {
                var t = targets[i];

                if (t.HasUnit)
                {
                    exitAmount += t.Unit.Damage(caster.WillApplyDamage(b[i], t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : -1, true, true, false, DamageType.None).damageAmount;
                }
            }

            if(exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            return exitAmount > 0;
        }
    }
}
