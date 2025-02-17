using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DryDamageEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var target in targets)
            {
                if (target.HasUnit)
                {
                    exitAmount += target.Unit.Damage(caster.WillApplyDamage(entryVariable, target.Unit), caster, DeathType.Basic, areTargetSlots ? (target.SlotID - target.Unit.SlotID) : -1, false, true, false, DamageType.None).damageAmount;
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
