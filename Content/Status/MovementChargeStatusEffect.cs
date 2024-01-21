using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class MovementChargeStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;
        public override bool CanBeRemoved => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(RefreshMove, TriggerCalls.OnMoved.ToString(), caller);
        }

        public void RefreshMove(object sender, object args)
        {
            if (sender is IUnit unit && unit.RestoreSwapUse() && unit is IStatusEffector effector)
            {
                int duration = StatusContent;
                StatusContent = Mathf.Max(0, StatusContent - 1);
                if (!TryRemoveStatusEffect(effector) && duration != StatusContent)
                {
                    effector.StatusEffectValuesChanged(EffectType, StatusContent - duration);
                }
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(RefreshMove, TriggerCalls.OnMoved.ToString(), caller);
        }

        public override void EffectTick(object sender, object args)
        {
            if (sender is IStatusEffector effector && StatusDurationCanBeReduced)
            {
                int duration = StatusContent;
                StatusContent = 0;
                if (!TryRemoveStatusEffect(effector) && duration != StatusContent)
                {
                    effector.StatusEffectValuesChanged(EffectType, StatusContent - duration);
                }
            }
        }
    }
}
