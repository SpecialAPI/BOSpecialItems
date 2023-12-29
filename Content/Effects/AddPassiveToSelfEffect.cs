using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddPassiveToSelfEffect : EffectSO
    {
		[SerializeField]
		public BasePassiveAbilitySO passiveToAdd;

		public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
		{
			exitAmount = 0;
			if (caster.AddPassiveAbility(passiveToAdd))
			{
				exitAmount++;
			}
			return exitAmount > 0;
		}
	}
}
