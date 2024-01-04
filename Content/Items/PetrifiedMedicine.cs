using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class PetrifiedMedicine
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("Petrified Medicine", "\"More grey pigment. You probably shouldn't be taking these\"", "Start combat with an additional 3 universal grey pigment.", "PetrifiedMedicine", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnFirstTurnStart;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ForceGeneratePigmentColorEffect>(x => x.pigmentColor = Pigments.Grey),
                    entryVariable = 3,
                    targets = null
                }
            };
        }
    }
}
