using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public abstract class ApplyGenericStatusEffect<T> : EffectSO where T : GenericStatusEffect, new()
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
		{
			exitAmount = 0;
			if (entryVariable <= 0)
			{
				return false;
			}
			foreach (var t in targets)
			{
				if (t.HasUnit)
				{
					var tse = new T();
					tse.SetupStatus(entryVariable);
					if (t.Unit.ApplyStatusEffect(tse, entryVariable))
					{
						exitAmount += entryVariable;
					}
				}
			}
			return exitAmount > 0;
		}
    }
}
