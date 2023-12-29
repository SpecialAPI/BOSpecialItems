using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class RemovePassiveFromSelfEffect : EffectSO
	{
		[SerializeField]
		public PassiveAbilityTypes passiveToRemove = PassiveAbilityTypes.Focus;

		public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			if (caster.TryRemovePassiveAbility(passiveToRemove))
			{
				exitAmount++;
			}
			return exitAmount > 0;
		}
    }
}
