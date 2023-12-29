using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DelayedEffectsPerformEffect : EffectSO
    {
        public EffectInfo[] effects;
        public bool subAction = true;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            (subAction ? (Action<CombatAction>)CombatManager.Instance.AddSubAction : CombatManager.Instance.AddRootAction)(new EffectAction(effects, caster));
            exitAmount = entryVariable;
            return true;
        }
    }
}
