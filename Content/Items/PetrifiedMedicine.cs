using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class PetrifiedMedicine
    {
        public static void Init()
        {
            var item = new GenericItem<PerformEffectWearable>("Petrified Medicine", "\"More grey pigment. You probably shouldn't be taking these\"", "Start combat with an additional 3 universal grey pigment.", "PetrifiedMedicine", ItemPools.Treasure);
            item.item.triggerOn = TriggerCalls.OnFirstTurnStart;
            item.item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ForceGeneratePigmentColorEffect>(x => x.pigmentColor = Pigments.Gray),
                    entryVariable = 3,
                    targets = null
                }
            };
            item.AddItem();
        }
    }
}
