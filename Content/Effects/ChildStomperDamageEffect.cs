using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BOSpecialItems.Content.Effects
{
    public class ChildStomperDamageEffect : EffectSO
    {
        public int childDamage;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (var t in targets)
            {
                if (t.HasUnit)
                {
                    var child = t.Unit.ContainsPassiveAbility(PassiveAbilityTypes.Infantile);

                    if (child)
                    {
                        exitAmount += t.Unit.SilentDamage(caster.WillApplyDamage(childDamage, t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1), true, true, false).damageAmount;
                    }
                    else
                    {
                        exitAmount += t.Unit.Damage(caster.WillApplyDamage(entryVariable, t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1), true, true, false).damageAmount;
                    }
                }
            }

            if (exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            return exitAmount > 0;
        }
    }
}
