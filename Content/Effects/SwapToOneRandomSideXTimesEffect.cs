using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class SwapToOneRandomSideXTimesEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            var chars = new List<IUnit>();
            var enemies = new List<IUnit>();
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].HasUnit)
                {
                    var u = targets[i].Unit;
                    if (u.IsUnitCharacter && !chars.Contains(u))
                    {
                        chars.Add(u);
                    }
                    else if (!u.IsUnitCharacter && !enemies.Contains(u))
                    {
                        enemies.Add(u);
                    }
                }
            }
            foreach (IUnit ch in chars)
            {
                var num = Random.Range(0, 2) * 2 - 1;
                if (ch.SlotID + num < 0 || ch.SlotID + num >= stats.combatSlots.CharacterSlots.Length)
                {
                    num *= -1;
                }
                for(int i = 0; i < entryVariable; i++)
                {
                    if (ch.SlotID + num >= 0 && ch.SlotID + num < stats.combatSlots.CharacterSlots.Length && stats.combatSlots.SwapCharacters(ch.SlotID, ch.SlotID + num, isMandatory: true))
                    {
                        exitAmount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            foreach (IUnit en in enemies)
            {
                var num = Random.Range(0, 2) * (en.Size + 1) - 1;
                if (!stats.combatSlots.CanEnemiesSwap(en.SlotID, en.SlotID + num, out var firstSlotSwap, out var secondSlotSwap))
                {
                    num = (num < 0) ? en.Size : (-1);
                }
                for (int i = 0; i < entryVariable; i++)
                {
                    if (stats.combatSlots.CanEnemiesSwap(en.SlotID, en.SlotID + num, out firstSlotSwap, out secondSlotSwap) && stats.combatSlots.SwapEnemies(en.SlotID, firstSlotSwap, en.SlotID + num, secondSlotSwap))
                    {
                        exitAmount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
