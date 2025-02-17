using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    public class MultiCustomTriggerEffectWearable : BaseWearableSO
    {
        public List<EffectsAndTriggerBase> triggerEffects;
        private readonly Dictionary<int, Action<object, object>> effectMethods = new();

        public override bool IsItemImmediate => false;
        public override bool DoesItemTrigger => false;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            if (triggerEffects != null)
            {
                for (var i = 0; i < triggerEffects.Count; i++)
                {
                    var te = triggerEffects[i];
                    var strings = te.TriggerStrings();

                    foreach (var str in strings)
                    {
                        CombatManager.Instance.AddObserver(GetEffectMethod(i), str, caller);
                    }
                }
            }
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            if (triggerEffects != null)
            {
                for (var i = 0; i < triggerEffects.Count; i++)
                {
                    var te = triggerEffects[i];
                    var strings = te.TriggerStrings();

                    foreach (var str in strings)
                    {
                        CombatManager.Instance.RemoveObserver(GetEffectMethod(i), str, caller);
                    }
                }
            }
        }

        public Action<object, object> GetEffectMethod(int i)
        {
            if (effectMethods.TryGetValue(i, out var existing))
            {
                return existing;
            }
            return effectMethods[i] = (object sender, object args) => TryPerformItemEffect(sender, args, i);
        }

        public void TryPerformItemEffect(object sender, object args, int index)
        {
            if (index < triggerEffects.Count && sender is IWearableEffector effector && effector.CanWearableTrigger)
            {
                var te = triggerEffects[index];

                if (te != null)
                {
                    if (te.conditions != null)
                    {
                        foreach (var cond in te.conditions)
                        {
                            if (!cond.MeetCondition(effector, args))
                            {
                                return;
                            }
                        }
                    }

                    if (te.immediate)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new PerformItemCustomImmediateAction(this, sender, args, index));
                    }
                    else
                    {
                        CombatManager.Instance.AddSubAction(new PerformItemCustomAction(this, sender, args, index));
                    }
                }
            }
        }

        public override void FinalizeCustomTriggerItem(object sender, object args, int idx)
        {
            if (idx < triggerEffects.Count && sender is IWearableEffector effector && sender is IUnit caster && !effector.IsWearableConsumed)
            {
                var te = triggerEffects[idx];

                if (te != null)
                {
                    var consumed = te.getsConsumed;
                    if (consumed)
                    {
                        effector.ConsumeWearable();
                    }
                    if (te.doesPopup)
                    {
                        CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction(effector.ID, GetItemLocData().text, consumed, wearableImage));
                    }

                    te.effect.DoEffect(caster, args, te);
                }
            }
        }
    }

    public abstract class TriggerEffect
    {
        public abstract void DoEffect(IUnit sender, object args, EffectsAndTriggerBase effectsAndTrigger);
    }

    public abstract class EffectsAndTriggerBase
    {
        public bool immediate;
        public bool doesPopup;
        public List<EffectorConditionSO> conditions;
        public bool getsConsumed;
        public TriggerEffect effect;

        public abstract IEnumerable<string> TriggerStrings();
    }

    public class EffectsAndTriggerPair : EffectsAndTriggerBase
    {
        public TriggerCalls trigger;

        public override IEnumerable<string> TriggerStrings()
        {
            yield return trigger.ToString();
        }
    }

    public class EffectsAndCustomTriggerPair : EffectsAndTriggerBase
    {
        public string customTrigger;

        public override IEnumerable<string> TriggerStrings()
        {
            yield return customTrigger;
        }
    }

    public class EffectsAndMultipleTriggersPair : EffectsAndTriggerBase
    {
        public List<TriggerCalls> triggers;

        public override IEnumerable<string> TriggerStrings()
        {
            foreach (var tc in triggers)
            {
                yield return triggers.ToString();
            }
        }
    }

    public class EffectsAndMultipleCustomTriggersPair : EffectsAndTriggerBase
    {
        public List<string> customTriggers;

        public override IEnumerable<string> TriggerStrings()
        {
            return customTriggers;
        }
    }
}
