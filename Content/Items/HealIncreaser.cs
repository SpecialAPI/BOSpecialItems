using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class HealIncreaser
    {
        public static void Init()
        {
            NewItem<MultiCustomTriggerEffectWearable>("healing item", "", "all healing is set to 1", "Placeholder", ItemPools.Extra, 0).triggerEffects = new()
            {
                new EffectsAndCustomTriggerPair()
                {
                    customTrigger = CustomEvents.WILL_HEAL_UNIT,
                    conditions = null,
                    doesPopup = true,
                    effect = new SetHealingToOneTriggerEffect(),
                    getsConsumed = false,
                    immediate = true
                }
            };
        }
    }
}
