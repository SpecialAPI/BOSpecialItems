using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class LoudPhone
    {
        public static void Init()
        {
            var chance = 60;

            var loudphone = NewItem<PerformEffectWearable>("LoudPhone", "\"CAW CAW CAW\"", $"{chance}% chance to refresh this party member's abilities upon performing an ability. Inflict Weakened to this party member if they get refreshed.", "LoudPhone", ItemPools.Treasure);
            loudphone.triggerOn = TriggerCalls.OnAbilityUsed;
            loudphone.conditions = new EffectorConditionSO[]
            {
                CreateScriptable<PercentageEffectorCondition>(x => x.triggerPercentage = chance)
            };
            loudphone.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    targets = TargettingLibrary.ThisSlot,
                    condition = null,
                    entryVariable = 0
                },
                new()
                {
                    effect = CreateScriptable<ApplyWeakenedEffect>(),
                    targets = TargettingLibrary.ThisSlot,
                    condition = null,
                    entryVariable = 1
                }
            };
            loudphone.AttachGadget(GadgetDB.SetupGadget("CAW", "CAW CAW CAW", "BodyScream_A", TargettingLibrary.AllAlliesButThisVisual, Cost(Pigments.Grey, Pigments.Grey), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllAlliesButThis
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyWeakenedEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.AllAlliesButThis
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        GetEffectIntent<WeakenedStatusEffect>()
                    },
                    targets = TargettingLibrary.AllAlliesButThisVisual
                }
            }));
        }
    }
}
