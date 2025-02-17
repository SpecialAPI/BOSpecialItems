using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOSpecialItems.Content.Effects
{
    public class FeedbackerDamageEffect : EffectSO
    {
        public int damageWithNotifications;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var target in targets)
            {
                if (target.HasUnit)
                {
                    var hasOnDamageNotifications = HasAnyNotifications(TriggerCalls.OnDamaged.ToString(), target.Unit) || HasAnyNotifications(TriggerCalls.OnDirectDamaged.ToString(), target.Unit);

                    exitAmount += target.Unit.SilentDamage(caster.WillApplyDamage(hasOnDamageNotifications ? damageWithNotifications : entryVariable, target.Unit), caster, DeathType.Basic, areTargetSlots ? (target.SlotID - target.Unit.SlotID) : -1, true, true, true, DamageType.None).damageAmount;
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
