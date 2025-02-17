using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables.TriggerEffects
{
    public class PerformEffectTriggerEffect : TriggerEffect
    {
        public EffectInfo[] effects = new EffectInfo[0];

        public override void DoEffect(IUnit sender, object args, EffectsAndTriggerBase effectsAndTrigger)
        {
            if (effectsAndTrigger.immediate)
            {
                CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(effects, sender, 0));
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EffectAction(effects, sender, 0));
            }
        }
    }
}
