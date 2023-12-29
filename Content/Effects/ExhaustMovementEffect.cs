using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ExhaustMovementEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i].HasUnit && targets[i].Unit is CharacterCombat cc)
				{
					bool canUseAbilitiesNoTrigger = cc.CanSwapNoTrigger;
					cc.CanSwap = false;
					cc.SetVolatileUpdateUIAction(true);
					if (canUseAbilitiesNoTrigger && !cc.CanSwapNoTrigger)
					{
						exitAmount++;
					}
				}
			}
			return exitAmount > 0;
		}
    }
}
