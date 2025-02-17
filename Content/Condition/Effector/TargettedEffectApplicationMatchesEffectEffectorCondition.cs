using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Condition.Effector
{
    public class TargettedEffectApplicationMatchesEffectEffectorCondition : EffectorConditionSO
    {
        public StatusEffectType targetEffect;

        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            return args is TargettedStatusEffectApplication appl && appl.statusEffect != null && appl.statusEffect.EffectType == targetEffect;
        }
    }
}
