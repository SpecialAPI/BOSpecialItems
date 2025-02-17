using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ConsumeAllPigmentFromGeneratorEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            return (exitAmount = stats.YellowManaBar.ConsumeAllMana(stats.GenerateUnitJumpInformation(caster.ID, caster.IsUnitCharacter), stats.audioController.manaConsumedSound)) > 0;
        }
    }
}
