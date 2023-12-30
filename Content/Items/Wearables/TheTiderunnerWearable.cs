using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    public class TheTiderunnerWearable : BaseWearableSO
    {
        public override bool IsItemImmediate => false;

        public override bool DoesItemTrigger => true;

        public EffectInfo[] leftEffects;
        public EffectInfo[] rightEffects;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            CombatManager.Instance.AddObserver(TryConsumeWearable, CustomEvents.ABILITY_USED_CONTEXT, caller);
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            CombatManager.Instance.RemoveObserver(TryConsumeWearable, CustomEvents.ABILITY_USED_CONTEXT, caller);
        }

        public override void TriggerPassive(object sender, object args)
        {
            if(args is AbilityContext context && sender is CharacterCombat cc && cc.CombatAbilities.Count > 1)
            {
                if(context.abilityId == 0)
                {
                    CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction((sender as IWearableEffector).ID, GetItemLocData().text, false, wearableImage));
                    if (IsItemImmediate)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(leftEffects, cc));
                    }
                    else
                    {
                        CombatManager.Instance.AddSubAction(new EffectAction(leftEffects, cc));
                    }
                }
                else if(context.abilityId == cc.CombatAbilities.Count - 1)
                {
                    CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction((sender as IWearableEffector).ID, GetItemLocData().text, false, wearableImage));
                    if (IsItemImmediate)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(leftEffects, cc));
                    }
                    else
                    {
                        CombatManager.Instance.AddSubAction(new EffectAction(rightEffects, cc));
                    }
                }
            }
        }
    }

    public class MoveSlapRightFlag : WearableStaticFlagPassive
    {
    }
}
