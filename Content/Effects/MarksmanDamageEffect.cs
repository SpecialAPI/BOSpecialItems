using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class MarksmanDamageEffect : EffectSO
    {
        public int chainStartDamage;
        public int chainDamageIncrease;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var target in targets)
            {
                if (target.HasUnit)
                {
                    exitAmount += target.Unit.Damage(caster.WillApplyDamage(entryVariable, target.Unit), caster, DeathType.Basic, areTargetSlots ? (target.SlotID - target.Unit.SlotID) : -1, true, true, false, DamageType.None).damageAmount;

                    var remainingEnemies = stats.combatSlots.GetAllUnitTargetSlots(target.Unit.IsUnitCharacter, false, -1).ToList();
                    remainingEnemies.RemoveAll(x => x.Unit == target.Unit || x.Unit == null);

                    var current = target.Unit;

                    for(var chain = 0; chain < PreviousExitValue && remainingEnemies.Count > 0; chain++)
                    {
                        var anyoneHasFrail = remainingEnemies.Any(x => x.Unit != null && x.Unit.ContainsStatusEffect(StatusEffectType.Frail));

                        var closestEnemies = new List<IUnit>();
                        var closestDistance = 9999;

                        foreach(var t in remainingEnemies)
                        {
                            if(!t.HasUnit || (anyoneHasFrail && !t.Unit.ContainsStatusEffect(StatusEffectType.Frail)))
                            {
                                continue;
                            }

                            int dist;

                            var left = t.Unit.LastSlotId() < current.SlotID;
                            var right = t.Unit.SlotID > current.LastSlotId();

                            if (left && !right)
                            {
                                dist = Mathf.Abs(t.Unit.LastSlotId() - current.SlotID);
                            }
                            else if (right && !left)
                            {
                                dist = Mathf.Abs(t.Unit.SlotID - current.LastSlotId());
                            }
                            else
                            {
                                continue;
                            }

                            if (dist < closestDistance)
                            {
                                closestEnemies.Clear();
                                closestDistance = dist;
                            }
                            if (dist <= closestDistance)
                            {
                                closestEnemies.Add(t.Unit);
                            }
                        }

                        IUnit nextChainEnemy;

                        if(closestEnemies.Count == 0)
                        {
                            break;
                        }
                        else if (closestEnemies.Count > 1)
                        {
                            nextChainEnemy = closestEnemies[Random.Range(0, closestEnemies.Count)];
                        }
                        else
                        {
                            nextChainEnemy = closestEnemies[0];
                        }

                        if(nextChainEnemy != null)
                        {
                            nextChainEnemy.Damage(caster.WillApplyDamage(chainStartDamage + chainDamageIncrease * chain, nextChainEnemy), caster, DeathType.Basic, -1, false, true, false, DamageType.None);
                            current = nextChainEnemy;
                            remainingEnemies.RemoveAll(x => x.Unit == nextChainEnemy);
                            continue;
                        }
                        break;
                    }
                }
            }

            if(exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            return exitAmount > 0;
        }
    }
}
