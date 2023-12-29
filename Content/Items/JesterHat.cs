using BrutalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class JesterHat
    {
        public static void Init()
        {
            var previousDidntFail = CreateScriptable<PreviousEffectCondition>(x =>
            {
                x.previousAmount = 1;
                x.wasSuccessful = true;
            });

            var pirouetteItem = new GenericItem<BasicWearable>("Jester Hat", "\"Can do anything\"", "This party member has Skittish as a passive.\nAdds \"Pirouette\" as an additional ability, a damaging ability with a different additional effect each turn.", "JesterHat", ItemPools.Treasure);
            pirouetteItem.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x => x._extraPassiveAbility = Passives.Skittish),
                CreateScriptable<ExtraAbility_Wearable_SMS>(x => x._extraAbility = new()
                {
                    cost = new ManaColorSO[]
                    {
                        Pigments.SplitPigment(Pigments.Red, Pigments.Blue),
                        Pigments.SplitPigment(Pigments.Purple, Pigments.Yellow)
                    },
                    ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x.abilitySprite = LoadSprite("AttackIcon_Question");
                        x.visuals = null;
                        x._abilityName = "Pirouette";
                        x._description = "Deals 5 indirect damage to the Opposing enemy.\nAdditional effect is different each turn.";
                        x.animationTarget = null;
                        x.intents = new IntentTargetInfo[]
                        {
                            new IntentTargetInfo()
                            {
                                targets = TargettingLibrary.ThisSide,
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Misc
                                }
                            },
                            new IntentTargetInfo()
                            {
                                targets = TargettingLibrary.OpposingSide,
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Misc
                                }
                            },
                            new IntentTargetInfo()
                            {
                                targets = TargettingLibrary.OpposingSlot,
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Damage_3_6
                                }
                            }
                        };
                        x.effects = new EffectInfo[]
                        {
                            //turn 1 - does nothing
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(1, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x =>
                                {
                                    x._animationTarget = TargettingLibrary.ThisSlot;
                                    x._visuals = LoadedAssetsHandler.GetCharacterAbility("Insult_1_A").visuals;
                                })
                            },

                            //turn 2 - scars all enemies
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(2, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x =>
                                {
                                    x._animationTarget = TargettingLibrary.AllEnemiesVisual;
                                    x._visuals = LoadedAssetsHandler.GetEnemyAbility("Struggle_A").visuals;
                                })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.AllEnemies,
                                entryVariable = 1,
                                effect = CreateScriptable<ApplyScarsEffect>()
                            },

                            //turn 3 - applies frail 2 to this party member
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(3, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.ThisSlot; x._visuals = LoadedAssetsHandler.GetEnemyAbility("Struggle_A").visuals; })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.ThisSlot,
                                entryVariable = 2,
                                effect = CreateScriptable<ApplyFrailEffect>()
                            },

                            //turn 4 - applies 3 shield to all positions
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(4, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.ThisSide; x._visuals = LoadedAssetsHandler.GetCharacterAbility("Entrenched_1_A").visuals; })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.ThisSide,
                                entryVariable = 2,
                                effect = CreateScriptable<ApplyShieldSlotEffect>()
                            },

                            //turn 5 - does nothing
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(5, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.ThisSlot; x._visuals = LoadedAssetsHandler.GetCharacterAbility("Insult_1_A").visuals; })
                            },

                            //turn 6 - heal random party member 5-7 health
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(6, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.AllAlliesVisual; x._visuals = LoadedAssetsHandler.GetCharacterAbility("Stitches_1_A").visuals; })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.AllAllies,
                                entryVariable = 5,
                                effect = CreateScriptable<RandomlyHealRandomTargetEffect>(x => { x.maxAdditionalHealing = 2; })
                            },

                            //turn 7 - shuffle health
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(7, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.AllAlliesVisual; x._visuals = LoadedAssetsHandler.GetCharacterAbility("Amalgam_1_A").visuals; })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.AllAllies,
                                entryVariable = 0,
                                effect = CreateScriptable<ShuffleHealthEffect>()
                            },

                            //turn 8 - applies berserk to the opposing enemy
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(8, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x => { x._animationTarget = TargettingLibrary.OpposingSlot; x._visuals = LoadedAssetsHandler.GetEnemyAbility("Unmake_A").visuals; })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.OpposingSlot,
                                entryVariable = 1,
                                effect = CreateScriptable<ApplyBerserkEffect>()
                            },

                            //turn 9 - heal all party members 1-2 health
                            new EffectInfo()
                            {
                                condition = CurrentTurnIsSpecificTurnInRotationCondition.Create(9, 9),
                                effect = CreateScriptable<AnimationVisualsEffect>(x =>
                                {
                                    x._animationTarget = TargettingLibrary.AllAlliesVisual;
                                    x._visuals = LoadedAssetsHandler.GetCharacterAbility("Stitches_1_A").visuals;
                                })
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                effect = CreateScriptable<ExtraVariableForNextEffect>(),
                                entryVariable = 2
                            },
                            new EffectInfo()
                            {
                                condition = previousDidntFail,
                                targets = TargettingLibrary.AllAllies,
                                entryVariable = 4,
                                effect = CreateScriptable<RandomHealBetweenPreviousAndEntryEffect>()
                            },

                            //guaranteed effect
                            new EffectInfo()
                            {
                                condition = null,
                                effect = CreateScriptable<DamageEffect>(x =>
                                {
                                    x._indirect = true;
                                }),
                                entryVariable = 5,
                                targets = TargettingLibrary.OpposingSlot
                            }
                        };
                    })
                })
            };
            pirouetteItem.AddItem();
            pirouetteItem.item.AttachGadget(GadgetDB.GetGadget("Deck of Wonder"));
        }
    }
}
