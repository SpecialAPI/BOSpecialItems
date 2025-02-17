using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables.TriggerEffects
{
    public class DoEffectOnNotificationTargetTriggerEffect : TriggerEffect
    {
        public TriggerEffect targetEffect;

        public override void DoEffect(IUnit sender, object args, EffectsAndTriggerBase effectsAndTrigger)
        {
            if (args is ITargettedNotificationInfo inf && inf.Target != null)
            {
                targetEffect.DoEffect(inf.Target, args, effectsAndTrigger);
            }
        }
    }
}
