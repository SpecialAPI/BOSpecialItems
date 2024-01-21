using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class AllSeeingEye
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("O OO00 OO00 - O00OO O0O O0O O00O OOO0 OOO 0 O0O OO00O O0O", "\"O00O O0O00 0 OO OOOO O00O0 O00O0 O0O0O O0000 O0O00 O00OO\"", "OOOO OOO0 0 OO OOOO OO0O O0 O O0O00 0 O0O OOO0 O00 , 0 O O00 O00 O00OO 0 O 0 O000 O O00O0 O0 O00O OOO0 OOO O0O O00O0 0 O O00OO 0 O 0 O0000 O0O O00O0 OO0O O OOO0 O0O OOO0 O0O00 0 O0000 O O00O0 O0O00 OO00O 0 OO0O O0O OO0O O0 O0O O00O0 . 0 O0O00 O000 O00O O00OO 0 O00O O0O00 O0O OO0O 0 O00O O00OO 0 O00 O0O O00OO O0O00 O00O0 OOOO OO00O O0O O00 0 O0O0O O0000 OOOO OOO0 0 O OO O0O00 O00O O0OO0 O O0O00 O00O OOOO OOO0 .\n[ PUT THIS BACK DOWN WHILE YOU STILL HAVE THE CHANCE. ]", "AllSeeingEye", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnCombatEnd;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    entryVariable = 1,
                    condition = null,
                    effect = CreateScriptable<GainLootRandomCustomCharacterEffect>(x => { x.possibleCharacters = Baba(); x.nameAddition = NameAdditionLocID.NameAdditionNone; x.rank = 0; }),
                    targets = null
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraLootOptionsEffect>(x => { x._changeOption = true; x._itemName = item.name; })
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraLootOptionsEffect>(x => { x._changeOption = true; x._itemName = item.name; })
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ExtraLootOptionsEffect>(x => { x._changeOption = true; x._itemName = item.name; })
                },
            };
            item.getsConsumedOnUse = true;
        }

        public static List<string> Baba()
        {
            var baba = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Baba_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Baba");
                x.characterBackSprite = LoadSprite("BabaBack");
                x.characterOWSprite = LoadSprite("BabaOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Baba";
                x.characterEntityID = ExtendEnum<EntityIDs>("Baba");
                x.healthColor = Pigments.Grey;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 12,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Grey),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Stunning Throw";
                                    x._description = "25% chance to cancel a random ability from the opposing enemy.\nMove the opposing enemy to the left or right 5 times.";
                                    x.visuals = GetAnyAbility("Parry_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 25),
                                            effect = CreateScriptable<RemoveTargetTimelineAbilityEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.OpposingSlot
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<SwapToSidesXTimesEffect>(),
                                            entryVariable = 5,
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc,
                                                IntentType.Swap_Sides,
                                                IntentType.Swap_Sides,
                                                IntentType.Swap_Sides,
                                                IntentType.Swap_Sides,
                                                IntentType.Swap_Sides,
                                            }
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_BabaThrow");
                                })
                            }
                        }
                    }
                };
            });
            var keke = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Keke_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Keke");
                x.characterBackSprite = LoadSprite("KekeBack");
                x.characterOWSprite = LoadSprite("KekeOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Keke";
                x.characterEntityID = ExtendEnum<EntityIDs>("Keke");
                x.healthColor = Pigments.Red;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 10,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Red, Pigments.Red),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Kick Away";
                                    x._description = "Deal 9 damage to the opposing enemy.\nMoves the opposing enemy as far as possible to the Left or Right.";
                                    x.visuals = GetAnyAbility("Parry_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 9,
                                            targets = TargettingLibrary.OpposingSlot
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<SwapToOneRandomSideXTimesEffect>(),
                                            entryVariable = 4,
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_7_10,
                                                IntentType.Swap_Mass
                                            },
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_KekeKick");
                                })
                            }
                        }
                    }
                };
            });
            var me = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Me_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Me");
                x.characterBackSprite = LoadSprite("MeBack");
                x.characterOWSprite = LoadSprite("MeOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Me";
                x.characterEntityID = ExtendEnum<EntityIDs>("Me");
                x.healthColor = Pigments.Purple;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 16,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Purple, Pigments.Red),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Blast Radius";
                                    x._description = "Deal 20 damage to the opposing enemy. If successful, deal 5 damage to this party member. Damage spreads indirectly to the left and right.";
                                    x.visuals = GetAnyAbility("Crush_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageOnDoubleCascadeEffect>(x =>
                                            {
                                                x._cascadeIsIndirect = true;
                                                x._decreaseAsPercentage = true;
                                                x._cascadeDecrease = 50;
                                                x._consistentCascade = true;
                                            }),
                                            entryVariable = 20,
                                            targets = TargettingLibrary.Relative(false, 0, -1, 1, -2, 2, -3, 3, -4, 4)
                                        },
                                        new()
                                        {
                                            condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                                            effect = CreateScriptable<DamageOnDoubleCascadeEffect>(x =>
                                            {
                                                x._cascadeIsIndirect = true;
                                                x._decreaseAsPercentage = true;
                                                x._cascadeDecrease = 50;
                                                x._consistentCascade = true;
                                            }),
                                            entryVariable = 6,
                                            targets = TargettingLibrary.Relative(true, 0, -1, 1, -2, 2, -3, 3, -4, 4)
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_16_20,
                                            },
                                            targets = TargettingLibrary.OpposingSlot
                                        },
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_3_6,
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_MeBomb");
                                })
                            }
                        }
                    }
                };
            });
            var jiji = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Jiji_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Jiji");
                x.characterBackSprite = LoadSprite("JijiBack");
                x.characterOWSprite = LoadSprite("JijiOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Jiji";
                x.characterEntityID = ExtendEnum<EntityIDs>("Jiji");
                x.healthColor = Pigments.Yellow;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 12,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Red, Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "RAM!!!";
                                    x._description = "Refresh this party member's movement. If movement wasn't refreshed, apply 1 Movement Charge to this party member.\nApply 1 Ram Charge to this party member.";
                                    x.visuals = GetAnyAbility("Struggle_A").visuals;
                                    x.animationTarget = TargettingLibrary.ThisSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RestoreSwapUseEffect>(),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                                            effect = CreateScriptable<ApplyMovementChargeEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<ApplyRamChargeEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc,
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_JijiCharge");
                                })
                            }
                        }
                    }
                };
            });
            var fofo = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Fofo_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Fofo");
                x.characterBackSprite = LoadSprite("FofoBack");
                x.characterOWSprite = LoadSprite("FofoOW", 32, new(0.5f, 0f));

                x._characterName = "Fofo";
                x.characterEntityID = ExtendEnum<EntityIDs>("Fofo");
                x.healthColor = Pigments.SplitPigment(Pigments.Green, Pigments.Purple);
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 8,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.SplitPigment(Pigments.Green, Pigments.Blue), Pigments.SplitPigment(Pigments.Green, Pigments.Red)),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Tractor Beam";
                                    x._description = "Move all enemies closer to this party member. Deal 10 damage to the opposing enemy.";
                                    x.visuals = GetAnyAbility("PressurePoint_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = true),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.Relative(false, -1, -2, -3, -4)
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.Relative(false, 1, 2, 3, 4)
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 10,
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
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
                                                IntentType.Swap_Right
                                            },
                                            targets = TargettingLibrary.Relative(false, -1, -2, -3, -4)
                                        },
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Swap_Left
                                            },
                                            targets = TargettingLibrary.Relative(false, 1, 2, 3, 4)
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_FofoTractor");
                                })
                            }
                        }
                    }
                };
                x.passiveAbilities = new BasePassiveAbilitySO[]
                {
                    CreateScriptable<PerformEffectPassiveAbility>(x =>
                    {
                        x._passiveName = "Bloom";
                        x._characterDescription = "At the start of each turn, this party member's health color is changed to a random base color split with almost unusable green pigment.";
                        x._enemyDescription = "At the start of each turn, this enemy's health color is changed to a random base color split with almost unusable green pigment.";
                        x.conditions = null;
                        x._triggerOn = new TriggerCalls[] { TriggerCalls.OnTurnStart };
                        x.doesPassiveTriggerInformationPanel = true;
                        x.passiveIcon = LoadSprite("Bloom");
                        x.type = ExtendEnum<PassiveAbilityTypes>("Bloom");

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ChangeToRandomHealthColorEffect>(x => x._healthColors = new ManaColorSO[]
                                {
                                    Pigments.SplitPigment(Pigments.Green, Pigments.Red),
                                    Pigments.SplitPigment(Pigments.Green, Pigments.Blue),
                                    Pigments.SplitPigment(Pigments.Green, Pigments.Yellow),
                                    Pigments.SplitPigment(Pigments.Green, Pigments.Purple),
                                }),
                                entryVariable = 0,
                                targets = TargettingLibrary.ThisSlot
                            }
                        };
                    })
                };
            });
            var it = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_It_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("It");
                x.characterBackSprite = LoadSprite("ItBack");
                x.characterOWSprite = LoadSprite("ItOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "It";
                x.characterEntityID = ExtendEnum<EntityIDs>("It");
                x.healthColor = Pigments.Blue;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 8,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Blue, Pigments.Blue),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Jump";
                                    x._description = "Refresh this party member's movement. If movement wasn't refreshed, apply 1 Movement Charge to this party member.\nApply 1 Jump Charge to this party member.";
                                    x.visuals = GetAnyAbility("Struggle_A").visuals;
                                    x.animationTarget = TargettingLibrary.ThisSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RestoreSwapUseEffect>(),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                                            effect = CreateScriptable<ApplyMovementChargeEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<ApplyJumpChargeEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc,
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_ItJump");
                                })
                            }
                        }
                    }
                };
            });
            var bird = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Bird_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Bird");
                x.characterBackSprite = LoadSprite("BirdBack");
                x.characterOWSprite = LoadSprite("BirdOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Bird";
                x.characterEntityID = ExtendEnum<EntityIDs>("Bird");
                x.healthColor = Pigments.Purple;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 8,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Purple),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Switcheroo";
                                    x._description = "Refresh this party member's movement. If movement wasn't refreshed, apply 1 Movement Charge to this party member.\nApply 4 Movement Charge to this party member.";
                                    x.visuals = GetAnyAbility("Struggle_A").visuals;
                                    x.animationTarget = TargettingLibrary.ThisSlot;
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RestoreSwapUseEffect>(),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                                            effect = CreateScriptable<ApplyMovementChargeEffect>(),
                                            entryVariable = 1,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<ApplyMovementChargeEffect>(),
                                            entryVariable = 4,
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc,
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_BirdFly");
                                })
                            }
                        }
                    }
                };
            });
            var crab = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Crab_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Crab");
                x.characterBackSprite = LoadSprite("CrabBack");
                x.characterOWSprite = LoadSprite("CrabOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Crab";
                x.characterEntityID = ExtendEnum<EntityIDs>("Crab");
                x.healthColor = Pigments.Red;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 12,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Red, Pigments.Red, Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Snip Snap";
                                    x._description = "Deal 5 damage to the left and right enemies.\nMove the left enemy as much to the right as possible.\nMove the right enemy as much to the left as possible.";
                                    x.visuals = GetAnyAbility("RendTheRight_A").visuals;
                                    x.animationTarget = TargettingLibrary.Relative(false, -1, 1);
                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 5,
                                            targets = TargettingLibrary.Relative(false, -1, 1)
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<MoveAwayOrCloserXTimesEffect>(x => x.away = false),
                                            entryVariable = 4,
                                            targets = TargettingLibrary.Relative(false, -1, 1)
                                        }
                                    };
                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_3_6,
                                                IntentType.Swap_Right,
                                                IntentType.Swap_Right,
                                                IntentType.Swap_Right,
                                                IntentType.Swap_Right,
                                            },
                                            targets = TargettingLibrary.Relative(false, -1)
                                        },
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_3_6,
                                                IntentType.Swap_Left,
                                                IntentType.Swap_Left,
                                                IntentType.Swap_Left,
                                                IntentType.Swap_Left,
                                            },
                                            targets = TargettingLibrary.Relative(false, 1)
                                        },
                                    };
                                    x.abilitySprite = LoadSprite("AttackIcon_CrabSnipSnap");
                                })
                            }
                        }
                    }
                };
            });
            var worm = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "BOSpecialItems_Worm_CH";

                x.damageSound = "";
                x.deathSound = "";
                x.dxSound = "";

                x.characterSprite = LoadSprite("Worm");
                x.characterBackSprite = LoadSprite("WormBack");
                x.characterOWSprite = LoadSprite("WormOW", 32, new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[0];

                x._characterName = "Worm";
                x.characterEntityID = ExtendEnum<EntityIDs>("Worm");
                x.healthColor = Pigments.Grey;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 4,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Bash";
                                    x._description = "Deals 5 damage to the Opposing enemy.\nMoves the Opposing enemy to the Left or Right.";
                                    x.visuals = GetAnyAbility("Bash_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;
                                    x.effects = GetAnyAbility("Bash_A").effects.Copy();
                                    x.intents = GetAnyAbility("Bash_A").intents.Copy();
                                    x.abilitySprite = LoadSprite("AttackIcon_Bash");
                                })
                            },
                            new()
                            {
                                cost = Cost(),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "...";
                                    x._description = "Does nothing.";
                                    x.visuals = null;
                                    x.animationTarget = null;
                                    x.effects = [];
                                    x.intents = [];
                                    x.abilitySprite = LoadSprite("AttackIcon_Worm");
                                })
                            }
                        }
                    }
                };
            });

            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Baba_CH", baba);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Keke_CH", keke);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Me_CH", me);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Jiji_CH", jiji);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Fofo_CH", fofo);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_It_CH", it);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Bird_CH", bird);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Crab_CH", crab);
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Worm_CH", worm);

            return [baba.name, keke.name, me.name, jiji.name, fofo.name, it.name, bird.name, crab.name, worm.name];
        }
    }
}
