using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public static class GadgetDB
    {
        public static ExtraAbilityInfo defaultGadget;
        public static Dictionary<string, ExtraAbilityInfo> gadgetDict = new();
        public static readonly Dictionary<string, ExtraAbilityInfo> allGadgets = new();

        public static ExtraAbilityInfo TryGetGadgetForItem(string itemname)
        {
            if(gadgetDict.TryGetValue(itemname, out var v) && v != null)
            {
                return v;
            }
            return defaultGadget;
        }

        public static void AttachGadget(this BaseWearableSO item, ExtraAbilityInfo gadget)
        {
            if (!gadgetDict.ContainsKey(item.name))
            {
                gadgetDict.Add(item.name, gadget);
            }
        }

        public static void Init()
        {
            defaultGadget = SetupGadget("Jack of All Trades", "Deal 6 damage to the opposing enemy.\nApply 4 shield to this party member's position.\n30% chance to refresh this party member's movement.", "Contusion_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.SplitPigment(Pigments.Red, Pigments.Blue), Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 6,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyShieldSlotEffect>(),
                    entryVariable = 4,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 30),
                    effect = CreateScriptable<RestoreSwapUseEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Shield,
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });

            var Gadget_Indulgence = SetupGadget("Indulgence", "Deals 2 damage to the Left and Right party members.", "Indulgence_A", TargettingLibrary.Relative(true, -1, 1), Cost(Pigments.Yellow), GetAnyAbility("Indulgence_A").effects.Copy(), GetAnyAbility("Indulgence_A").intents.Copy());
            var Gadget_Weep = SetupGadget("Weep", "Cries harder and produces 3 Blue Pigment.", "Weep_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), GetAnyAbility("Weep_A").effects.Copy(), GetAnyAbility("Weep_A").intents.Copy());
            var Gadget_Sell = SetupGadget("Sell", "Destroys the left ally's held item. If an item was destroyed, produce 5 coins.", "Absolve_1_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ConsumeItemEffect>(),
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<ExtraCurrencyEffect>(),
                    targets = null,
                    entryVariable = 5
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Misc_Currency
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_Devour = SetupGadget("Devour", "Destroys the left ally's held item. If an item was destroyed, fully heal this party member.", "Absolve_1_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ConsumeItemEffect>(),
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<FullHealEffect>(),
                    targets = TargettingLibrary.ThisSlot,
                    entryVariable = 5
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_21
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_WaitWhat = SetupGadget("Wait, What?", "Applies 1 TargetSwap to all party members and enemies.", "Amalgam_1_A", CreateScriptable<Targetting_AllUnits>(), Cost(Pigments.Green, Pigments.Green, Pigments.Green), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyTargetSwapEffect>(),
                    entryVariable = 1,
                    targets = CreateScriptable<Targetting_AllUnits>()
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = CreateScriptable<Targetting_AllUnits>()
                }
            });
            var Gadget_Flood = SetupGadget("Flood", "Vomits and produces 3 Pigment of this party member's health colour.", "Flood_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), GetAnyAbility("Flood_A").effects.Copy(), GetAnyAbility("Flood_A").intents.Copy());
            var Gadget_LuckyRoll = SetupGadget("Lucky Roll", "Attempt to produce lucky pigment 6 times.", "Flood_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                }
            }, GetAnyAbility("Flood_A").intents.Copy());
            var Gadget_JumbleGuts = SetupGadget("Jumbled Guts", "Produces 3 random pigment.", "Flood_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new EffectInfo()
                {
                    effect = CreateScriptable<GenerateRandomManaBetweenEffect>(x => x.possibleMana = new ManaColorSO[]
                    {
                        Pigments.Yellow,
                        Pigments.Red,
                        Pigments.Blue,
                        Pigments.Purple
                    }),
                    condition = null,
                    entryVariable = 3,
                    targets = null
                }
            }, GetAnyAbility("Flood_A").intents.Copy());
            var Gadget_Ram = SetupGadget("Ram", "This party member moves to the Left or Right 3 times. Deals 10 damage to the opposing enemy.", null, null, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x =>
                    {
                        x._animationTarget = TargettingLibrary.OpposingSlot;
                        x._visuals = GetAnyAbility("Crush_A").visuals;
                    })
                },
                new()
                {
                    condition = null,
                    entryVariable = 10,
                    targets = TargettingLibrary.OpposingSlot,
                    effect = CreateScriptable<DamageEffect>()
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Swap_Sides,
                        IntentType.Swap_Sides,
                        IntentType.Swap_Sides
                    },
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
            });
            var Gadget_Rush = SetupGadget("Rush", "This party member moves to the Left or Right 3 times. Heal the Left and Right allies 6 health.", null, null, Cost(Pigments.Blue, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot,
                    effect = CreateScriptable<SwapToSidesEffect>()
                },
                new()
                {
                    condition = null,
                    entryVariable = 0,
                    targets = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x =>
                    {
                        x._animationTarget = TargettingLibrary.Relative(true, -1, 1);
                        x._visuals = GetAnyAbility("Mend_1_A").visuals;
                    })
                },
                new()
                {
                    condition = null,
                    entryVariable = 6,
                    targets = TargettingLibrary.Relative(true, -1, 1),
                    effect = CreateScriptable<HealEffect>()
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Swap_Sides,
                        IntentType.Swap_Sides,
                        IntentType.Swap_Sides
                    },
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_5_10
                    },
                    targets = TargettingLibrary.Relative(true, -1, 1)
                },
            });
            var Gadget_Kingslayer = SetupGadget("Kingslayer", "Deals 7 damage to the opposing enemy. If damage was dealt, indirectly deal 7 damage to other spaces occupied by the opposing enemy", "FlayTheFlesh_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    targets = TargettingLibrary.OpposingSlot,
                    entryVariable = 7,
                    effect = CreateScriptable<KingslayerEffect>()
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_Bruise = SetupGadget("Bruise", "Deals 5 damage to this party member. Heals this party member 10 health.", "Malpractice_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 10,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6,
                        IntentType.Heal_5_10
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Cleanse = SetupGadget("Cleanse", "Remove all status effects from this party member.", "Mend_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<RemoveAllStatusEffectsEffect>(),
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Divinity = SetupGadget("Divinity", "Apply 3 Divine Protection to this party member. Apply 7 shield to this party member's position.", "Excommunicate_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyDivineProtectionEffect>(),
                    entryVariable = 3,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyShieldSlotEffect>(),
                    entryVariable = 7,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Status_DivineProtection,
                        IntentType.Field_Shield
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Trade = SetupGadget("Trade", "Destroys the left ally's held item. If an item was destroyed, produce a random treasure.", "Absolve_1_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ConsumeItemEffect>(),
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<ExtraLootEffect>(x => x._isTreasure = true),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_Hope = SetupGadget("Hope", "Exhaust all party members' abilities and movement. Shuffle all party members. Heal all party members 6 health.", "Concentration_A", TargettingLibrary.AllAlliesVisual, Cost(Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(x => x._doesExhaustInstead = true),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllAllies
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExhaustMovementEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllAllies
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<MassSwapZoneEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSide
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 6,
                    targets = TargettingLibrary.AllAllies
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Swap_Mass,
                        IntentType.Heal_5_10
                    },
                    targets = TargettingLibrary.AllAlliesVisual
                }
            });
            var Gadget_BrokenLeg = SetupGadget("Broken Leg", "Refresh this party member's movement. If successfull, refresh this party member's abilities.\nInflict 3 Frail to this party member.", "FallingSkies_A", TargettingLibrary.ThisSlot, Cost(Pigments.Red, Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<RestoreSwapUseEffect>(),
                    condition = null,
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    effect = CreateScriptable<ApplyFrailEffect>(),
                    condition = null,
                    entryVariable = 3,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Status_Frail
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_MissingPiece = SetupGadget("Missing Piece", "Make the left party member attempt to perform their missing ability.", null, null, Cost(Pigments.Yellow, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<PerformMissingAbilityEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_FreeWill = SetupGadget("Free Will", "Exhaust the left party member. If successfull, make them perform 2 random abilities.", null, null, Cost(Pigments.Yellow, Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(x => x._doesExhaustInstead = true),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<PerformRandomAbilityEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = true; }),
                    effect = CreateScriptable<PerformRandomAbilityEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1)
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_DeckOfWonder = SetupGadget("Deck of Wonder", "Perform 3 random item abilities back to back.", null, null, Cost(Pigments.Yellow, Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<PerformRandomItemAbilityEffect>(),
                    entryVariable = 3,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_SlapSwarm = SetupGadget("Slap Swarm", "Deal 1 damage to the opposing enemy 3 times.", "Slap_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x => { x._visuals = GetAnyAbility("Slap_A").visuals; x._animationTarget = TargettingLibrary.OpposingSlot; }),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x => { x._visuals = GetAnyAbility("Slap_A").visuals; x._animationTarget = TargettingLibrary.OpposingSlot; }),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_1_2,
                        IntentType.Damage_1_2,
                        IntentType.Damage_1_2
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_BashSwarm = SetupGadget("Bash Swarm", "Deal 5 damage to the opposing enemy 3 times.", "Bash_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x => { x._visuals = GetAnyAbility("Bash_A").visuals; x._animationTarget = TargettingLibrary.OpposingSlot; }),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AnimationVisualsEffect>(x => { x._visuals = GetAnyAbility("Bash_A").visuals; x._animationTarget = TargettingLibrary.OpposingSlot; }),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6,
                        IntentType.Damage_3_6,
                        IntentType.Damage_3_6
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_Maim = SetupGadget("Maim", "Deal 6 damage to the opposing enemy.\nInflict 1 Scar to the opposing enemy.", "Silence_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 6,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyScarsEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6,
                        IntentType.Status_Scars
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_CoinFlip = SetupGadget("Coinflip", "Attempt to produce lucky pigment. If successfull, deal 16 damage to the opposing enemy. Otherwise, apply 7 shield to the opposing enemy's position.", "Insult_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<TryProduceLuckyPigmentEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 16,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = false; }),
                    effect = CreateScriptable<ApplyShieldSlotEffect>(),
                    entryVariable = 7,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_16_20,
                        IntentType.Field_Shield
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Mana_Generate
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Tickle = SetupGadget("Tickle", "Deal 1 damage to the left ally. Heal 2 health to the left ally.\nRepeat this effect 2 times.", "PressurePoint_1_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.Relative(true, -1)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_1_2,
                        IntentType.Heal_1_4,
                        IntentType.Damage_1_2,
                        IntentType.Heal_1_4,
                        IntentType.Damage_1_2,
                        IntentType.Heal_1_4
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_Mend = SetupGadget("Mend", "Heal this party member and Left ally 4 health.", "Mend_1_A", TargettingLibrary.Relative(true, -1, 0), Cost(Pigments.Blue, Pigments.Blue), GetAnyAbility("Mend_2_A").effects.Copy(), GetAnyAbility("Mend_2_A").intents.Copy());
            var Gadget_WasteAway = SetupGadget("Waste Away", "Heal this party member 10 health.\nInflict Curse to this party member.", "WasteAway_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Purple, Pigments.Purple), GetAnyAbility("WasteAway_1_A").effects.Copy(), GetAnyAbility("WasteAway_1_A").intents.Copy());
            var Gadget_Fumes = SetupGadget("Fumes", "Deals 4 damage to All enemies.", "WasteAway_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), GetAnyAbility("Fumes_2_A").effects.Copy(), GetAnyAbility("Fumes_2_A").intents.Copy());
            var Gadget_FakeDeath = SetupGadget("Fake Death", "Instantly kills the left ally. If successfull, resurrect a random dead party member.", "Misery_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(x => { x._returnKillAsSuccess = true; x._indirect = true; }),
                    entryVariable = 9999,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DelayedEffectsPerformEffect>(x => x.effects = new EffectInfo[]
                    {
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<ResurrectEffect>(),
                            entryVariable = 1,
                            targets = TargettingLibrary.Relative(true, -1)
                        }
                    }),
                    entryVariable = 0,
                    targets = null
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_Death,
                        IntentType.Other_Resurrect
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_PestControl = SetupGadget("Pest Control", "Deals 20 damage equally distributed among the opposing enemy and all enemies connected to the opposing enemy.\nOnly damage dealt to the opposing enemy is direct.", "Fumes_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow, Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEqualDistributionEffect>(),
                    entryVariable = 20,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_16_20,
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_Lycanthropy = SetupGadget("Lycanthropy", "Exhausts this party member's movement and deals 5 damage to the opposing enemy. If movement was successfully exhausted, deals 15 damage instead.", "Chomp_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Blue, Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExhaustMovementEffect>(),
                    targets = TargettingLibrary.ThisSlot,
                    entryVariable = 0
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 15,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6,
                        IntentType.Damage_11_15
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_BodyScream = SetupGadget("Body Scream", "Inflict 1 Frail to the left, opposing and right enemies", "BodyScream_A", TargettingLibrary.Relative(false, -1, 0, 1), Cost(Pigments.Blue, Pigments.Blue, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyFrailEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.Relative(false, -1, 0, 1)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Status_Frail
                    },
                    targets = TargettingLibrary.Relative(false, -1, 0, 1)
                }
            });
            var Gadget_Howl = SetupGadget("Howl", "Reroll all abilities from the opposing enemy. Randomize all stored pigment.", "BodyScream_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow, Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ReRollTargetTimelineAbilityEffect>(),
                    targets = TargettingLibrary.OpposingSlot,
                    entryVariable = 9999
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RandomizeAllManaEffect>(x => x.manaRandomOptions = new ManaColorSO[] { Pigments.Red, Pigments.Blue, Pigments.Yellow, Pigments.Purple }),
                    entryVariable = 0,
                    targets = null
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Mana_Randomize
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_DumbDown = SetupGadget("Dumb Down", "Apply 1 Berserk to the left ally.\nInflict 2 Weakened to the left ally.", "Concentration_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyBerserkEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyWeakenedEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.Relative(true, -1)
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        GetEffectIntent<BerserkStatusEffect>(),
                        GetEffectIntent<WeakenedStatusEffect>(),
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_Smokescreen = SetupGadget("Smokescreen", "Attempt to remove 4 pigment. Apply an equivalent amount of shield to Left and Right allies and Self.", "Smokescreen_2_A", TargettingLibrary.Relative(true, -1, 0, 1), Cost(Pigments.Blue), GetAnyAbility("Smokescreen_2_A").effects.Copy(), GetAnyAbility("Smokescreen_2_A").intents.Copy());
            var Gadget_Huff = SetupGadget("Huff", "Remove all overflow. Reduce maximum health by 1. Heal self 2", "Huff_2_A", GetAnyAbility("Huff_2_A").animationTarget, Cost(Pigments.Yellow), GetAnyAbility("Huff_2_A").effects.Copy(), GetAnyAbility("Huff_2_A").intents.Copy());
            var Gadget_HereKitty = SetupGadget("Here Kitty", "Here kitty.", "HereKitty_A", GetAnyAbility("HereKitty_A").animationTarget, Cost(Pigments.Yellow, Pigments.Purple, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<SpawnEnemyAnywhereEffect>(x => { x.givesExperience = false; x.enemy = LoadedAssetsHandler.GetEnemy("Bronzo_Bananas_Friendly_EN"); x._spawnType = SpawnType.None; }),
                    entryVariable = 1,
                    targets = null
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_FishyBusiness = SetupGadget("Fishy Business", "Destroys the left ally's held item. If an item was destroyed, produce 3 \"Fish\".", "Mungle_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ConsumeItemEffect>(),
                    targets = TargettingLibrary.Relative(true, -1)
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<ExtraLootListEffect>(x =>
                    {
                        x._nothingPercentage = 0;
                        x._treasurePercentage = 1;
                        x._shopPercentage = 2;
                        var canOfWormsFishEffect = (LoadedAssetsHandler.GetWearable("CanOfWorms_SW") as PerformEffectWearable).effects[0].effect as ExtraLootListEffect;
                        x._lootableItems = canOfWormsFishEffect._lootableItems;
                        x._lockedLootableItems = canOfWormsFishEffect._lockedLootableItems;
                    }),
                    targets = null,
                    entryVariable = 3
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_WideShield = SetupGadget("Wide Shield", "Apply 7 Shield to this party member's position. Apply 3 Shield to the left and right positions.", "Entrenched_1_A", TargettingLibrary.Relative(true, -1, 0, 1), Cost(Pigments.Yellow, Pigments.Blue, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyShieldSlotEffect>(),
                    entryVariable = 7,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyShieldSlotEffect>(),
                    entryVariable = 3,
                    targets = TargettingLibrary.Relative(true, -1, 1)
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Shield
                    },
                    targets = TargettingLibrary.Relative(true, -1, 0, 1)
                }
            });
            var Gadget_PocketShop = SetupGadget("Pocket Shop", "Lose 5 coins.\nIf any coins were lost, produce a random shop item.", "Slap_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<LosePlayerCurrencyEffect>(x => x._loseFromPlayer = true),
                    entryVariable = 5,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<ExtraLootEffect>(x => x._isTreasure = false),
                    entryVariable = 1,
                    targets = null
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc_Currency,
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            }, showCurrency: true);
            var Gadget_ExpensiveHealthcare = SetupGadget("Expensive Healthcare", "Lose 5 coins.\nIf any coins were lost, fully heal the left ally as well as this party member.", "Mend_1_A", TargettingLibrary.Relative(true, -1, 0), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<LosePlayerCurrencyEffect>(x => x._loseFromPlayer = true),
                    entryVariable = 5,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<FullHealEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1, 0)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc_Currency,
                    },
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_21,
                    },
                    targets = TargettingLibrary.Relative(true, -1, 0)
                },
            }, showCurrency: true);
            var Gadget_CloningDevice = SetupGadget("Cloning Device", "Create a level 1 clone of this party member.", "InhumanRoar_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Purple, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyCasterAndSpawnCharacterAnywhereEffect>(x => { x._rank = 0; x._rankIsAdditive = false; x._permanentSpawn = false; x._maximizeHealth = false; x._canGetDeadCharacter = false; x._nameAddition = NameAdditionLocID.NameAdditionNot; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    entryVariable = 1,
                    targets = null
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Playtime = SetupGadget("Playtime", "Spawns a doll.", "Insult_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyAndSpawnCustomCharacterAnywhereEffect>(x => { x._characterCopy = "Doll_CH"; x._rank = 0; x._nameAddition = NameAdditionLocID.NameAdditionNone; x._permanentSpawn = false; x._usePreviousAsHealth = false; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_MordrakeMordrake = SetupGadget("Mordrake Mordrake", "Mordrake Mordrake Mordrake Mordrake.", "InhumanRoar_A", TargettingLibrary.ThisSlot, Cost(Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyAndSpawnCustomCharacterAnywhereEffect>(x => { x._characterCopy = "MiniMordrake_CH"; x._rank = 0; x._nameAddition = NameAdditionLocID.NameAdditionNone; x._permanentSpawn = false; x._usePreviousAsHealth = false; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Reliabash = SetupGadget("Reliabash", "Deals 9 damage to the opposing enemy.\nThis will always deal 9 damage, regardless of any passives, items, status effects or field effects.", "Contusion_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.SplitPigment(Pigments.Red, Pigments.Yellow)), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ReliableDamageEffect>(),
                    entryVariable = 9,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_BrutalOrchestra = SetupGadget("Brutal Orchestra", "Produce a random treasure item and make this party member immediately flee combat.\n\"This really is the brutal orchestra\"", "RejectDeath_A", null, Cost(Pigments.Yellow, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraLootEffect>(x => x._isTreasure = true),
                    entryVariable = 1,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageByCostEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<FleeTargetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_ThisIsFine = SetupGadget("This Is Fine", "Inflict 2 fire to all enemy positions.\nInflict 3 fire to this party member's position.\nInflict 2 Oil-Slicked to this party member.", "Sear_1_A", TargettingLibrary.OpposingSide, Cost(Pigments.Yellow, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyFireSlotEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.OpposingSide
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyFireSlotEffect>(),
                    entryVariable = 3,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyOilSlickedEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Fire
                    },
                    targets = TargettingLibrary.OpposingSide
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Fire,
                        IntentType.Status_OilSlicked
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_LiquidLullaby = SetupGadget("Liquid Lullaby", "Consumes 3 Pigment and heals an amount equal to the Pigment consumed.", "LiquidLullaby_A", TargettingLibrary.ThisSlot, Cost(), GetAnyAbility("LiquidLullaby_A").effects.Copy(), GetAnyAbility("LiquidLullaby_A").intents.Copy());
            var Gadget_PigmentGun = SetupGadget("Pigment Gun", "Consumes all pigment and deals damage to the opposing enemy equal to the pigment consumed.", "Flood_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ConsumeAllManaEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(x => x._usePreviousExitValue = true),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Mana_Consume
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_BountyHunter = SetupGadget("Bounty Hunter", "Deals 16 damage to the enemies with the highest health.", "Domination_A", CreateScriptable<TargettingStrongestUnit>(x => x.isAllies = false), Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 16,
                    targets = CreateScriptable<TargettingStrongestUnit>(x => x.isAllies = false)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_16_20
                    },
                    targets = TargettingLibrary.AllEnemiesVisual
                }
            });
            var Gadget_Invigorate = SetupGadget("Invigorate", "Refresh party members except this one. Deal 4 indirect damage to this party member for each party member refreshed.", "PressurePoint_1_A", CreateScriptable<Targetting_ByUnit_Side>(x => { x.ignoreCastSlot = true; x.getAllies = true; x.getAllUnitSlots = false; }), Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    entryVariable = 0,
                    targets = CreateScriptable<Targetting_ByUnit_Side>(x => { x.ignoreCastSlot = true; x.getAllies = true; x.getAllUnitSlots = false; })
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(x => { x._indirect = true; x._usePreviousExitValue = true; }),
                    entryVariable = 4,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = CreateScriptable<Targetting_ByUnit_Side>(x => { x.ignoreCastSlot = true; x.getAllies = true; x.getAllUnitSlots = false; })
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_16_20
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_Infinity = SetupGadget("Infinity", "Deals 6 damage to the opposing enemy.\nDeal 1 indirect damage and refresh this party member.\n50% chance to inflict 1 Scar to this party member.", "Cacophony_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 6,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(x => x._indirect = true),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 50),
                    effect = CreateScriptable<ApplyScarsEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_1_2,
                        IntentType.Status_Scars,
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_Cowardice = SetupGadget("Cowardice", "Heal this party member 4 health and make them immediately flee combat.", null, null, Cost(Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 4,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageByCostEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<FleeTargetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_1_4,
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Amalgam = SetupGadget("Amalgam", "Deal 9 damage to the Opposing enemy.\nChange the Opposing enemy's health colour to match this party member's.", "Amalgam_3_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow, Pigments.Yellow), GetAnyAbility("Amalgam_3_A").effects.Copy(), GetAnyAbility("Amalgam_3_A").intents.Copy());
            var Gadget_DivineSword = SetupGadget("Divine Sword", "Deal 12 damage to the opposing enemy.\nIf the opposing enemy has full health, do double damage.", "Domination_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Yellow, Pigments.Yellow, Pigments.Yellow, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DoubleDamageToFullHealthEffect>(),
                    entryVariable = 12,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_11_15,
                        IntentType.Damage_21
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_OneTooManyRestarts = SetupGadget("One Too Many Restarts", "Create a level 1 Nowak clone.", "Wrath_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyAndSpawnCustomCharacterAnywhereEffect>(x => { x._characterCopy = "Nowak_CH"; x._rank = 0; x._nameAddition = NameAdditionLocID.NameAdditionRestOf; x._permanentSpawn = false; x._usePreviousAsHealth = false; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_OneMansTrash = SetupGadget("One Man's Trash", "Produce a random shop item.\n50% chance to make this party member immediately flee combat.", "Insult_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraLootEffect>(x => x._isTreasure = false),
                    entryVariable = 1,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 50),
                    effect = CreateScriptable<ExtraVariableForNextEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageByCostEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = true; }),
                    effect = CreateScriptable<FleeTargetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Feast = SetupGadget("Feast", "Apply 1 Infestation to this party member, the left ally and the opposing enemy.\nIf there is no opposing enemy, apply 10 Frail, 5 Scars and 2 Ruptured to this party member.", "WrigglingWrath_A", TargettingLibrary.ThisSlot, Cost(Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddInfestationEffect>(x => x.infestation = Passives.Infestation(1)),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1, 0)
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddInfestationEffect>(x => x.infestation = Passives.Infestation(1)),
                    entryVariable = 0,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                    effect = CreateScriptable<ApplyFrailEffect>(),
                    entryVariable = 10,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = false; }),
                    effect = CreateScriptable<ApplyScarsEffect>(),
                    entryVariable = 5,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 3; x.wasSuccessful = false; }),
                    effect = CreateScriptable<ApplyRupturedEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.Relative(true, -1, 0)
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Status_Frail,
                        IntentType.Status_Scars,
                        IntentType.Status_Ruptured
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Unstoppable = SetupGadget("Unstoppable", "Reduce this party member's maximum health to this party member's current health value.\nDeal damage to the opposing enemy equal to twice the amount of maximum health removed.", "Concentration_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeMaxHealthByCurrentHealthEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageEffect>(x => x._usePreviousExitValue = true),
                    entryVariable = 2,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_MaxHealth
                    },
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_21
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
            });
            var Gadget_Consume = SetupGadget("Consume", "Reduce the opposing enemy's maximum health to the opposing enemy's current health value.\nHeal this party member an amount equal to 40% of the maximum health removed.", "Chomp_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Blue, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeMaxHealthByCurrentHealthEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<PreviousExitValuePercentageEffect>(),
                    entryVariable = 40,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = true; }),
                    effect = CreateScriptable<HealEffect>(x => x.usePreviousExitValue = true),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Other_MaxHealth
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_21
                    },
                    targets = TargettingLibrary.ThisSlot
                },
            });
            var Gadget_TrickOrTreat = SetupGadget("Trick or Treat", "Deal 12 damage to the opposing enemy. Inflicts 1 Fire to each enemy position.\n20% chance to backfire horribly.", "Buster_1_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 20),
                    effect = CreateScriptable<ExtraVariableForNextEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 12,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = false; }),
                    effect = CreateScriptable<ApplyFireSlotEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.OpposingSide
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 3; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 6,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraVariableForNextEffect>(),
                    entryVariable = 0,
                    targets = null
                },
                new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 5; x.wasSuccessful = true; }),
                    effect = CreateScriptable<ApplyRandomFireBetweenPreviousAndEntryEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSide
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_11_15
                    },
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_3_6
                    },
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Fire
                    },
                    targets = TargettingLibrary.ThisSide
                },
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Field_Fire
                    },
                    targets = TargettingLibrary.OpposingSide
                }
            });
            var Gadget_UselessMachine = SetupGadget("Useless Machine", "Refresh this party member.\n25% chance to inflict 1 Scar to this party member.", null, null, Cost(), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<RefreshAbilityUseEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 25),
                    effect = CreateScriptable<ApplyScarsEffect>(),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Status_Scars
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Concentrate = SetupGadget("Concentrate", "Turns all stored pigment into universal grey pigment.", "Concentration_A", TargettingLibrary.ThisSlot, Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ModifyManaNotOfSpecificColorEffect>(x => { x.options = Cost(Pigments.Grey); x.excludeColor = Pigments.Grey; }),
                    entryVariable = 0,
                    targets = null
                },
                /*new()
                {
                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                    effect = CreateScriptable<DamageEffect>(x => { x._usePreviousExitValue = true; x._indirect = true; }),
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }*/
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Mana_Modify,
                        //IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Blow = SetupGadget("Blow", "Deal between 0-20 damage to the Opposing enemy.", "Blow_2_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), GetAnyAbility("Blow_2_A").effects.Copy(), GetAnyAbility("Blow_2_A").intents.Copy());
            var Gadget_Entwined = SetupGadget("Entwined", "Inflict 2 Linked to the Left and Right enemies.", "Entwined_1_A", TargettingLibrary.Relative(false, -1, 1), Cost(Pigments.Red), GetAnyAbility("Entwined_1_A").effects.Copy(), GetAnyAbility("Entwined_1_A").intents.Copy());
            var Gadget_TemporarySolution = SetupGadget("Temporary Solution", "Heal this party member 8 health.\nReduce this party member's maximum health by 3.\nReduce this party member's maximum health to this party member's current health value.", "WasteAway_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<HealEffect>(),
                    entryVariable = 8,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeMaxHealthEffect>(x => x._increase = false),
                    entryVariable = 3,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeMaxHealthByCurrentHealthEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Heal_5_10,
                        IntentType.Other_MaxHealth
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Mutilate = SetupGadget("Mutilate", "Inflict 2 Linked to the opposing enemy.\nDeal 8 damage to the opposing enemy.", "Mutilate_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Red, Pigments.Red), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyLinkedEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.OpposingSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageEffect>(),
                    entryVariable = 8,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Status_Linked,
                        IntentType.Damage_7_10
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_Hint = SetupGadget("Hint", "Apply 2 Fury to this party member.\nThis action can't be repeated by Fury.\n\"You can do it!\"", "Concentration_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Yellow), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyFuryEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.ThisSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        GetEffectIntent<FuryStatusEffect>()
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            }, false);
            var Gadget_SmartUp = SetupGadget("Smart Up", "Apply 2 Powered Up to the left ally.", "PressurePoint_1_A", TargettingLibrary.Relative(true, -1), Cost(Pigments.Yellow, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ApplyPoweredUpEffect>(),
                    entryVariable = 2,
                    targets = TargettingLibrary.Relative(true, -1)
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        GetEffectIntent<PoweredUpStatusEffect>()
                    },
                    targets = TargettingLibrary.Relative(true, -1)
                }
            });
            var Gadget_TapeWormDiet = SetupGadget("Tape Worm Diet", "This party member immediately flees combat.\nSpawns a permanent Gall Gagger.", "Drink_1_A", TargettingLibrary.ThisSlot, Cost(Pigments.Yellow, Pigments.Yellow, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageByCostEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<FleeTargetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyAndSpawnCustomCharacterAnywhereEffect>(x => { x._characterCopy = "GallGagger_CH"; x._rank = 0; x._nameAddition = NameAdditionLocID.NameAdditionNone; x._permanentSpawn = true; x._usePreviousAsHealth = false; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });
            var Gadget_Starvation = SetupGadget("Starvation", "Deal 16 damage to the opposing enemy.\nIf the opposing enemy is a fish, deal 40 damage instead.", "Chomp_A", TargettingLibrary.OpposingSlot, Cost(Pigments.Blue, Pigments.Blue, Pigments.Blue), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DealDamageByUnitTypeEffect>(x => { x.damageToUnitType = 40; x.targetUnitType = UnitType.Fish; }),
                    entryVariable = 16,
                    targets = TargettingLibrary.OpposingSlot
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Damage_16_20,
                        IntentType.Damage_21
                    },
                    targets = TargettingLibrary.OpposingSlot
                }
            });
            var Gadget_StuntDouble = SetupGadget("Stunt Double", "This party member immediately flees combat and is replaced by a copy with the same health that is 1 level lower.", null, null, Cost(Pigments.Yellow, Pigments.Purple), new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<DamageByCostEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<FleeTargetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.ThisSlot
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<CopyCasterAndSpawnCharacterAnywhereEffect>(x => { x._rank = -1; x._rankIsAdditive = true; x._permanentSpawn = false; x._maximizeHealth = false; x._canGetDeadCharacter = true; x._nameAddition = NameAdditionLocID.NameAdditionNone; x._extraModifiers = new WearableStaticModifierSetterSO[0]; }),
                    targets = null,
                    entryVariable = 1
                }
            }, new IntentTargetInfo[]
            {
                new()
                {
                    targetIntents = new IntentType[]
                    {
                        IntentType.Misc,
                        IntentType.Other_Spawn
                    },
                    targets = TargettingLibrary.ThisSlot
                }
            });

            gadgetDict = new()
            {
                { "AGift_TW", Gadget_Devour },
                { "AllThatIsMortal_TW", Gadget_Concentrate },
                { "Ampoule_SW", Gadget_Flood },
                { "AnotherDud_SW", Gadget_Tickle },
                { "AsceticEgg_TW", Gadget_Trade },
                { "Atabrine_SW", Gadget_Flood },
                { "BarelyUsedGauze_SW", Gadget_Bruise },
                { "BlackenedToad_TW", Gadget_PigmentGun },
                { "BlindFaith_TW", Gadget_Hope },
                { "BloatingCoffers_TW", Gadget_Sell },
                { "BloodBottle_SW", Gadget_Flood },
                { "BlueBible_TW", Gadget_Divinity },
                { "BoxOfMedals_SW", Gadget_Maim },
                { "BrigadeOfDis_TW", Gadget_DeckOfWonder },
                { "BrokenHammer_TW", Gadget_BashSwarm },
                { "CanOfWorms_SW", Gadget_FishyBusiness },
                { "CaretakersCudgel_TW", Gadget_Ram },
                { "CertificateOfExemption_SW", Gadget_Concentrate },
                { "ColonCoins_TW", Gadget_Sell },
                { "ConscriptionNotice_SW", Gadget_WaitWhat },
                { "ConsolationPrize_SW", Gadget_Invigorate },
                { "ConvergentRage_TW", Gadget_Unstoppable },
                { "CounterfeitMedal_SW", Gadget_Maim },
                { "Cremation_TW", Gadget_ThisIsFine },
                { "CrookedDagger_TW", Gadget_Consume },
                { "CrookedDie_TW", Gadget_CoinFlip },
                { "CzechHedgehog_SW", Gadget_Tickle },
                { "DDT_SW", Gadget_WasteAway },
                { "DefacedScripture_TW", Gadget_Mutilate },
                { "DefectiveRounds_SW", Gadget_Blow },
                { "DemonCore_SW", Gadget_Tickle },
                { "DewormingPills_SW", Gadget_PestControl },
                { "DivineMud_TW", Gadget_Divinity },
                { "DriedPaint_SW", Gadget_Smokescreen },
                { "EdsTags_SW", Gadget_Cowardice },
                { "EffigyOfTheMettleMother_TW", Gadget_ThisIsFine },
                { "ExtraStitching_SW", Gadget_Bruise },
                { "FirstAid_SW", Gadget_Mend },
                { "FishingRod_TW", Gadget_FishyBusiness },
                { "FistFullOfAsh_TW", Gadget_Huff },
                { "FlyPaper_SW", Gadget_LuckyRoll },
                { "FunnelHelmet_TW", Gadget_WideShield },
                { "FunnyMushrooms_TW", Gadget_Flood },
                { "GamblersRightHand_TW", Gadget_CoinFlip },
                { "GamifiedSquid_TW", Gadget_FishyBusiness },
                { "GildedMirror_TW", Gadget_WaitWhat },
                { "GlueTrap_SW", Gadget_LuckyRoll },
                { "GospelsSeveredHead_TW", Gadget_Divinity },
                { "GreatWhite_ExtraW", Gadget_Devour },
                { "GrumpyGump_TW", Gadget_Tickle },
                { "GumpMingGoa_TW", Gadget_Tickle },
                { "Guppy_ExtraW", Gadget_Devour },
                { "HeadOfScribe_TW", Gadget_BodyScream },
                { "HealthInsurance_SW", Gadget_ExpensiveHealthcare },
                { "Hereafter_TW", Gadget_StuntDouble },
                { "HolyChalice_TW", Gadget_Divinity },
                { "HowlingLong_TW", Gadget_Howl },
                { "Ichthys_TW", Gadget_DeckOfWonder },
                { "IdeaOfEvil_TW", Gadget_DeckOfWonder },
                { "ImmolatedFairy_TW", Gadget_DeckOfWonder },
                { "Indulgence_TW", Gadget_Indulgence },
                { "IronHelmet_TW", Gadget_WideShield },
                { "IronNecklace_SW", Gadget_Cleanse },
                { "LadyGloves_SW", Gadget_DeckOfWonder },
                { "LadyPills_SW", Gadget_Flood },
                { "LilHomunculus_TW", Gadget_Cleanse },
                { "LittleKnife_TW", Gadget_PestControl },
                { "LuckyHelmet_SW", Gadget_WideShield },
                { "LumpOfLamb_TW", Gadget_CloningDevice },
                { "LustPudding_TW", Gadget_BountyHunter },
                { "LycanthropesCore_TW", Gadget_Lycanthropy },
                { "ManMadeOvum_TW", Gadget_DeckOfWonder },
                { "MeatreWorm_TW", Gadget_Devour },
                { "MeatreWormEaten_ExtraW", Gadget_Devour },
                { "MeatreWormSkeleton_ExtraW", Gadget_Devour },
                { "MedicalLeeches_SW", Gadget_LiquidLullaby },
                { "MiniMordrake_TW", Gadget_MordrakeMordrake },
                { "ModernMedicine_SW", Gadget_DumbDown },
                { "MommaNooty_TW", Gadget_LiquidLullaby },
                { "MysteryRation_SW", Gadget_FakeDeath },
                { "Norris_TW", Gadget_BashSwarm },
                { "NorrisBad_ExtraW", Gadget_SlapSwarm },
                { "OlReliable_SW", Gadget_Reliabash },
                { "OlStumpy_SW", Gadget_Cleanse },
                { "OpulentEgg_TW", Gadget_DivineSword },
                { "PainKillers_SW", Gadget_TemporarySolution },
                { "PegLeg_TW", Gadget_BrokenLeg },
                { "PepPowder_SW", Gadget_Tickle },
                { "PlasterCast_SW", Gadget_Cleanse },
                { "PontiffsParade_TW", Gadget_Divinity },
                { "Prosthetics_SW", Gadget_SmartUp },
                { "PrussianBlue_SW", Gadget_Fumes },
                { "PurpleHeart_SW", Gadget_FakeDeath },
                { "RAM_TW", Gadget_Ram },
                { "RecalledGasMask_SW", Gadget_Huff },
                { "RibOfEve_TW", Gadget_DeckOfWonder },
                { "RightShoe_ExtraW", Gadget_WideShield },
                { "RoentgenRays_SW", Gadget_Invigorate },
                { "RorschachTest_SW", Gadget_Amalgam },
                { "RotundAmphibian_TW", Gadget_Rush },
                { "RuntyRotter_TW", Gadget_Smokescreen },
                { "RusskiVampire_SW", Gadget_Bruise },
                { "SafeAndSound_SW", Gadget_Trade },
                { "Salmon_ExtraW", Gadget_Devour },
                { "SculpturesTools_SW", Gadget_Amalgam },
                { "SeedsOfTheConsumed_TW", Gadget_Rush },
                { "SerpentsHead_TW", Gadget_Invigorate },
                { "ShardOfNowak_TW", Gadget_DivineSword },
                { "Skates_TW", Gadget_Rush },
                { "SkinnedSkate_TW", Gadget_DeckOfWonder },
                { "SoggyBandages_SW", Gadget_Bruise },
                { "SomeoneElsesFace_SW", Gadget_MissingPiece },
                { "SomeonesWeddingRing_TW", Gadget_Divinity },
                { "SpikedCollar_TW", Gadget_DeckOfWonder },
                { "SplatterMask_SW", Gadget_Cleanse },
                { "SpringTrap_SW", Gadget_LuckyRoll },
                { "Stigmata_TW", Gadget_Invigorate },
                { "StillbornEgg_TW", Gadget_FakeDeath },
                { "StolenGold_SW", Gadget_Sell },
                { "StrangeFruit_TW", Gadget_DeckOfWonder },
                { "SulfaPowder_SW", Gadget_WideShield },
                { "Surrender_SW", Gadget_Cowardice },
                { "SweetHeart_SW", Gadget_Divinity },
                { "TaintedApple_TW", Gadget_Devour },
                { "TakePennyLeavePenny_SW", Gadget_Sell },
                { "TheApple_TW", Gadget_Devour },
                { "TheAsymptote_ExtraW", Gadget_Infinity },
                { "TheBrand_TW", Gadget_DeckOfWonder },
                { "TheFirstBorn_TW", Gadget_BountyHunter },
                { "TheHumanSoul_TW", Gadget_MissingPiece },
                { "TheIdealFormOfTrash_TW", Gadget_OneMansTrash },
                { "TheInfiniteJersey_ExtraW", Gadget_Infinity },
                { "TheJersey_TW", Gadget_Infinity },
                { "TheRestOfNowak_TW", Gadget_OneTooManyRestarts },
                { "TondalsVision_TW", Gadget_DeckOfWonder },
                { "TrenchFoot_SW", Gadget_LuckyRoll },
                { "Trepanation_TW", Gadget_Entwined },
                { "UnfortunateProphecy_TW", Gadget_DeckOfWonder },
                { "UsedBandAids_SW", Gadget_Mend },
                { "UsedNeedle_SW", Gadget_SlapSwarm },
                { "ViolatedPact_TW", Gadget_WideShield },
                { "WheelOfFortune_TW", Gadget_FreeWill },
                { "WickerChild_TW", Gadget_FakeDeath },
                { "YouCanDoIt_SW", Gadget_Hint },
                { "VyacheslavsLastSip_SW", Gadget_WaitWhat },
                { "TheMastersSickle_SW", Gadget_DeckOfWonder },
                { "EsotericArtifact_SW", Gadget_WaitWhat },
                { "TapeWormPills_SW", Gadget_TapeWormDiet },
                { "FaultyLandMine_SW", Gadget_Ram },
                { "ChainofCommand_SW", Gadget_Kingslayer },
                { "GentlemensGlove_SW", Gadget_DeckOfWonder },
                { "BalticBrine_SW", Gadget_Weep },
                { "HomelessHotline_SW", Gadget_WaitWhat },
                { "LitteringLeaflets_SW", Gadget_JumbleGuts },
                { "ForgottenPump_SW", Gadget_JumbleGuts },
                { "DumDum_SW", Gadget_Maim },
                { "ExpiredMedicine_SW", Gadget_JumbleGuts },
                { "Soap_SW", Gadget_Sell },
                { "PharmaceuticalRollerCoaster_SW", Gadget_CoinFlip },
                { "Vowbreaker_SW", Gadget_Concentrate },
                { "LilSmiley_SW", Gadget_Amalgam },
                { "UsedDogTags_SW", Gadget_WasteAway },
                { "WarBond_SW", Gadget_PocketShop },
                { "BrokenDoll_TW", Gadget_Playtime },
                { "CursedSword_TW", Gadget_DeckOfWonder },
                { "InfernalEye_TW", Gadget_Cleanse },
                { "Enigma_TW", Gadget_DeckOfWonder },
                { "LilOrro_TW", Gadget_Starvation },
                { "SacrificialSaint_TW", Gadget_FakeDeath },
                { "ProfessionalProcrastinator_TW", Gadget_UselessMachine },
                { "BloodBreathingBomb_TW", Gadget_TrickOrTreat },
                { "StarvingApples_TW", Gadget_Feast },
                { "EggOfFirmament_TW", Gadget_Trade },
                { "CounterfeitCoin_SW", Gadget_Sell },
                { "Bronzos2Cents_SW", Gadget_Sell },
                { "Bananas_TW", Gadget_Devour },
                { "BananasCat_TW", Gadget_HereKitty },
                { "BronzosStupidHat_TW", Gadget_Sell },
                { "ABananaPeel_ExtraW", Gadget_Devour },
                { "WelsCatfish_ExtraW", Gadget_Devour },
                { "LeftShoe_ExtraW", Gadget_WideShield },
                { "HarvestAndPlenty_TW", Gadget_LuckyRoll },
                { "ExquisiteCorpse_TW", Gadget_BrutalOrchestra },
                { "EggOfIncubus_TW", Gadget_BrutalOrchestra },
                { "EggOfIncubusCracked_ExtraW", Gadget_BrutalOrchestra },
                { "ExtremelyUnfinishedHeir_ExtraW", Gadget_BrutalOrchestra },
            };
        }

        public static Sprite gadgetSprite = LoadSprite("AttackIcon_Mirror");

        public static ExtraAbilityInfo GetGadget(string name)
        {
            if(allGadgets.TryGetValue(name.ToLowerInvariant(), out var g))
            {
                return g;
            }
            return null;
        }

        public static ExtraAbilityInfo SetupGadget(string name, string description, string visualsBase, BaseCombatTargettingSO animTarget, ManaColorSO[] cost, EffectInfo[] effects, IntentTargetInfo[] intents, bool allowFuryRepeat = true, bool allowFuryTick = true, bool showCurrency = false, AttackVisualsSO overrideVisuals = null)
        {
            if(GetGadget(name) != null)
            {
                return GetGadget(name);
            }
            ExtraAbilityInfo g = new()
            {
                cost = cost,
                ability = CreateScriptable<AbilitySO>(x =>
                {
                    x.abilitySprite = gadgetSprite;
                    x.intents = intents;
                    x.visuals = overrideVisuals ?? (!string.IsNullOrEmpty(visualsBase) ? (LoadedAssetsHandler.GetCharacterAbility(visualsBase) ?? LoadedAssetsHandler.GetEnemyAbility(visualsBase) ?? GetBossAbility(visualsBase)).visuals : null);
                    x.animationTarget = animTarget;
                    x.effects = effects;
                    x._abilityName = name;
                    x._description = description;
                    x.name = $"Gadget_{name.Replace(" ", "").Replace(",", "").Replace("?", "")}{(allowFuryRepeat ? (allowFuryTick ? "" : "_NoFuryTick") : "_NoFuryRepeat")}{(showCurrency ? "_ShowCurrencyOnObtain" : "")}_A";
                })
            };
            allGadgets[name.ToLowerInvariant()] = g;
            return g;
        }
    }
}
