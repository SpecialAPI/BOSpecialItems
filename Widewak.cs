using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class Widewak
    {
        public static void Init()
        {
            var widewak = CreateScriptable<CharacterSO>(x =>
            {
                x.name = "Widewak_CH";
                x._characterName = "Widewak";
                x.characterEntityID = ExtendEnum<EntityIDs>("Widewak");
                x.healthColor = Pigments.Purple;
                x.usesAllAbilities = true;
                x.usesBasicAbility = false;
                x.basicCharAbility = new() { ability = LoadedAssetsHandler.GetCharacterAbility("Slap_A"), cost = Cost(Pigments.Yellow) };
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 12,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                ability = GadgetDB.SetupGadget("animations test 1", "Is this a forest?", null, null, Cost(), new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x =>
                                        {
                                            x.animations = new()
                                            {
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 0),
                                                    timeDelay = 0f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 1),
                                                    timeDelay = 0.1f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 2),
                                                    timeDelay = 0.2f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 3),
                                                    timeDelay = 0.3f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 4),
                                                    timeDelay = 0.4f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                            };
                                        }),
                                        entryVariable = 0,
                                        targets = null
                                    }
                                }, new IntentTargetInfo[0]).ability,
                                cost = Cost()
                            },
                            new()
                            {
                                ability = GadgetDB.SetupGadget("animations test 2", "Google witches", null, null, Cost(), new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x =>
                                        {
                                            x.animations = new()
                                            {
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false,  1, 3),
                                                    timeDelay = 0f,
                                                    visuals = GetAnyAbility("FlayTheFlesh_A").visuals
                                                },
                                                new()
                                                {
                                                    playAudio = true,
                                                    targets = TargettingLibrary.Absolute(false, 2),
                                                    timeDelay = 0f,
                                                    visuals = GetAnyAbility("Domination_A").visuals
                                                }
                                            };
                                        }),
                                        entryVariable = 0,
                                        targets = null
                                    }
                                }, new IntentTargetInfo[0]).ability,
                                cost = Cost()
                            },
                            new()
                            {
                                ability = GadgetDB.SetupGadget("animations test 3", "Send me a fax so I can't escape", null, null, Cost(), new EffectInfo[]
                                {
                                    new()
                                    {
                                        condition = null,
                                        effect = CreateScriptable<AdvancedAnimationVisualsEffect>(x =>
                                        {
                                            x.animations = new()
                                            {

                                            };
                                        }),
                                        entryVariable = 0,
                                        targets = null
                                    }
                                }, new IntentTargetInfo[0]).ability,
                                cost = Cost()
                            }
                        }
                    },
                    new()
                    {
                        health = 16,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Parry_2_A"),
                                cost = Cost(Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Takedown_2_A"),
                                cost = Cost(Pigments.Yellow, Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Wrath_2_A"),
                                cost = Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple)
                            }
                        }
                    },
                    new()
                    {
                        health = 20,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Parry_3_A"),
                                cost = Cost(Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Takedown_3_A"),
                                cost = Cost(Pigments.Yellow, Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Wrath_3_A"),
                                cost = Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple)
                            }
                        }
                    },
                    new()
                    {
                        health = 24,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Parry_4_A"),
                                cost = Cost(Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Takedown_4_A"),
                                cost = Cost(Pigments.Yellow, Pigments.Yellow)
                            },
                            new()
                            {
                                ability = LoadedAssetsHandler.GetCharacterAbility("Wrath_4_A"),
                                cost = Cost(Pigments.Purple, Pigments.Purple, Pigments.Purple)
                            }
                        }
                    }
                };

                x.passiveAbilities = new BasePassiveAbilitySO[] { Passives.Focus };
                x.characterAnimator = null;
                x.characterSprite = LoadSprite("widewak");
                x.characterBackSprite = LoadSprite("widewak_back");
                x.characterOWSprite = LoadSprite("widewak_overworld");
                x.extraCombatSprites = null;
                x.movesOnOverworld = false;
                x.speakerDataName = LoadedAssetsHandler.GetCharcater("Nowak_CH").speakerDataName;
                x.damageSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").damageSound;
                x.deathSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").deathSound;
                x.dxSound = LoadedAssetsHandler.GetCharcater("Nowak_CH").dxSound;
            });

            LoadedAssetsHandler.LoadedCharacters[widewak.name] = widewak;
        }
    }
}
