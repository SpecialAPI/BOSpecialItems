using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class StrangeDevice
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("Strange Device", "\"157\"", "All random effects are now predetermined.", "StrangeDevice", ItemPools.Shop, 1, false);

            item.triggerOn = TriggerCalls.OnBeforeCombatStart;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<SetRandomSeedEffect>(),
                    entryVariable = 157,
                    targets = null
                }
            };
        }
    }
}
