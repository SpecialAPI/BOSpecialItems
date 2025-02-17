using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AlternateBetweenCasterAbilitiesEffect : EffectSO
    {
        public List<ExtraAbilityInfo> Abilities;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var abilities = caster.GetAbilities();

            for(int i = 0; i < abilities.Count; i++)
            {
                if (abilities[i] != null && Abilities.Exists(x => x.ability == abilities[i].ability))
                {
                    var extraAb = Abilities[(Abilities.FindIndex(x => x.ability == abilities[i].ability) + 1) % Abilities.Count];

                    abilities[i] = new(extraAb.ability, extraAb.cost)
                    {
                        rarity = extraAb.rarity,
                    };

                    exitAmount++;
                }
            }

            if(exitAmount > 0)
            {
                caster.UpdateUIAbilities();
            }

            return exitAmount > 0;
        }
    }
}
