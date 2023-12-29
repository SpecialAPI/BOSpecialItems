using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class FailedRound
    {
        public static void Init()
        {
            var failedround = new GenericItem<PerformEffectWearable>("Failed Round", "\"Misfire\"", "This party member is 1 level higher than they would be otherwise.\nOn combat start, inflict 1 TargetSwap to this party member", "FailRounds", ItemPools.Treasure);
            failedround.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<RankChange_Wearable_SMS>(x =>
                {
                    x._rankAdditive = 1;
                })
            };
            failedround.item.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplyTargetSwapEffect>(),
                    condition = null,
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            failedround.item.triggerOn = TriggerCalls.OnCombatStart;
            failedround.item.doesItemPopUp = true;
            failedround.AddItem();
            failedround.item.AttachGadget(GadgetDB.GetGadget("Trick or Treat"));
        }
    }
}
