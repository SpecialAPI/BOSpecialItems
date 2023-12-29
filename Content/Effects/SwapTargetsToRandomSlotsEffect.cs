using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class SwapTargetsToRandomSlotsEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			var list = new List<IUnit>();
			var list2 = new List<IUnit>();
			for (int i = 0; i < targets.Length; i++)
			{
				if (targets[i].HasUnit)
				{
					IUnit unit = targets[i].Unit;
					if (unit.IsUnitCharacter && !list.Contains(unit))
					{
						list.Add(unit);
					}
					else if (!unit.IsUnitCharacter && !list2.Contains(unit))
					{
						list2.Add(unit);
					}
				}
			}
			foreach (IUnit item in list)
			{
				int num = Random.Range(0, 5);
				if (num >= 0 && num < stats.combatSlots.CharacterSlots.Length)
				{
					if (stats.combatSlots.SwapCharacters(item.SlotID, num, isMandatory: true))
					{
						exitAmount++;
					}
					continue;
				}
				num *= -1;
				if (num >= 0 && num < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(item.SlotID, num, isMandatory: true))
				{
					exitAmount++;
				}
			}
			foreach (IUnit item2 in list2)
			{
				int num = Random.Range(0, 6 - item2.Size);
				if (stats.combatSlots.CanEnemiesSwap(item2.SlotID, num, out var firstSlotSwap, out var secondSlotSwap))
				{
					if (stats.combatSlots.SwapEnemies(item2.SlotID, firstSlotSwap, num, secondSlotSwap))
					{
						exitAmount++;
					}
					continue;
				}
				num = ((num < 0) ? item2.Size : (-1));
				if (stats.combatSlots.CanEnemiesSwap(item2.SlotID, num, out firstSlotSwap, out secondSlotSwap) && stats.combatSlots.SwapEnemies(item2.SlotID, firstSlotSwap, num, secondSlotSwap))
				{
					exitAmount++;
				}
			}
			return exitAmount > 0;
		}
    }
}
