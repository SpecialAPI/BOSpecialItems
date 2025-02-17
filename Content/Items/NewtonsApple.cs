using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class NewtonsApple
    {
        public static void Init()
        {
            Newton();
        }

        public static void Newton()
        {
            CreateScriptable<CharacterSO>(x =>
            {
                var scale = "_50";

                x._characterName = "Isaac Newton";
                x.name = "IsaacNewton_CH";

                x.characterSprite = LoadSprite($"isaacnewton{scale}");
                x.characterBackSprite = LoadSprite($"isaacnewton_back{scale}");
                x.characterOWSprite = LoadSprite($"isaacnewton_ow", pivot: new(0.5f, 0f));

                x.passiveAbilities = new BasePassiveAbilitySO[]
                {
                    CreateScriptable<PerformEffectPassiveAbility>(x =>
                    {
                        x._passiveName = "Isaac Newton";
                        x.SetDescriptions("This {0} invented gravity");
                        x.type = ExtendEnum<PassiveAbilityTypes>("IsaacNewton");

                        x.passiveIcon = LoadSprite($"isaacnewton_passive{scale}");

                        x._triggerOn = [];
                        x.effects = [];
                    })
                };

                x.movesOnOverworld = false;

                x.healthColor = Pigments.SplitPigment(Pigments.Red, Pigments.Green);

                x.damageSound = LoadedAssetsHandler.GetCharcater("Kleiver_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.GetCharcater("Smokestacks_CH").deathSound;
                x.dxSound = LoadedAssetsHandler.GetCharcater("Smokestacks_CH").dxSound;

                x.CharacterLevelSetup(i => new()
                {
                    health = Choose(i, 16, 18, 20, 22),
                    rankAbilities = new CharacterAbility[]
                    {
                        new()
                        {
                            cost = Cost(Pigments.Red, Pigments.Grey),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var damage = Choose(i, 6, 8, 10, 12);

                                var pulltext = Choose(i, "gently nudge", "nudge", "pull", "punt");
                                var pullmult = Choose(i, 1f, 1.25f, 1.5f, 1.75f);

                                x._abilityName = $"{Choose(i, "Discover", "Invent", "Create", "Become")} Gravity";
                                x._description = $"Move all enemies closer to this party member.\nDeal {damage} damage to the opposing party member, give them gravity and {pulltext} it towards this party member.";

                                x.abilitySprite = LoadSprite("AttackIcon_Gravity");

                                x.visuals = GetAnyAbility("Struggle_A").visuals;
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
                                        entryVariable = damage,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AddGravityEffect>(x =>
                                        {
                                            x.collider = AddGravityUIAction.ColliderType.None;
                                            x.launchForce = new Vector3(0f, 50f, -100f) * pullmult;
                                        }),
                                        entryVariable = 0,
                                        targets = TargettingLibrary.OpposingSlot
                                    }
                                };

                                x.intents = new IntentTargetInfo[]
                                {
                                    new()
                                    {
                                        targetIntents = new IntentType[]
                                        {
                                            IntentForDamage(damage)
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
                            })
                        },
                        new()
                        {
                            cost = Cost(Pigments.SplitPigment(Pigments.Red, Pigments.Green)),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var ruptured = Choose(i, 1, 2, 2, 3);
                                var extension = Choose(i, 1, 1, 2, 2);

                                var pulltext = Choose(i, "gently nudge", "nudge", "pull", "punt");
                                var pullmult = Choose(i, 1f, 1.33f, 1.66f, 2f);

                                var chance = Choose(i, 2, 5, 6, 7);
                                var appleitem = Choose(i, "a Tainted Apple", "a Tainted Apple", "a Tainted Apple", "The Apple");
                                var appleid = Choose(i, "TaintedApple_TW", "TaintedApple_TW", "TaintedApple_TW", "TheApple_TW");

                                x._abilityName = $"{Choose(i, "Rotten", "Fallen", "Falling", "The")} Apple";
                                x._description = $"Apply {ruptured} Ruptured to the opposing enemy.\nIncrease all negative status and field effects on the enemy side by {extension}.\nGive the opposing enemy gravity, switch its collider to sphere and {pulltext} it in a random direction.\n{chance}% chance to produce {appleitem}.";

                                x.abilitySprite = LoadSprite("AttackIcon_Apple");

                                x.visuals = GetAnyAbility("WasteAway_1_A").visuals;
                                x.animationTarget = TargettingLibrary.OpposingSlot;

                                x.effects = new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<ApplyRupturedEffect>(),
                                        entryVariable = ruptured,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<IncreaseStatusEffectsEffect>(),
                                        entryVariable = extension,
                                        targets = TargettingLibrary.OpposingSide
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AddGravityEffect>(x =>
                                        {
                                            x.collider = AddGravityUIAction.ColliderType.Sphere;
                                            x.launchForce = Vector3.zero;
                                            x.randomForce = 50f * pullmult;
                                        }),
                                        entryVariable = 0,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = Conditions.Chance(chance),
                                        effect = CreateScriptable<ExtraLootListEffect>(x =>
                                        {
                                            x._nothingPercentage = 0;
                                            x._shopPercentage = 0;
                                            x._treasurePercentage = 0;
                                            x._lockedLootableItems = new LootItemProbability[0];
                                            x._lootableItems = new LootItemProbability[] { new() { itemName = appleid, probability = 100 } };
                                        }),
                                        entryVariable = 1,
                                        targets = null
                                    }
                                };

                                x.intents = new IntentTargetInfo[]
                                {
                                    new()
                                    {
                                        targetIntents = new IntentType[]
                                        {
                                            IntentType.Status_Ruptured
                                        },
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        targetIntents = new IntentType[]
                                        {
                                            IntentType.Misc
                                        },
                                        targets = TargettingLibrary.AllEnemiesVisual
                                    },
                                    new()
                                    {
                                        targetIntents = new IntentType[]
                                        {
                                            IntentType.Misc
                                        },
                                        targets = TargettingLibrary.ThisSlot
                                    },
                                };
                            })
                        },
                        new()
                        {
                            cost = Cost(Pigments.Grey, Pigments.Red),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var damage = Choose(i, 5, 7, 9, 13);

                                var pulltext = Choose(i, "gently nudge", "nudge", "pull", "punt");
                                var pullmult = Choose(i, 1f, 1.25f, 1.5f, 1.75f);

                                var spacesmoved = Choose(i, 1, 1, 2, 2);
                                var moveaddition = "";
                                if(spacesmoved > 1)
                                {
                                    moveaddition = $" {spacesmoved} spaces";
                                }

                                x._abilityName = $"{Choose(i, "Add", "Apply", "Transfer", "Kick into")} Force";
                                x._description = $"Deal {damage} damage to the opposing enemy.\nAdd gravity to the opposing enemy and {pulltext} it away from this party member. Move the opposing enemy{moveaddition} to the left or right.";

                                x.abilitySprite = LoadSprite("AttackIcon_Force");

                                x.visuals = GetAnyAbility("Contusion_1_A").visuals;
                                x.animationTarget = TargettingLibrary.OpposingSlot;

                                x.effects = new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<DamageEffect>(),
                                        entryVariable = damage,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AddGravityEffect>(x =>
                                        {
                                            x.collider = AddGravityUIAction.ColliderType.None;
                                            x.launchForce = new Vector3(0f, 100f, 250f) * pullmult;
                                        }),
                                        entryVariable = 0,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<SwapToSidesXTimesEffect>(),
                                        entryVariable = spacesmoved,
                                        targets = TargettingLibrary.OpposingSlot
                                    }
                                };

                                x.intents = new IntentTargetInfo[]
                                {
                                    new()
                                    {
                                        targetIntents = new IntentType[]
                                        {
                                            IntentForDamage(damage),
                                            IntentType.Swap_Sides
                                        },
                                        targets = TargettingLibrary.OpposingSlot
                                    }
                                };
                            })
                        }
                    }
                });

                x.usesBasicAbility = true;
                x.basicCharAbility = new() { ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A"), cost = Cost(Pigments.Yellow) };
                x.usesAllAbilities = false;

                x.AddCharacter();
            });
        }
    }
}
