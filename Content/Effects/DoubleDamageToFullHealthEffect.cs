using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DoubleDamageToFullHealthEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach(var t in targets)
            {
                if (t.HasUnit)
                {
                    exitAmount += t.Unit.Damage(caster.WillApplyDamage((t.Unit.CurrentHealth >= t.Unit.MaximumHealth ? 2 : 1) * entryVariable, t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1), true, true, false).damageAmount;
                }
            }
            if(exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
                return true;
            }
            return false;
        }
    }
}
