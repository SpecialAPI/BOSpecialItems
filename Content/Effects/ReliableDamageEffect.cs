using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ReliableDamageEffect : EffectSO
    {
		public bool triggerOnBeingDamagedCalls = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			foreach (TargetSlotInfo targetSlotInfo in targets)
			{
				if (targetSlotInfo.HasUnit)
				{
					caster.WillApplyDamage(entryVariable, targetSlotInfo.Unit);
					exitAmount += targetSlotInfo.Unit.ReliableDamage(entryVariable, caster, DeathType.Basic, areTargetSlots ? targetSlotInfo.SlotID - targetSlotInfo.Unit.SlotID : -1, true, true, false, DamageType.None, triggerOnBeingDamagedCalls).damageAmount;
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
