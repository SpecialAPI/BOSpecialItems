using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class RandomlyHealRandomTargetEffect : EffectSO
    {
		public int maxAdditionalHealing;
		
		public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
		{
			exitAmount = 0;
			var validtargets = targets.ToList().FindAll(x => x.HasUnit && x.Unit.IsAlive && x.Unit.CurrentHealth != x.Unit.MaximumHealth && x.Unit.CanHeal(true, entryVariable));
			if(validtargets.Count > 0)
			{
				var target = validtargets[UnityEngine.Random.Range(0, validtargets.Count)];
				exitAmount += target.Unit.Heal(entryVariable + UnityEngine.Random.Range(0, maxAdditionalHealing + 1), HealType.Heal, true);
			}
			return exitAmount > 0;
		}
	}
}
