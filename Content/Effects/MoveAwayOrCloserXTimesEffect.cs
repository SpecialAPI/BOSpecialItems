using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class MoveAwayOrCloserXTimesEffect : EffectSO
    {
        public bool away;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            var chars = new List<(IUnit unit, int move)>();
            var enemies = new List<(IUnit unit, int move)>();
            for (int i = 0; i < targets.Length; i++)
            {
                if (targets[i].HasUnit)
                {
                    var u = targets[i].Unit;

                    var isLeft = u.SlotID < caster.SlotID;
                    var isRight = u.SlotID + u.Size > caster.SlotID + caster.Size;

                    if(isLeft == isRight)
                    {
                        continue;
                    }

                    var move = isLeft == away ? -1 : 1;

                    if (u.IsUnitCharacter && !chars.Contains((u, move)))
                    {
                        chars.Add((u, move));
                    }
                    else if (!u.IsUnitCharacter && !enemies.Contains((u, move)))
                    {
                        enemies.Add((u, move));
                    }
                }
            }
            foreach ((var ch, var num) in chars)
            {
                for (int i = 0; i < entryVariable; i++)
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
            foreach ((var en, var num) in enemies)
            {
                for (int i = 0; i < entryVariable; i++)
                {
                    if (stats.combatSlots.CanEnemiesSwap(en.SlotID, en.SlotID + num, out var firstSlotSwap, out var secondSlotSwap) && stats.combatSlots.SwapEnemies(en.SlotID, firstSlotSwap, en.SlotID + num, secondSlotSwap))
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
