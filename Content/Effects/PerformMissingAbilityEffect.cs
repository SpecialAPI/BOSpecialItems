using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class PerformMissingAbilityEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			foreach (TargetSlotInfo targetSlotInfo in targets)
			{
				if (targetSlotInfo.HasUnit && targetSlotInfo.Unit is CharacterCombat cc)
				{
					var success = false;
					if(!cc.Character.usesAllAbilities && cc.Character.HasRankedData)
					{
						var abs = cc.Character.rankedData[CombatAbilityModifiers.ModifyAbilityRank(cc.ClampedRank, cc)].rankAbilities;
						if (abs != null && abs.Length > 0)
						{
							var abilities = new List<AbilitySO>();
							for (int i = 0; i < abs.Length; i++)
							{
								if (i != cc.FirstAbility && i != cc.SecondAbility)
								{
									abilities.Add(abs[i].ability);
								}
							}
							if (abilities.Count > 0)
							{
								exitAmount++;
								success = true;
								foreach (var ab in abilities)
								{
									cc.TryPerformRandomAbility(ab);
								}
							}
						}
					}
                    if (!success)
                    {
						cc.TryPerformRandomAbility(BrutalAPIPlugin.slapCharAbility.ability);
                    }
				}
			}
			return exitAmount > 0;
		}
    }
}
