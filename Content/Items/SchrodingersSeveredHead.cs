using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering.VirtualTexturing;

namespace BOSpecialItems.Content.Items
{
    public static class SchrodingersSeveredHead
    {
        public static void Init()
        {
            var item = NewItem<ModifyAbilityUsedWithEffectsWearable>("Schrodinger's Severed Head", "", "Replaces all abilities performed by this party member with \"Obsession\".\nMay do something else at the end of combat?", "SchrodingersSeveredHead", ItemPools.Extra, 0, false);

            item.triggerOn = TriggerCalls.OnCombatEnd;
            item.effects = new EffectInfo[]
            {

            };

            TheUltrakillers();

            item.newAbility = GetAnyAbility("Obsession_A");
        }

        public static string[] TheUltrakillers()
        {
            var v1 = CreateScriptable<CharacterSOAdvanced>(x =>
            {
                x.passiveAbilities = new BasePassiveAbilitySO[]
                {
                    CreateScriptable<PerformEffectPassiveAbility>(x =>
                    {
                        x._passiveName = "<color=#ff0000>ULTRAKILL</color>";
                        x._characterDescription = x._enemyDescription = "<color=#ff0000>MANKIND IS DEAD</color>\n<color=#ff0000>BLOOD IS FUEL</color>\n<color=#ff0000>HELL IS FULL</color>";
                        x.type = ExtendEnum<PassiveAbilityTypes>("ULTRAKILL");

                        x.passiveIcon = LoadSprite("ULTRAKILL");

                        x._triggerOn = [];
                        x.effects = [];
                    }),
                    Passives.Enfeebled,
                    Passives.Leaky(3),
                    CreateScriptable<IntegerSetterPassiveAbility>(x =>
                    {
                        x._passiveName = "Inorganic";
                        x._characterDescription = "This character cannot receive direct healing.";
                        x._enemyDescription = "This enemy cannot receive direct healing.";
                        x.type = ExtendEnum<PassiveAbilityTypes>("Inorganic");

                        x.passiveIcon = LoadSprite("Inorganic");

                        x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectHealed };
                        x.integerValue = 0;

                        x.doesPassiveTriggerInformationPanel = true;
                    }),
                    CreateScriptable<AbsorbantPlatingPassiveAbility>(x =>
                    {
                        x._passiveName = "Absorbent Plating";
                        x.SetDescriptions("When the opposing {1} produces pigment of this {0}'s health color, consume that pigment to indirectly heal this {0} health equal to the amount of pigment consumed.\nDoesn't trigger if this {0}'s health is full.");
                        x.type = ExtendEnum<PassiveAbilityTypes>("AbsorbantPlating");

                        x.passiveIcon = LoadSprite("AbsorbantPlating");

                        x._triggerOn = [];
                        x.percentage = 100;

                        x.doesPassiveTriggerInformationPanel = false;
                    })
                };

                x._characterName = "V1";
                x.name = "V1_CH";

                x.damageSound = LoadedAssetsHandler.LoadCharacter("Kleiver_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.LoadEnemy("Xiphactinus_EN").deathSound;
                x.dxSound = "";

                x.healthColor = Pigments.Red;

                x.characterSprite = LoadSprite("V1");
                x.characterBackSprite = LoadSprite("V1_back");
                x.characterOWSprite = LoadSprite("V1_OW", pivot: new(0.5f, 0f));

                x.spriteScale = Vector3.one * 2f;
                x.spriteOffset = Vector3.up * 107.5f;

                x.menuSprite = LoadSprite("V1_menu");

                var revolver = new CharacterAbility()
                {
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Revolver";
                        x._description = "Consume all pigment from the yellow pigment generator.\nDeal 6 damage to the opposing enemy plus 3 for each pigment consumed.\nPigment can be dragged out of the yellow pigment generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_Revolver");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.OpposingSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<CustomPreviousExitValueDamageEffect>(x => x.previousExitValueMultiplier = 3),
                                entryVariable = 6,
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Mana_Consume
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
                            }
                        };
                    }),
                    cost = Cost(Pigments.Red)
                };

                var shotgun = new CharacterAbility()
                {
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Shotgun";
                        x._description = "Consume all pigment from the yellow pigment generator.\nIf one or less pigment was consumed, deal 20 damage randomy distributed among the left, opposing and right spaces.\nOtherwise, deal 5 damage to the opposing enemy for each pigment consumed. Damage dealt by the charged effect indirectly spreads.\nPigment can be dragged out of the yellow pigment generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_Shotgun");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.LessThanOrEqual),
                                entryVariable = 1,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(wasSuccessful: false),
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.OpposingSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(),
                                effect = CreateScriptable<DamageOnDoubleCascadeEffect>(x =>
                                {
                                    x._indirect = false;
                                    x._cascadeDecrease = 50;
                                    x._cascadeIsIndirect = true;
                                    x._consistentCascade = true;
                                    x._decreaseAsPercentage = true;
                                    x._usePreviousExitValue = true;
                                }),
                                entryVariable = 5,
                                targets = TargettingLibrary.Relative(false, 0, -1, 1, -2, 2, -3, 3, -4, 4, -5, 5)
                            },
                            new()
                            {
                                condition = Conditions.Previous(2, false),
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.Relative(false, -1, 0, 1),
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(),
                                effect = CreateScriptable<RandomDamageDistributionEffect>(),
                                entryVariable = 20,
                                targets = TargettingLibrary.Relative(false, -1, 0, 1)
                            },
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Mana_Consume
                                },
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_16_20
                                },
                                targets = TargettingLibrary.Relative(false, -1, 0, 1)
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_21
                                },
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };
                    }),
                    cost = Cost(Pigments.Red, Pigments.Red, Pigments.Red)
                };

                var nailgun = new CharacterAbility()
                {
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Nailgun";
                        x._description = "Consume all pigment from the yellow pigment generator.\nDeal 2 damage to the opposing enemy 3 times.\nIf any pigment was consumed, deal 2 damage an additional time.\nApply 1 fire to the opposing position for each pigment over one consumed.\nPigment can be dragged out of the yellow pigment generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_Nailgun");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.OpposingSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 1,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 2,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 3,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<ApplyFireSlotEffect>(),
                                entryVariable = 1,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<ApplyFireSlotEffect>(),
                                entryVariable = 1,
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Mana_Consume
                                },
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Field_Fire,
                                    IntentType.Field_Fire,
                                },
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };
                    }),
                    cost = Cost(Pigments.Red, Pigments.Red)
                };

                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 30,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost(Pigments.Blue),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Feedbacker";
                                    x._description = "Deal 6 damage to the opposing enemy.\nIf this would activate any on-hit effects, don't activate them and instead deal 16 damage.";

                                    x.abilitySprite = LoadSprite("AttackIcon_Feedbacker");

                                    x.visuals = GetAnyAbility("Parry_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<FeedbackerDamageEffect>(x => x.damageWithNotifications = 16),
                                            entryVariable = 6,
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };

                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Damage_3_6,
                                                IntentType.Damage_16_20
                                            },
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                })
                            },
                            revolver,
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Equipment Change";
                                    x._description = "Alternate between Revolver, Shotgun and Nailgun.\nRefresh this party member.";

                                    x.abilitySprite = LoadSprite("AttackIcon_EquipmentChange");

                                    x.visuals = null;
                                    x.animationTarget = null;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<AlternateBetweenCasterAbilitiesEffect>(x => x.Abilities = new()
                                            {
                                                new(revolver),
                                                new(shotgun),
                                                new(nailgun)
                                            }),
                                            entryVariable = 0,
                                            targets = null
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RefreshAbilityUseEffect>(),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };

                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                })
                            }
                        }
                    }
                };

                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
            });
            var v2 = CreateScriptable<CharacterSOAdvanced>(x =>
            {
                x.passiveAbilities = new BasePassiveAbilitySO[]
                {
                    Passives.Leaky(3),
                    CreateScriptable<IntegerSetterPassiveAbility>(x =>
                    {
                        x._passiveName = "Inorganic";
                        x._characterDescription = "This character cannot receive direct healing.";
                        x._enemyDescription = "This enemy cannot receive direct healing.";
                        x.type = ExtendEnum<PassiveAbilityTypes>("Inorganic");

                        x.passiveIcon = LoadSprite("Inorganic");

                        x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectHealed };
                        x.integerValue = 0;

                        x.doesPassiveTriggerInformationPanel = true;
                    }),
                    CreateScriptable<PerformEffectPassiveAbility>(x =>
                    {
                        x._passiveName = "Self-Repair";
                        x.SetDescriptions("Indirectly heal 4 health at the end of combat.");
                        x.type = ExtendEnum<PassiveAbilityTypes>("AbsorbantPlating");

                        x.passiveIcon = LoadSprite("SelfRepair");

                        x._triggerOn = new TriggerCalls[] { TriggerCalls.OnCombatEnd };
                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<HealEffect>(),
                                entryVariable = 4,
                                targets = TargettingLibrary.ThisSlot
                            }
                        };

                        x.doesPassiveTriggerInformationPanel = true;
                    }),
                    CreateScriptable<MultiSpecialStoredValuePerformEffectPassive>(x =>
                    {
                        x._passiveName = "Arms";
                        x.SetDescriptions("At the start of each turn, reduce the cooldown of Knuckleblaster and Whiplash by 1.");
                        x.type = ExtendEnum<PassiveAbilityTypes>("V2Arms");

                        x.passiveIcon = LoadSprite("V2Arms");

                        x._triggerOn = new TriggerCalls[] { TriggerCalls.OnTurnStart };
                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<CasterStoredValueChangeEffect>(x => { x._increase = false; x._minimumValue = 0; x._valueName = StoredValue("KnuckleblasterRecharge"); }),
                                entryVariable = 1,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<CasterStoredValueChangeEffect>(x => { x._increase = false; x._minimumValue = 0; x._valueName = StoredValue("WhiplashRecharge"); }),
                                entryVariable = 1,
                                targets = null
                            },
                        };

                        x.extraStoredValues = new UnitStoredValueNames[]
                        {
                            StoredValue("KnuckleblasterRecharge"),
                            StoredValue("WhiplashRecharge")
                        };

                        AddStoredValue("KnuckleblasterRecharge", new()
                        {
                            staticString = "Knuckleblaster cooldown: {0}",
                            colorType = StoredValueInfo.ColorType.Custom,
                            condition = StoredValueInfo.StoredValueCondition.Positive,
                            customColor = new Color32(82, 16, 16, 255)
                        });
                        AddStoredValue("WhiplashRecharge", new()
                        {
                            staticString = "Whiplash cooldown: {0}",
                            colorType = StoredValueInfo.ColorType.Custom,
                            condition = StoredValueInfo.StoredValueCondition.Positive,
                            customColor = new Color32(48, 44, 22, 255)
                        });
                    })
                };

                x._characterName = "V2";
                x.name = "V2_CH";

                x.damageSound = LoadedAssetsHandler.LoadCharacter("Kleiver_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.LoadEnemy("Xiphactinus_EN").deathSound;
                x.dxSound = "";

                x.healthColor = Pigments.Red;

                x.characterSprite = LoadSprite("V2");
                x.characterBackSprite = LoadSprite("V2_back");
                x.characterOWSprite = LoadSprite("V2_OW", pivot: new(0.5f, 0f));

                x.spriteScale = Vector3.one * 2f;
                x.spriteOffset = Vector3.up * 107.5f;

                x.menuSprite = LoadSprite("V2_menu");

                var knuckleblaster = new CharacterAbility()
                {
                    cost = Cost(Pigments.Red, Pigments.Red, Pigments.Red),
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Knuckleblaster";
                        x._description = "Deal 10 damage to the opposing enemy.\nInflict 2 Frail to the opposing enemy.\nDisable Knuckleblaster usage for 2 turns.";

                        x.abilitySprite = LoadSprite("AttackIcon_Knuckleblaster");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<CasterStoreValueCheckEffect>(x => x._valueName = StoredValue("KnuckleblasterRecharge")),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(1, false),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.OpposingSlot; x._visuals = GetAnyAbility("Contusion_1_A").visuals; }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(2, false),
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 10,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(3, false),
                                effect = CreateScriptable<ApplyFrailEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(4, false),
                                effect = CreateScriptable<CasterStoreValueSetterEffect>(x => x._valueName = StoredValue("KnuckleblasterRecharge")),
                                entryVariable = 2,
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
                                    IntentType.Status_Frail
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
                            }
                        };

                        x.specialStoredValue = StoredValue("KnuckleblasterRecharge");
                    })
                };
                var whiplash = new CharacterAbility()
                {
                    cost = Cost(Pigments.Red),
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Whiplash";
                        x._description = "Picks the farthest enemy from this party member. If that enemy is size 1, try to move that enemy in front of this party member. If that enemy is size 2 or greater, try to move this party member in front of that enemy.\nApply 1 Scar to the opposing enemy.\nReduce this party member's maximum health by 2.\nRefresh this party member and disable Whiplash usage for this turn.";

                        x.abilitySprite = LoadSprite("AttackIcon_Whiplash");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<CasterStoreValueCheckEffect>(x => x._valueName = StoredValue("WhiplashRecharge")),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Chance(50),
                                effect = CreateScriptable<ExtraVariableForNextEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.MultiPrevious((2, false), (1, false)),
                                effect = CreateScriptable<WhiplashVisualsEffect>(x => { x.visuals = GetAnyAbility("Struggle_A").visuals; x.prioritizeLeft = true; }),
                                entryVariable = 0,
                                targets = TargettingLibrary.AllEnemies
                            },
                            new()
                            {
                                condition = Conditions.MultiPrevious((3, false), (2, true)),
                                effect = CreateScriptable<WhiplashVisualsEffect>(x => { x.visuals = GetAnyAbility("Struggle_A").visuals; x.prioritizeLeft = false; }),
                                entryVariable = 0,
                                targets = TargettingLibrary.AllEnemies
                            },
                            new()
                            {
                                condition = Conditions.MultiPrevious((4, false), (3, false)),
                                effect = CreateScriptable<WhiplashMovementEffect>(x => x.prioritizeLeft = true),
                                entryVariable = 0,
                                targets = TargettingLibrary.AllEnemies
                            },
                            new()
                            {
                                condition = Conditions.MultiPrevious((5, false), (4, true)),
                                effect = CreateScriptable<WhiplashMovementEffect>(x => x.prioritizeLeft = false),
                                entryVariable = 0,
                                targets = TargettingLibrary.AllEnemies
                            },
                            new()
                            {
                                condition = Conditions.Previous(6, false),
                                effect = CreateScriptable<ApplyScarsEffect>(),
                                entryVariable = 1,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(7, false),
                                effect = CreateScriptable<ChangeMaxHealthEffect>(x => x._increase = false),
                                entryVariable = 2,
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(8, false),
                                effect = CreateScriptable<CasterStoreValueSetterEffect>(x => x._valueName = StoredValue("WhiplashRecharge")),
                                entryVariable = 1,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(9, false),
                                effect = CreateScriptable<RefreshAbilityUseEffect>(),
                                entryVariable = 0,
                                targets = TargettingLibrary.ThisSlot
                            }
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Swap_Mass
                                },
                                targets = TargettingLibrary.AllEnemiesVisual
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Swap_Mass,
                                    IntentType.Misc
                                },
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Status_Scars
                                },
                                targets = TargettingLibrary.OpposingSlot
                            },
                        };

                        x.specialStoredValue = StoredValue("WhiplashRecharge");
                    })
                };

                var marksman = new CharacterAbility()
                {
                    cost = Cost(Pigments.Red),
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Slab Marksman";
                        x._description = "Consumes all pigment from the yellow pigment generator.\nDeals 4 damage to the opposing enemy. Chains to one enemy for each pigment consumed, starting at 8 dry damage and increasing by 3 every hit. Prioritizes chaining to enemies with Frail. Sub-prioritizes chaining to closer enemies. If two enemies have the same chaining priority, will pick the next enemy to chain randomly. Can't chain to already hit enemies.\nPigment can be dragged out of the generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_Marksman");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.OpposingSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<MarksmanDamageEffect>(x => { x.chainStartDamage = 8; x.chainDamageIncrease = 3; }),
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
                                    IntentType.Mana_Consume
                                },
                                targets = TargettingLibrary.ThisSlot
                            },
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
                                    IntentType.Damage_21
                                },
                                targets = CreateScriptable<Targetting_ByUnit_Side>(x => { x.getAllies = false; x.getAllUnitSlots = true; x.ignoreCastSlot = true; })
                            }
                        };
                    })
                };
                var pumpcharge = new CharacterAbility()
                {
                    cost = Cost(Pigments.Red, Pigments.Red),
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Pump-Charge";
                        x._description = "Consume all pigment from the yellow pigment generator.\nDeal 20 damage plus 10 for each pigment consumed randomly distributed among the left, opposing and right slots.\n10% chance for each pigment consumed to explode instead, dealing 8 damage to this party member and 18 to the opposing enemy.\nPigment can be dragged out of the yellow pigment generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_PumpCharge");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<PreviousExitValueChanceEffect>(x => x.chancePerPreviousExitValue = 10),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(wasSuccessful: false),
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.Relative(false, -1, 0, 1),
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(2, false),
                                effect = CreateScriptable<RandomDamageDistributionEffect>(x => x.previousExitValueContribution = 10),
                                entryVariable = 20,
                                targets = TargettingLibrary.Relative(false, -1, 0, 1)
                            },
                            new()
                            {
                                condition = Conditions.Previous(3),
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.ThisSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = Conditions.Previous(4),
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 8,
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(5),
                                effect = CreateScriptable<DamageEffect>(),
                                entryVariable = 18,
                                targets = TargettingLibrary.OpposingSlot
                            },
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_21
                                },
                                targets = TargettingLibrary.Relative(false, -1, 0, 1)
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_16_20
                                },
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Mana_Consume,
                                    IntentType.Damage_3_6
                                },
                                targets = TargettingLibrary.ThisSlot
                            }
                        };
                    })
                };
                var sandynailgun = new CharacterAbility()
                {
                    cost = Cost(Pigments.Red, Pigments.Red, Pigments.Red),
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x._abilityName = "Sandy Nailgun";
                        x._description = "Consume all pigment from the yellow pigment generator.\nDeal 2 dry damage to the opposing enemy 3 times.\nIf any pigment was consumed, deal 2 dry damage an additional time.\nApply 1 fire to the opposing position for each pigment over one consumed.\nPigment can be dragged out of the yellow pigment generator to not be consumed by this ability.";

                        x.abilitySprite = LoadSprite("AttackIcon_SandyNailgun");

                        x.visuals = null;
                        x.animationTarget = null;

                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ConsumeAllPigmentFromGeneratorEffect>(),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                {
                                    new()
                                    {
                                        playAudio = true,
                                        targets = TargettingLibrary.OpposingSlot,
                                        visuals = GetAnyAbility("Crush_A").visuals,
                                        targettingOffset = 0,
                                        timeDelay = 0f
                                    }
                                }),
                                entryVariable = 0,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 1,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 2,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<ComparePreviousExitValueEffect>(x => x.comparison = ComparePreviousExitValueEffect.ComparisonType.GreaterThanOrEqual),
                                entryVariable = 3,
                                targets = null
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DryDamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DryDamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<DryDamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<DryDamageEffect>(),
                                entryVariable = 2,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<ApplyFireSlotEffect>(),
                                entryVariable = 1,
                                targets = TargettingLibrary.OpposingSlot
                            },
                            new()
                            {
                                condition = Conditions.Previous(6),
                                effect = CreateScriptable<ApplyFireSlotEffect>(),
                                entryVariable = 1,
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };

                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Mana_Consume
                                },
                                targets = TargettingLibrary.ThisSlot
                            },
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Damage_1_2,
                                    IntentType.Field_Fire,
                                    IntentType.Field_Fire,
                                },
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };
                    })
                };

                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 30,
                        rankAbilities = new CharacterAbility[]
                        {
                            whiplash,
                            marksman,
                            new()
                            {
                                cost = Cost(Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Equipment Change";
                                    x._description = "Alternate between Slab Marksman, Pump-Charge and Sandy Nailgun.\nAlternate between Knuckleblaster and Whiplash.\nRefresh this party member.";

                                    x.abilitySprite = LoadSprite("AttackIcon_EquipmentChange");

                                    x.visuals = null;
                                    x.animationTarget = null;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<AlternateBetweenCasterAbilitiesEffect>(x => x.Abilities = new()
                                            {
                                                new(marksman),
                                                new(pumpcharge),
                                                new(sandynailgun)
                                            }),
                                            entryVariable = 0,
                                            targets = null
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<AlternateBetweenCasterAbilitiesEffect>(x => x.Abilities = new()
                                            {
                                                new(knuckleblaster),
                                                new(whiplash)
                                            }),
                                            entryVariable = 0,
                                            targets = null
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RefreshAbilityUseEffect>(),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };

                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.ThisSlot
                                        }
                                    };
                                })
                            }
                        }
                    }
                };

                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
            });
            var gabriel = CreateScriptable<CharacterSO>(x =>
            {
                x.passiveAbilities = [];

                x._characterName = "Gabriel";
                x.name = "Gabriel_CH";

                x.damageSound = LoadedAssetsHandler.GetCharcater("Griffin_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.GetCharcater("Griffin_CH").deathSound;
                x.dxSound = LoadedAssetsHandler.GetCharcater("Griffin_CH").dxSound;

                x.healthColor = Pigments.Yellow;

                x.characterSprite = LoadSprite("Gabriel");
                x.characterBackSprite = LoadSprite("Gabriel_back");
                x.characterOWSprite = LoadSprite("Gabriel_OW");

                var splendor = new CharacterAbility()
                {
                    cost = Cost(Pigments.Yellow, Pigments.Blue)
                };
                var justice = new CharacterAbility()
                {
                    cost = Cost(Pigments.Blue, Pigments.Yellow)
                };

                splendor.ability = CreateScriptable<AbilitySO>(x =>
                {
                    x._abilityName = "Splendor";
                    x._description = "Deal 7 damage to the opposing enemy and it to the left.\nChange into Justice.\n40% chance to refresh this party member.";

                    x.abilitySprite = LoadSprite("AttackIcon_Splendor");

                    x.visuals = GetAnyAbility("Domination_A").visuals;
                    x.animationTarget = TargettingLibrary.OpposingSlot;

                    x.effects = new EffectInfo[]
                    {
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<DamageEffect>(),
                            entryVariable = 7,
                            targets = TargettingLibrary.OpposingSlot
                        },
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false),
                            entryVariable = 0,
                            targets = TargettingLibrary.OpposingSlot
                        },
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<AlternateBetweenCasterAbilitiesEffect>(x => x.Abilities = new() { new(splendor), new(justice) })
                        },
                        new()
                        {
                            condition = Conditions.Chance(40),
                            effect = CreateScriptable<RefreshAbilityUseEffect>(),
                            entryVariable = 0,
                            targets = TargettingLibrary.ThisSlot
                        }
                    };

                    x.intents = new IntentTargetInfo[]
                    {
                        new()
                        {
                            targetIntents = new IntentType[]
                            {
                                IntentType.Damage_7_10,
                                IntentType.Swap_Left
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
                        }
                    };
                });
                justice.ability = CreateScriptable<AbilitySO>(x =>
                {
                    x._abilityName = "Justice";
                    x._description = "Deal 7 damage to the opposing enemy and it to the right.\nChange into Splendor.\n40% chance to refresh this party member.";

                    x.abilitySprite = LoadSprite("AttackIcon_Justice");

                    x.visuals = GetAnyAbility("Domination_A").visuals;
                    x.animationTarget = TargettingLibrary.OpposingSlot;

                    x.effects = new EffectInfo[]
                    {
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<DamageEffect>(),
                            entryVariable = 7,
                            targets = TargettingLibrary.OpposingSlot
                        },
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = true),
                            entryVariable = 0,
                            targets = TargettingLibrary.OpposingSlot
                        },
                        new()
                        {
                            condition = null,
                            effect = CreateScriptable<AlternateBetweenCasterAbilitiesEffect>(x => x.Abilities = new() { new(splendor), new(justice) })
                        },
                        new()
                        {
                            condition = Conditions.Chance(40),
                            effect = CreateScriptable<RefreshAbilityUseEffect>(),
                            entryVariable = 0,
                            targets = TargettingLibrary.ThisSlot
                        }
                    };

                    x.intents = new IntentTargetInfo[]
                    {
                        new()
                        {
                            targetIntents = new IntentType[]
                            {
                                IntentType.Damage_7_10,
                                IntentType.Swap_Right
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
                        }
                    };
                });

                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 20,
                        rankAbilities = new CharacterAbility[]
                        {
                            splendor,
                            new()
                            {
                                cost = Cost(Pigments.Blue, Pigments.Blue, Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Divine Spear";
                                    x._description = "Deal 16 damage to the opposing enemy. If the opposing enemy is Ruptured, remove it and deal 20 damage instead.";

                                    x.abilitySprite = LoadSprite("AttackIcon_DivineSpear");

                                    x.visuals = GetAnyAbility("Excommunicate_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<RemoveStatusEffectEffect>(x => x._statusToRemove = StatusEffectType.Ruptured),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.OpposingSlot
                                        },
                                        new()
                                        {
                                            condition = Conditions.Previous(),
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 20,
                                            targets = TargettingLibrary.OpposingSlot
                                        },
                                        new()
                                        {
                                            condition = Conditions.Previous(2, false),
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 16,
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };

                                    x.intents = new IntentTargetInfo[]
                                    {
                                        new()
                                        {
                                            targetIntents = new IntentType[]
                                            {
                                                IntentType.Rem_Status_Ruptured,
                                                IntentType.Damage_16_20,
                                                IntentType.Damage_16_20
                                            }
                                        }
                                    };
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Red, Pigments.Blue, Pigments.Yellow),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Sword Throw";
                                    x._description = "Deal 8 damage to the far left and far right enemies. Inflict 3 Ruptured to the far left and far right enemies.";

                                    x.abilitySprite = LoadSprite("AttackIcon_SwordThrow");

                                    x.visuals = GetAnyAbility("FlayTheFlesh_A").visuals;
                                    x.animationTarget = TargettingLibrary.Relative(false, -2, 2);

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageEffect>(),
                                            entryVariable = 8,
                                            targets = TargettingLibrary.Relative(false, -2, 2)
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<ApplyRupturedEffect>(),
                                            entryVariable = 3,
                                            targets = TargettingLibrary.Relative(false, -2, 2)
                                        }
                                    };
                                })
                            }
                        }
                    }
                };

                x.usesAllAbilities = true;
                x.usesBasicAbility = false;
            });

            v1.AddCharacter();
            v2.AddCharacter();
            gabriel.AddCharacter();

            return [v1.name, v2.name];
        }
    }
}
