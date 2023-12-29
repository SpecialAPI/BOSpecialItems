using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class KingslayerEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			foreach (TargetSlotInfo t in targets)
			{
				if (t.HasUnit)
				{
					var offset = areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1);
					var amount = entryVariable;
					amount = caster.WillApplyDamage(amount, t.Unit);
					var info = t.Unit.Damage(amount, caster, DeathType.Basic, offset, addHealthMana: true, directDamage: true, false);
					exitAmount += info.damageAmount;
					if(info.damageAmount > 0 && t.Unit.Size > 1)
                    {
						for(int i = 0; i < t.Unit.Size; i++)
                        {
							var t2 = stats.combatSlots.GetGenericAllySlotTarget(t.Unit.SlotID + i, t.Unit.IsUnitCharacter);
							if(t2 != t)
                            {
								exitAmount += t.Unit.Damage(amount, null, DeathType.Basic, areTargetSlots ? (t2.SlotID - t.Unit.SlotID) : (-1), false, false, true).damageAmount;
							}
                        }
                    }
				}
			}
			caster.DidApplyDamage(exitAmount);
			return exitAmount > 0;
		}
    }
}
