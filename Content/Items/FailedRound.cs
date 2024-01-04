using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class FailedRound
    {
        public static void Init()
        {
            var failedround = NewItem<PerformEffectWearable>("Failed Round", "\"Misfire\"", "This party member is 1 level higher than they would be otherwise.\nOn combat start, inflict 1 TargetSwap to this party member", "FailRounds", ItemPools.Treasure);
            failedround.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<RankChange_Wearable_SMS>(x =>
                {
                    x._rankAdditive = 1;
                })
            };
            failedround.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplyTargetSwapEffect>(),
                    condition = null,
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            failedround.triggerOn = TriggerCalls.OnCombatStart;
            failedround.doesItemPopUp = true;
            failedround.AttachGadget(GadgetDB.GetGadget("Trick or Treat"));
        }
    }
}
