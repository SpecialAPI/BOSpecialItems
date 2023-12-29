using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class BerserkStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(OnWillDamageTriggered, TriggerCalls.OnWillApplyDamage.ToString(), caller);
        }

        public void OnWillDamageTriggered(object sender, object args)
        {
            (args as DamageDealtValueChangeException).AddModifier(new MultiplyIntValueModifier(true, 2));
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(OnWillDamageTriggered, TriggerCalls.OnWillApplyDamage.ToString(), caller);
        }
    }
}
