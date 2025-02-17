using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DealDamageByUnitTypeEffect : EffectSO
    {
        public UnitType targetUnitType;
        public int damageToUnitType;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach(var t in targets)
            {
                if (t.HasUnit)
                {
                    exitAmount += t.Unit.Damage(caster.WillApplyDamage(t.Unit.UnitType == targetUnitType ? damageToUnitType : entryVariable, t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1), true, true, false).damageAmount;
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
