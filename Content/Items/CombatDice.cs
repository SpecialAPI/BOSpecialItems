using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class CombatDice
    {
        public static void Init()
        {
            var warriorDice = NewItem<BasicWearable>("Combat Dice", "\"Or is it called a douse?\"", "Replaces \"Slap\" with \"Combat Roll\", a pigment rerolling ability with a chance to refresh.\nAdds \"Fury\" as an additional ability, an ability that applies Fury to this party member.", "CombatDice", ItemPools.Treasure);
            warriorDice.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<BasicAbilityChange_Wearable_SMS>(x =>
                {
                    x._basicAbility = new()
                    {
                        cost = new ManaColorSO[] { Pigments.Grey },
                        ability = CreateScriptable<AbilitySO>(x =>
                        {
                            x._abilityName = "Combat Roll";
                            x._description = "Generates 1 pigment of a random color.\n75% chance to refresh.";
                            x.visuals = LoadedAssetsHandler.GetCharacterAbility("Insult_1_A").visuals;
                            x.animationTarget = TargettingLibrary.ThisSlot;
                            x.abilitySprite = LoadSprite("AttackIcon_CombatRoll");
                            x.intents = new IntentTargetInfo[]
                            {
                                new IntentTargetInfo()
                                {
                                    targetIntents = new IntentType[]
                                    {
                                        IntentType.Mana_Generate,
                                        IntentType.Misc
                                    },
                                    targets = TargettingLibrary.ThisSlot
                                }
                            };
                            x.effects = new EffectInfo[]
                            {
                                new EffectInfo()
                                {
                                    effect = CreateScriptable<GenerateRandomManaBetweenEffect>(x =>
                                    {
                                        x.possibleMana = new ManaColorSO[]
                                        {
                                            Pigments.Yellow,
                                            Pigments.Red,
                                            Pigments.Blue,
                                            Pigments.Purple
                                        };
                                    }),
                                    entryVariable = 1,
                                    condition = null,
                                    targets = null
                                },
                                new EffectInfo()
                                {
                                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                                    condition = CreateScriptable<PercentageEffectCondition>(x =>
                                    {
                                        x.percentage = 75;
                                    }),
                                    targets = TargettingLibrary.ThisSlot
                                }
                            };
                        })
                    };
                }),
                CreateScriptable<ExtraAbility_Wearable_SMS>(x =>
                {
                    x._extraAbility = new()
                    {
                        cost = new ManaColorSO[] { Pigments.Red, Pigments.Red, Pigments.Red },
                        ability = CreateScriptable<AbilitySO>(x =>
                        {
                            x._abilityName = "Fury";
                            x._description = "Apply 2 Fury to this party member.\nThis action can't be repeated by Fury.";
                            x.visuals = LoadedAssetsHandler.GetEnemyAbility("Unmake_A").visuals;
                            x.animationTarget = TargettingLibrary.ThisSlot;
                            x.abilitySprite = LoadSprite("AttackIcon_Fury");
                            x.intents = new IntentTargetInfo[]
                            {
                                new IntentTargetInfo()
                                {
                                    targetIntents = new IntentType[]
                                    {
                                        IntentType.Misc
                                    },
                                    targets = TargettingLibrary.ThisSlot
                                }
                            };
                            x.effects = new EffectInfo[]
                            {
                                new EffectInfo()
                                {
                                    effect = CreateScriptable<ApplyFuryEffect>(),
                                    condition = null,
                                    targets = TargettingLibrary.ThisSlot,
                                    entryVariable = 2
                                }
                            };
                            x.name = "SpecialExtra_Fury_NoFuryRepeat_A";
                        })
                    };
                })
            };
            warriorDice.AttachGadget(GadgetDB.GetGadget("Deck of Wonder"));
        }
    }
}
