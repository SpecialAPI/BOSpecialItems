using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class PerformAllAbilitiesButThisOffsettedEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if(caster is CharacterCombat cc)
            {
                var validAbilities = cc.CombatAbilities.Select(x => x.ability).Where(x => x != null && x.effects != null && !x.effects.Any(x => x.effect != null && x.effect is PerformAllAbilitiesButThisOffsettedEffect)).ToList();
                var offsettedAbilities = new List<(AbilitySO, int)>();
                for(int i = 0; i < validAbilities.Count; i++)
                {
                    offsettedAbilities.Add((validAbilities[i], i - Mathf.CeilToInt((validAbilities.Count - 1) / 2) + (validAbilities.Count % 2 == 0 && i > Mathf.FloorToInt((validAbilities.Count - 1) / 2) ? 1 : 0)));
                }
                CombatManager.Instance.AddSubAction(new AdvancedAnimationVisualsAction(offsettedAbilities.Select(x => new AdvancedAnimationData()
                {
                    playAudio = true,
                    targets = x.Item1.animationTarget,
                    timeDelay = 0f,
                    visuals = x.Item1.visuals,
                    targettingOffset = x.Item2
                }).ToList(), caster));
                offsettedAbilities.Do(x => CombatManager.Instance.AddSubAction(new OffsettedEffectAction(x.Item1.effects, x.Item2, caster, 0)));
                return true;
            }
            return false;
        }
    }
}
