using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ChangeMaxHealthFilledEffect : EffectSO
    {
		public bool increase = true;
		public bool entryAsPercentage;

		public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
		{
			exitAmount = 0;
			foreach(var target in targets)
            {
                if (target.HasUnit)
                {
					var u = target.Unit;
					var diff = entryAsPercentage ? u.CalculatePercentualAmount(entryVariable) : entryVariable;
					var newHealth = u.MaximumHealth + (diff * (increase ? 1 : -1));
                    if (u.MaximizeHealth(newHealth))
                    {
						exitAmount += diff;
						if(diff > 0)
                        {
							u.SetHealthTo(u.CurrentHealth + diff);
                        }
                    }
                }
            }
			return exitAmount > 0;
		}
	}
}
