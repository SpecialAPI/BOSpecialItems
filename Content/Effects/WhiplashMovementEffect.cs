using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.GraphicsBuffer;

namespace BOSpecialItems.Content.Effects
{
    public class WhiplashMovementEffect : EffectSO
    {
        public bool prioritizeLeft;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var farthestEnemies = new List<IUnit>();
            var farthestDistance = -1;

            foreach (var target in targets)
            {
                if (target.HasUnit)
                {
                    int dist;

                    var left = target.Unit.LastSlotId() < caster.SlotID;
                    var right = target.Unit.SlotID > caster.LastSlotId();

                    if (left && !right)
                    {
                        dist = Mathf.Abs(target.Unit.SlotID - caster.SlotID);
                    }
                    else if (right && !left)
                    {
                        dist = Mathf.Abs(target.Unit.LastSlotId() - caster.LastSlotId());
                    }
                    else
                    {
                        continue;
                    }

                    if (dist > farthestDistance)
                    {
                        farthestEnemies.Clear();
                        farthestDistance = dist;
                    }
                    if (dist >= farthestDistance)
                    {
                        farthestEnemies.Add(target.Unit);
                    }
                }
            }

            IUnit targetEnemy = null;

            if (farthestEnemies.Count <= 0)
            {
                return false;
            }
            else if (farthestEnemies.Count >= 1)
            {
                foreach (var target in farthestEnemies)
                {
                    if (target.LastSlotId() < caster.SlotID == prioritizeLeft)
                    {
                        targetEnemy = target;
                    }
                }
            }

            if (targetEnemy == null)
            {
                targetEnemy = farthestEnemies[0];
            }

            if (targetEnemy != null)
            {
                var moveCharacter = targetEnemy.Size > caster.Size;

                if (moveCharacter)
                {
                    for (int i = 0; i < targetEnemy.Size; i++)
                    {
                        var sid = targetEnemy.SlotID + i;
                        var sid2 = caster.SlotID;

                        if (targetEnemy.LastSlotId() < caster.SlotID)
                        {
                            sid = targetEnemy.LastSlotId() - i;
                        }

                        if (stats.combatSlots.SwapCharacters(caster.SlotID, sid, false, SwapType.CharacterSwap))
                        {
                            exitAmount = Mathf.Abs(sid - sid2);
                            break;
                        }
                    }
                }
                else
                {
                    int dist;

                    var left = targetEnemy.LastSlotId() < caster.SlotID;
                    var right = targetEnemy.SlotID > caster.LastSlotId();

                    if (left && !right)
                    {
                        dist = Mathf.Abs(targetEnemy.SlotID - caster.SlotID);
                    }
                    else if (right && !left)
                    {
                        dist = Mathf.Abs(targetEnemy.LastSlotId() - caster.LastSlotId());
                    }
                    else
                    {
                        return false;
                    }

                    for (int i = 0; i < dist; i++)
                    {
                        var move = left ? targetEnemy.Size : -1;

                        if (stats.combatSlots.CanEnemiesSwap(targetEnemy.SlotID, targetEnemy.SlotID + move, out var first, out var second) && stats.combatSlots.SwapEnemies(targetEnemy.SlotID, first, targetEnemy.SlotID + move, second, false, SwapType.EnemySwap))
                        {
                            exitAmount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
