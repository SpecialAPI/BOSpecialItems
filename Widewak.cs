using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UnityEngine;

namespace BOSpecialItems
{
    public static class Widewak
    {
        public static void Init()
        {
            var widewakPassive = CreateScriptable<WidewakPassive>(x =>
            {
                var multPerSize = 0.2f;

                x._passiveName = "Wide";
                x.SetDescriptions($"This {{0}} deals {(int)(multPerSize * 100f)}% more damage for each space they occupy above 1.\nThis {{0}} takes an amount of times less direct damage equal to the amount of spaces occupied by this {{0}}.");
                x.type = ExtendEnum<PassiveAbilityTypes>("Widewak");

                x.passiveIcon = LoadSprite("PassiveWidewak");

                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnWillApplyDamage, TriggerCalls.OnBeingDamaged };
                x.damageDealtMultPerSize = multPerSize;
            });

            CharacterSOAdvanced XSizeWakSetup(int size, string namePrefix, CharacterSOAdvanced nextTransform, AbilitySO expandAbilityOverride = null)
            {
                return CreateScriptable<CharacterSOAdvanced>(x =>
                {
                    x.name = $"{namePrefix}wak_CH";
                    x._characterName = $"{namePrefix}wak";
                    x.characterEntityID = ExtendEnum<EntityIDs>($"{namePrefix}wak");
                    x.healthColor = Pigments.Purple;
                    x.usesAllAbilities = true;
                    x.usesBasicAbility = false;
                    x.basicCharAbility = new() { ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A"), cost = Cost(Pigments.Yellow) };

                    var uppercutCost = new ManaColorSO[1 + 2 * (size - 1)];
                    var childStomperCost = new ManaColorSO[2 + 2 * (size - 1)];

                    uppercutCost[0] = Pigments.Yellow;

                    childStomperCost[0] = Pigments.Yellow;
                    childStomperCost[1] = Pigments.Yellow;

                    for(int i = 0; i < size - 1; i++)
                    {
                        uppercutCost[i * 2 + 1] = Pigments.Grey.Optional();
                        uppercutCost[i * 2 + 2] = Pigments.Grey.Optional();

                        childStomperCost[i * 2 + 2] = Pigments.Grey.Optional();
                        childStomperCost[i * 2 + 3] = Pigments.Grey.Optional();
                    }

                    x.CharacterLevelSetup(l => new()
                    {
                        health = Choose(l, 12, 16, 20, 24),
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = uppercutCost.ToArray(),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    var launchmult = Choose(l, 1f, 2f, 3f, 4f);
                                    var launchtext = Choose(l, "nudge them up", "kick them up", "launch them up", "launch them into space");

                                    var damage = Choose(l, 5, 7, 9, 11);

                                    x._abilityName = $"{Choose(l, "Painful", "Distasteful", "Harrowing", "\"Going to thesaurus.com to find synonyms for painful\"")} Uppercut";
                                    x._description = $"Give the opposing enemies gravity and {launchtext}.\nDeal {damage} damage to the opposing enemies.";

                                    x.abilitySprite = LoadSprite("AttackIcon_Uppercut");

                                    x.visuals = null;
                                    x.animationTarget = null;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                            {
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.OpposingSlot,
                                                    targettingOffset = 0,
                                                    timeDelay = 0f,
                                                    visuals = GetAnyAbility("Clobber_1_A").visuals,
                                                    zRotation = 180f
                                                }
                                            })
                                        },
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<DamageWithGravityEffect>(x =>
                                            {
                                                x.randomForce = 0f;
                                                x.launchForce = Vector3.up * 500f * launchmult;
                                                x.collider = AddGravityUIAction.ColliderType.None;
                                            }),
                                            entryVariable = damage,
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
                                        }
                                    };
                                })
                            },
                            new()
                            {
                                cost = childStomperCost.ToArray(),
                                ability = CreateScriptable<AbilitySO>(x =>
                                {
                                    var damage = Choose(l, 6, 8, 10, 12);
                                    var childdamage = Choose(l, 8, 12, 18, 24);

                                    x._abilityName = $"{Choose(l, "Despicable", "Bitter", "Heinous", "I am a proud")} Child Stomper";
                                    x._description = $"Deal {damage} to the opposing enemies.\nIf an opposing enemy is an infant, instead deal {childdamage} damage and don't trigger its on-damage effects.";

                                    x.abilitySprite = LoadSprite("AttackIcon_ChildStomper");

                                    x.visuals = GetAnyAbility("Takedown_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.OpposingSlot;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<ChildStomperDamageEffect>(x => x.childDamage = childdamage),
                                            entryVariable = damage,
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
                                                IntentForDamage(childdamage),
                                                IntentType.Misc
                                            },
                                            targets = TargettingLibrary.OpposingSlot
                                        }
                                    };
                                })
                            },
                            new()
                            {
                                cost = Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple),
                                ability = expandAbilityOverride != null ? expandAbilityOverride : CreateScriptable<AbilitySO>(x =>
                                {
                                    x._abilityName = "Expand";
                                    x._description = $"Attempt to transform into {nextTransform._characterName}, expanding one slot to the right.\nIf this fails, make this party member die a miserable death";

                                    x.abilitySprite = LoadSprite("AttackIcon_Expand");

                                    x.visuals = GetAnyAbility("Amalgam_1_A").visuals;
                                    x.animationTarget = TargettingLibrary.ThisSlot;

                                    x.effects = new EffectInfo[]
                                    {
                                        new()
                                        {
                                            condition = null,
                                            effect = CreateScriptable<CasterTransformationEffect>(x =>
                                            {
                                                x._fullyHeal = false;
                                                x._maintainTimelineAbilities = true;
                                                x._maintainMaxHealth = true;
                                                x._currentToMaxHealth = false;
                                                x._enemyTransformation = null;
                                                x._characterTransformation = nextTransform;
                                            }),
                                            entryVariable = 0,
                                            targets = TargettingLibrary.ThisSlot
                                        },
                                        new()
                                        {
                                            condition = Conditions.Previous(wasSuccessful: false),
                                            effect = CreateScriptable<DirectDeathEffect>(),
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
                    });

                    x.passiveAbilities = new BasePassiveAbilitySO[] { widewakPassive };
                    x.characterAnimator = null;
                    x.characterSprite = LoadSprite($"{namePrefix}wak", 32, new(0.5f, 0f));
                    x.characterBackSprite = LoadSprite($"{namePrefix}wak_Back", 32, new(0.5f, 0f));
                    x.characterOWSprite = LoadSprite($"{namePrefix}wak_Overworld", 32, new(0.25f, 0f));
                    x.extraCombatSprites = null;
                    x.movesOnOverworld = false;
                    x.speakerDataName = LoadedAssetsHandler.GetCharcater("Nowak_CH").speakerDataName;
                    x.damageSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").damageSound;
                    x.deathSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").deathSound;
                    x.dxSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").dxSound;

                    x.spriteScale = Vector3.one * (1 + 2 * (size - 1));
                    x.spriteOffset = new Vector3(215f, 214.8f, 0f) * (size - 1);

                    x.menuSprite = x.combatMenuSprite = LoadSprite($"{namePrefix}wak_Menu");

                    x.size = size;
                });
            }

            var god = CreateScriptable<CharacterSOAdvanced>(x =>
            {
                x.name = $"God_CH";
                x._characterName = $"God";
                x.characterEntityID = ExtendEnum<EntityIDs>($"God");
                x.healthColor = Pigments.Purple;
                x.usesAllAbilities = true;
                x.usesBasicAbility = false;
                x.basicCharAbility = new() { ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A"), cost = Cost(Pigments.Yellow) };

                var costs = new ManaColorSO[10];

                for(int i = 0; i < costs.Length; i++)
                {
                    costs[i] = Pigments.Grey.Optional();
                }

                x.CharacterLevelSetup(l => new()
                {
                    health = Choose(l, 12, 16, 20, 24),
                    rankAbilities = new CharacterAbility[]
                    {
                        new()
                        {
                            cost = costs.ToArray(),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var damage = Choose(l, 6, 8, 10, 12);
                                var launchmult = Choose(l, 1f, 2f, 3f, 4f);

                                var launchtext = Choose(l, "send them up", "launch them up", "ascend them up", "ascend them to heaven");

                                x._abilityName = $"{Choose(l, "Holy ", "Sacred ", "Divine ", "")}Ascent{Choose(l, "", "", "", " to Heaven")}";
                                x._description = $"Give the opposig enemies gravity and {launchtext}.\nDeal {damage} damage to the opposing enemies.";

                                x.abilitySprite = LoadSprite("AttackIcon_Ascent");

                                x.visuals = null;
                                x.animationTarget = null;

                                x.effects = new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x => x.animations = new()
                                        {
                                            new()
                                            {
                                                playAudio = true,
                                                targets = TargettingLibrary.OpposingSlot,
                                                targettingOffset = 0,
                                                timeDelay = 0f,
                                                visuals = GetAnyAbility("Excommunicate_A").visuals,
                                                zRotation = 180f
                                            }
                                        }),
                                        entryVariable = 0,
                                        targets = null
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<DamageWithGravityEffect>(x =>
                                        {
                                            x.randomForce = 0f;
                                            x.launchForce = Vector3.up * 1000f * launchmult;
                                            x.collider = AddGravityUIAction.ColliderType.None;
                                        }),
                                        entryVariable = damage,
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
                                    }
                                };
                            })
                        },
                        new()
                        {
                            cost = costs.ToArray(),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var damage = Choose(l, 7, 9, 11, 13);
                                var sinnerdamage = Choose(l, 10, 15, 25, 35);

                                x._abilityName = $"Punish the {Choose(l, "Sinners", "Heretics", "Blasphemers", "World")}";
                                x._description = $"Deal {damage} damage to the opposing enemies.\nIf an enemy has any on-damage effects, instead deal {sinnerdamage} damage to it and don't trigger its on-damage effects.\nInflict Curse to the opposing enemies.";

                                x.abilitySprite = LoadSprite("AttackIcon_Punish");

                                x.visuals = GetAnyAbility("Excommunicate_A").visuals;
                                x.animationTarget = TargettingLibrary.OpposingSlot;

                                x.effects = new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<FeedbackerDamageEffect>(x => x.damageWithNotifications = sinnerdamage),
                                        entryVariable = damage,
                                        targets = TargettingLibrary.OpposingSlot
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<ApplyCursedEffect>(),
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
                                            IntentForDamage(damage),
                                            IntentForDamage(sinnerdamage),
                                            IntentType.Misc,
                                            IntentType.Status_Cursed
                                        }
                                    }
                                };
                            })
                        },
                        new()
                        {
                            cost = Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple),
                            ability = CreateScriptable<AbilitySO>(x =>
                            {
                                var summonlevel = Choose(l, 0, 1, 2, 3);

                                x._abilityName = "Garden of Eden";
                                x._description = $"Immediately flee combat.\nSummon Nodam and Ewak as permanent level {summonlevel + 1} party members.";

                                x.abilitySprite = LoadSprite("AttackIcon_GardenOfEden");

                                x.visuals = GetAnyAbility("Repent_A").visuals;
                                x.animationTarget = CreateScriptable<TargettingCustomAllySlot>(x =>
                                {
                                    x.targetOffsets = new() { 0 };
                                    x.frontOffsets = new() { 1, 3 };
                                });

                                x.effects = new EffectInfo[]
                                {
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
                                        effect = CreateScriptable<SpawnCharacterInSlotEffect>(x =>
                                        {
                                            x.characterName = "Nodam_CH";
                                            x.slot = 1;
                                            x.alwaysTrySpawn = true;
                                            x.permanent = true;
                                            x.rank = summonlevel;
                                            x.addToCasterRank = false;
                                        })
                                    },
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<SpawnCharacterInSlotEffect>(x =>
                                        {
                                            x.characterName = "Ewak_CH";
                                            x.slot = 3;
                                            x.alwaysTrySpawn = true;
                                            x.permanent = true;
                                            x.rank = summonlevel;
                                            x.addToCasterRank = false;
                                        })
                                    }
                                };
                            })
                        }
                    }
                });

                x.passiveAbilities = new BasePassiveAbilitySO[] { widewakPassive };
                x.characterAnimator = null;
                x.characterSprite = LoadSprite("God", 32, new(0.5f, 0f));
                x.characterBackSprite = LoadSprite("God_Back", 32, new(0.5f, 0f));
                x.characterOWSprite = LoadSprite("God_Overworld", 32, new(0.25f, 0f));
                x.extraCombatSprites = null;
                x.movesOnOverworld = false;
                x.speakerDataName = LoadedAssetsHandler.GetCharcater("Nowak_CH").speakerDataName;
                x.damageSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").deathSound;
                x.dxSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").dxSound;

                x.spriteScale = Vector3.one * 9;
                x.spriteOffset = new Vector3(215f, 214.8f, 0f) * 4f;

                x.menuSprite = x.combatMenuSprite = LoadSprite("God_Menu");

                x.size = 5;
            });

            var widestwak = XSizeWakSetup(4, "Widest", null, CreateScriptable<AbilitySO>(x =>
            {
                x._abilityName = "Ascend";
                x._description = "Ascend, expanding one slot to the right.\nIf this fails, make this party member die a miserable death.\n\"You're right ther- I mean, beep.\"";

                x.abilitySprite = LoadSprite("AttackIcon_Ascend");

                x.visuals = GetAnyAbility("ComeHome_A").effects.Select(x => x.effect).OfType<AnimationVisualsIfUnitEffect>().First()._visuals;
                x.animationTarget = null;

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<CasterTransformationEffect>(x =>
                        {
                            x._fullyHeal = false;
                            x._maintainTimelineAbilities = true;
                            x._maintainMaxHealth = true;
                            x._currentToMaxHealth = false;
                            x._enemyTransformation = null;
                            x._characterTransformation = god;
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    },
                    new()
                    {
                        condition = Conditions.Previous(wasSuccessful: false),
                        effect = CreateScriptable<DirectDeathEffect>(x => x._obliterationDeath = true),
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
            }));
            var widerwak = XSizeWakSetup(3, "Wider", widestwak);
            var widewak = XSizeWakSetup(2, "Wide", widerwak);

            god.AddCharacter();
            widestwak.AddCharacter();
            widerwak.AddCharacter();
            widewak.AddCharacter();
        }
    }
}
