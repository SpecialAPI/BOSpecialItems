using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddNewPassiveNotInHealthOptionsEffect : EffectSO
    {
        public Dictionary<PigmentType, BasePassiveAbilitySO> passives;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            if (passives == null)
                return false;

            foreach(var target in targets)
            {
                if (target.HasUnit)
                {
                    var u = target.Unit;
                    var possiblePassives = new List<BasePassiveAbilitySO>();

                    foreach(var passive in passives)
                    {
                        if (passive.Value == null)
                            continue;

                        if(!u.ContainsPassiveAbility(passive.Value.type) && !u.UnitExt().HealthColors.Exists(x => x.pigmentType == passive.Key))
                            possiblePassives.Add(passive.Value);
                    }

                    if(possiblePassives.Count > 0)
                    {
                        var randomPassive = possiblePassives[Random.Range(0, possiblePassives.Count)];

                        if (u.AddPassiveAbility(randomPassive))
                            exitAmount++;
                    }
                }
            }

            return exitAmount > 0;
        }
    }
}
