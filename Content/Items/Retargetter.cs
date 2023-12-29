namespace BOSpecialItems.Content.Items
{
    public static class Retargetter
    {
        public static void Init()
        {
            var tsLeft = CreateScriptable<TargetShiftPassive>(x => 
            {
                x.shift = -1;
                x.passiveIcon = LoadSprite("TargetShift_L");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_Left");
                x._passiveName = "TargetShift (Left)";
                x._characterDescription = "All actions performed by this party member are performed 1 slot to the left.";
                x._enemyDescription = "All actions performed by this enemy are performed 1 slot to the left.";
            });
            var tsRight = CreateScriptable<TargetShiftPassive>(x => 
            {
                x.shift = 1;
                x.passiveIcon = LoadSprite("TargetShift_R");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_Right");
                x._passiveName = "TargetShift (Right)";
                x._characterDescription = "All actions performed by this party member are performed 1 slot to the right.";
                x._enemyDescription = "All actions performed by this enemy are performed 1 slot to the right.";
            });
            var targetter = new GenericItem<BasicWearable>("Eyepatch", "\"Hit the same less often\"", "This party member now has TargetShift (Left) as a passive. Adds \"Retarget\" as an additional ability, an ability that allows this party member to change their TargetShift.", "Eyepatch", ItemPools.Treasure);
            targetter.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = tsLeft;
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
                            x.visuals = BrutalAPIPlugin.slapCharAbility.ability.visuals;
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
                                        x.passiveToRemove = tsLeft.type;
                                    }),
                                    entryVariable = 0,
                                    condition = null
                                },
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<RemovePassiveFromSelfEffect>(x =>
                                    {
                                        x.passiveToRemove = tsRight.type;
                                    }),
                                    entryVariable = 0,
                                    condition = null
                                },
                                new()
                                {
                                    targets = TargettingLibrary.ThisSlot,
                                    effect = CreateScriptable<AddPassiveToSelfEffect>(x =>
                                    {
                                        x.passiveToAdd = tsLeft;
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
                                        x.passiveToAdd = tsRight;
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
            targetter.AddItem();
            targetter.item.AttachGadget(GadgetDB.GetGadget("Deck of Wonder"));
        }
    }
}
