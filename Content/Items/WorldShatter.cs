using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class WorldShatter
    {
        public static void Init()
        {
            var shatter = new GenericItem<BasicWearable>("World Shatter", "\"Goodbye\"", "Adds \"End of the Universe\" as an additional ability.", "WorldShatter", ItemPools.Treasure);
            shatter.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraAbility_Wearable_SMS>(x =>
                {
                    x._extraAbility = new()
                    {
                        cost = new ManaColorSO[]
                        {
                            Pigments.Red,
                            Pigments.Blue,
                            Pigments.Yellow,
                            Pigments.Purple
                        },
                        ability = CreateScriptable<AbilitySO>(x =>
                        {
                            x.visuals = ((LoadedAssetsHandler.GetWearable("DemonCore_SW") as PerformEffectWearable).effects.First(x => x.effect is AnimationVisualsEffect).effect as AnimationVisualsEffect)._visuals;
                            x.abilitySprite = LoadSprite("AttackIcon_EOTU");
                            x.intents = new IntentTargetInfo[]
                            {
                                new()
                                {
                                    targets = TargettingLibrary.ThisSide,
                                    targetIntents = new IntentType[]
                                    {
										IntentType.Damage_1_2,
										IntentType.Damage_3_6,
										IntentType.Damage_7_10,
										IntentType.Damage_11_15,
										IntentType.Damage_16_20,
										IntentType.Damage_21,
										IntentType.Damage_Death,
										IntentType.Swap_Sides, //yea
										IntentType.Swap_Right, //yea
										IntentType.Swap_Left, //yea
										IntentType.Swap_Mass, //yea
										IntentType.Mana_Generate, //yea
										IntentType.Mana_Consume, //yea
										//IntentType.Mana_Randomize,
										//IntentType.Mana_Modify,
										IntentType.Misc,
										IntentType.Status_Frail, //yea
										IntentType.Status_Ruptured, //yea
										IntentType.Status_Cursed, //yea
										IntentType.Status_Linked, //yea
										IntentType.Status_Spotlight, //yea
										IntentType.Status_Focused, //yea
										IntentType.Status_OilSlicked, //yea
										IntentType.Status_DivineProtection, //yea
										IntentType.Status_Scars, //yea
										IntentType.Field_Constricted, //yea
										IntentType.Field_Shield, //yea
										IntentType.Field_Fire //yea
									}
                                },
                                new()
                                {
                                    targets = TargettingLibrary.OpposingSide,
									targetIntents = new IntentType[]
									{
										IntentType.Damage_1_2,
										IntentType.Damage_3_6,
										IntentType.Damage_7_10,
										IntentType.Damage_11_15,
										IntentType.Damage_16_20,
										IntentType.Damage_21,
										IntentType.Damage_Death,
										IntentType.Swap_Sides, //yea
										IntentType.Swap_Right, //yea
										IntentType.Swap_Left, //yea
										IntentType.Swap_Mass, //yea
										IntentType.Mana_Generate, //yea
										IntentType.Mana_Consume, //yea
										//IntentType.Mana_Randomize,
										//IntentType.Mana_Modify,
										IntentType.Misc,
										IntentType.Status_Frail, //yea
										IntentType.Status_Ruptured, //yea
										IntentType.Status_Cursed, //yea
										IntentType.Status_Linked, //yea
										IntentType.Status_Spotlight, //yea
										IntentType.Status_Focused, //yea
										IntentType.Status_OilSlicked, //yea
										IntentType.Status_DivineProtection, //yea
										IntentType.Status_Scars, //yea
										IntentType.Field_Constricted, //yea
										IntentType.Field_Shield, //yea
										IntentType.Field_Fire //yea
									}
								}
                            };
							x._abilityName = "End of the Universe";
							x._description = "Performs most effects on random slots, both ally and enemy.\n\"At least it doesn't crash the game\"";
							x.animationTarget = null;

							var targetting = CreateScriptable<TargettingAllSlots>();

							var converter = (EffectInfo i) => new EffectInfo()
							{
								condition = null,
								effect = CreateScriptable<PerformEffectOnRandomTargets>(x =>
								{
									x.effect = i;
									x.targetsToPerformOn = 1;
									x.onlyPerformOnUnitSlots = false;
								}),
								entryVariable = 0,
								targets = targetting
							};

							x.effects = new EffectInfo[]
							{
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<GenerateColorManaEffect>(x =>
                                    {
										x.mana = Pigments.Red;
                                    }),
									entryVariable = 1
								},
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<GenerateColorManaEffect>(x =>
									{
										x.mana = Pigments.Blue;
									}),
									entryVariable = 1
								},
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<GenerateColorManaEffect>(x =>
									{
										x.mana = Pigments.Yellow;
									}),
									entryVariable = 1
								},
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<GenerateColorManaEffect>(x =>
									{
										x.mana = Pigments.Purple;
									}),
									entryVariable = 1
								},
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyFrailEffect>(),
									entryVariable = 1
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyScarsEffect>(),
									entryVariable = 5
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplySpotlightEffect>(),
									entryVariable = 1
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyCursedEffect>(),
									entryVariable = 1
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyDivineProtectionEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyLinkedEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyOilSlickedEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyRupturedEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyFocusedEffect>(),
									entryVariable = 1
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyConstrictedSlotEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyFireSlotEffect>(),
									entryVariable = 3
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ApplyShieldSlotEffect>(),
									entryVariable = 7
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false)
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<SwapToOneSideEffect>(x => x._swapRight = false)
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<SwapToSidesEffect>(),
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<SwapTargetsToRandomSlotsEffect>(),
									entryVariable = 7
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 1
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 5
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 8
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 12
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 18
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 22
								}),
								converter(new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<DamageEffect>(),
									entryVariable = 9999
								}),
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ConsumeRandomManaEffect>(),
									entryVariable = 3
								},
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ConsumeRandomManaEffect>(),
									entryVariable = 3
								},
								new()
								{
									condition = null,
									targets = null,
									effect = CreateScriptable<ConsumeRandomManaEffect>(),
									entryVariable = 3
								},
                                new()
                                {
									condition = null,
									targets = null,
									effect = CreateScriptable<RandomizeAllManaEffect>(x => x.manaRandomOptions = new ManaColorSO[]
                                    {
										Pigments.Red,
										Pigments.Blue,
										Pigments.Yellow,
										Pigments.Purple
                                    })
                                }
							};
						})
                    };
                })
            };
			shatter.AddItem();

			shatter.item.AttachGadget(GadgetDB.SetupGadget("Death's Door", "Deal 977106737 damage to the opposing enemy.\n10% chance to close the game and delete the current run.\nI wanted to make the damage number even bigger but unfortunately the int limit is a thing.", null, null, Cost(Pigments.Red, Pigments.Red, Pigments.Red, Pigments.Red, Pigments.Red), new EffectInfo[]
			{
				new()
				{
					condition = null,
					effect = CreateScriptable<DamageEffect>(),
					entryVariable = 977106737,
					targets = TargettingLibrary.OpposingSlot
				},
				new()
				{
					condition = CreateScriptable<PercentageEffectCondition>(x => x.percentage = 10),
					effect = CreateScriptable<FuckYouEffect>(),
					entryVariable = 0,
					targets = null
				},
			}, new IntentTargetInfo[]
			{
				new()
				{
					targetIntents = new IntentType[]
					{
						IntentType.Damage_Death,
						Intent("FuckYou")
					},
					targets = TargettingLibrary.OpposingSlot
				}
			}, overrideVisuals: ((LoadedAssetsHandler.GetWearable("DemonCore_SW") as PerformEffectWearable).effects.First(x => x.effect is AnimationVisualsEffect).effect as AnimationVisualsEffect)._visuals));

			AddIntent("FuckYou", new IntentInfoBasic()
			{
				_color = Color.white,
				_sound = "",
				_sprite = LoadSprite("Intent_FuckYou")
			});
		}
    }
}
