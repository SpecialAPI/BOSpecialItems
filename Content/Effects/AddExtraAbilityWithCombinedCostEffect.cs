using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddExtraAbilityWithCombinedCostEffect : EffectSO
    {
        public AbilitySO ability;
        private static readonly RaritySO defaultRarity = CreateScriptable<RaritySO>(x => { x.rarityValue = 0; x.canBeRerolled = false; });

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (caster is CharacterCombat cc)
            {
                cc.AddExtraAbility(new()
                {
                    ability = ability,
                    cost = cc.CombatAbilities.Select(x => x.cost).SelectMany(x => x).ToArray(),
                    rarity = defaultRarity
                });
                return true;
            }
            return false;
        }
    }
}
