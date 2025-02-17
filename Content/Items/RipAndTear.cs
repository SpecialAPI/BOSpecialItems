using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class RipAndTear
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("Rip and Tear", "\"Enemies have a 20% cha- oh wait, wrong game\"", "Upon killing an enemy, apply 2 Fury and 2 Berserk to this party member.", "RipAndTear", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnKill;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = Conditions.Chance(50),
                    effect = CreateScriptable<ApplyFuryEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = Conditions.Previous(wasSuccessful: false),
                    effect = CreateScriptable<ApplyBerserkEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.ThisSlot
                },
            };
        }
    }
}
