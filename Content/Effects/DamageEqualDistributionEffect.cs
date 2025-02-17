using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DamageEqualDistributionEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            var directdamage = 0;

            foreach(var t in targets)
            {
                if(t != null && t.HasUnit)
                {
                    List<(IUnit, bool)> hitUnits = new() { (t.Unit, true) };
                    var leftEnemy = stats.combatSlots.GetAllySlotTarget(t.Unit.SlotID, -1, t.Unit.IsUnitCharacter)?.Unit;
                    var rightEnemy = stats.combatSlots.GetAllySlotTarget(t.Unit.SlotID, 1, t.Unit.IsUnitCharacter)?.Unit;
                    while(leftEnemy != null || rightEnemy != null)
                    {
                        if(leftEnemy != null)
                        {
                            hitUnits.Add((leftEnemy, false));
                            leftEnemy = stats.combatSlots.GetAllySlotTarget(leftEnemy.SlotID, -1, leftEnemy.IsUnitCharacter)?.Unit;
                        }
                        if(rightEnemy != null)
                        {
                            hitUnits.Add((rightEnemy, false));
                            rightEnemy = stats.combatSlots.GetAllySlotTarget(rightEnemy.SlotID, 1, rightEnemy.IsUnitCharacter)?.Unit;
                        }
                    }
                    var amt = entryVariable;
                    while(hitUnits.Count > 0 && amt > 0)
                    {
                        var uinfo = hitUnits[0];
                        var unit = uinfo.Item1;
                        if(unit != null)
                        {
                            var directDamage = uinfo.Item2;
                            var damageAmount = amt / hitUnits.Count;
                            amt = Mathf.Max(amt - damageAmount, 0);
                            if (directDamage)
                            {
                                var dmg = unit.Damage(caster.WillApplyDamage(damageAmount, unit), caster, DeathType.Basic, t.SlotID - unit.SlotID, true, true, false, DamageType.None).damageAmount;

                                directdamage += dmg;
                                exitAmount += dmg;
                            }
                            else
                            {
                                exitAmount += unit.Damage(damageAmount, null, DeathType.Basic, -1, false, false, true, DamageType.None).damageAmount;
                            }
                        }
                        hitUnits.RemoveAt(0);
                    }
                }
            }

            if(directdamage > 0)
            {
                caster.DidApplyDamage(directdamage);
            }

            return exitAmount > 0;
        }
    }
}
