using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    public class ModifyAbilityUsedWithEffectsWearable : PerformEffectWearable
    {
        public override bool IsItemImmediate => false;
        public override bool DoesItemTrigger => true;

        public AbilitySO newAbility;
        public EffectorConditionSO[] modifyConditions;
        public bool doesPopupOnModify = true;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            base.CustomOnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(ModifyAbility, CustomEvents.MODIFY_USED_ABILITY, caller);
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            base.CustomOnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(ModifyAbility, CustomEvents.MODIFY_USED_ABILITY, caller);
        }

        public void ModifyAbility(object sender, object args)
        {
            if(args is AbilityContext context && sender is IWearableEffector effector && sender is IUnit caster)
            {
                if(modifyConditions != null)
                {
                    foreach(var c in modifyConditions)
                    {
                        if(c != null && !c.MeetCondition(effector, args))
                        {
                            return;
                        }
                    }
                }

                if (doesPopupOnModify)
                {
                    CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction(caster.ID, GetItemLocData().text, false, wearableImage));
                }

                context.ability = newAbility;
            }
        }
    }
}
