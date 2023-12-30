using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    public class ModifyWrongPigmentWearable : BaseWearableSO
    {
        public override bool IsItemImmediate => true;

        public override bool DoesItemTrigger => false;

        public int addWrongPigment;
        public int multiplyWrongPigment = 1;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            CombatManager.Instance.AddObserver(TryConsumeWearable, CustomEvents.MODIFY_WRONG_PIGMENT, caller);
        }

        public override void TriggerPassive(object sender, object args)
        {
            if (args is IntegerReference i)
            {
                i.value = (i.value * multiplyWrongPigment) + addWrongPigment;
            }
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            CombatManager.Instance.RemoveObserver(TryConsumeWearable, CustomEvents.MODIFY_WRONG_PIGMENT, caller);
        }
    }
}
