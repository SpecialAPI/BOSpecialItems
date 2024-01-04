using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class TheTiderunner
    {
        public static void Init()
        {
            var tiderunner = NewItem<TheTiderunnerWearable>("The Tiderunner", "\"Smooth Sailing\"", "After performing the leftmost action, move this party member to the left.\nAfter performing the rightmost action, move this party member to the right.\nMoves \"Slap\" 1 slot to the right.", "TheTiderunner", ItemPools.Treasure);
            tiderunner.leftEffects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<SwapToOneSideEffect>(x =>
                    {
                        x._swapRight = false;
                    }),
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            tiderunner.rightEffects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<SwapToOneSideEffect>(x =>
                    {
                        x._swapRight = true;
                    }),
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            tiderunner.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = CreateScriptable<MoveSlapRightFlag>();
                })
            };
            tiderunner.doesItemPopUp = false;
            tiderunner.AttachGadget(GadgetDB.GetGadget("Ram"));
        }
    }
}
