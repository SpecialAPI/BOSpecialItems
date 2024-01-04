namespace BOSpecialItems.Content.Items
{
    public static class Retargetter
    {
        public static void Init()
        {
            var targetter = NewItem<BasicWearable>("Eyepatch", "\"Hit the same less often\"", "This party member now has TargetShift (Left) as a passive. Adds \"Retarget\" as an additional ability, an ability that allows this party member to change their TargetShift.", "Eyepatch", ItemPools.Treasure);
            targetter.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = CustomPassives.TargetShift_Left;
                }),
                CreateScriptable<ExtraAbility_Wearable_SMS>(x =>
                {
                    x._extraAbility = new()
                    {
                        cost = new ManaColorSO[]
                        {
                            Pigments.Yellow
                        },
                        ability = CreateScriptable<AbilitySO>(x =>
                        {
                            x.abilitySprite = LoadSprite("AttackIcon_Retarget");
                            x.visuals = LoadedAssetsHandler.GetCharacterAbility("Slap_A").visuals;
                            x._abilityName = "Retarget";
                            x._description = "Remove TargetShift (Left) and TargetShift (Right) from this party member.\nIf TargetShift (Left) was removed, add TargetShift (Right) to this party member. Otherwise, add TargetShift (Left).";
                            x.animationTarget = TargettingLibrary.ThisSlot;
                            x.intents = new IntentTargetInfo[]
                            {
                                new IntentTargetInfo()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    targetIntents = new IntentType[]
                                    {
                                        IntentType.Misc
                                    }
                                }
                            };
                            x.effects = new EffectInfo[]
                            {
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<RemovePassiveFromSelfEffect>(x =>
                                    {
                                        x.passiveToRemove = CustomPassives.TargetShift_Left.type;
                                    }),
                                    entryVariable = 0,
                                    condition = null
                                },
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<RemovePassiveFromSelfEffect>(x =>
                                    {
                                        x.passiveToRemove = CustomPassives.TargetShift_Right.type;
                                    }),
                                    entryVariable = 0,
                                    condition = null
                                },
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<AddPassiveToSelfEffect>(x =>
                                    {
                                        x.passiveToAdd = CustomPassives.TargetShift_Left;
                                    }),
                                    entryVariable = 0,
                                    condition = CreateScriptable<PreviousEffectCondition>(x =>
                                    {
                                        x.previousAmount = 2;
                                        x.wasSuccessful = false;
                                    })
                                },
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<AddPassiveToSelfEffect>(x =>
                                    {
                                        x.passiveToAdd = CustomPassives.TargetShift_Right;
                                    }),
                                    entryVariable = 0,
                                    condition = CreateScriptable<PreviousEffectCondition>(x =>
                                    {
                                        x.previousAmount = 1;
                                        x.wasSuccessful = false;
                                    })
                                }
                            };
                        })
                    };
                })
            };
            targetter.AttachGadget(GadgetDB.GetGadget("Deck of Wonder"));
        }
    }
}
