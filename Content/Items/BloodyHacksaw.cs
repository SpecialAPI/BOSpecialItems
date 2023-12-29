using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class BloodyHacksaw
    {
        public static void Init()
        {
            var hacksaw = new GenericItem<BasicWearable>("Bloody Hacksaw", "\"Two into one!\"", "Adds \"Two Into One\" as an additional ability, a weak attack with the ability to merge enemies.", "BloodyHacksaw", ItemPools.Shop, 5);
            hacksaw.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraAbility_Wearable_SMS>(x =>
                {
                    x._extraAbility = new CharacterAbility()
                    {
                        cost = Cost(Pigments.Red, Pigments.Red, Pigments.Purple),
                        ability = CreateScriptable<AbilitySO>(x =>
                        {
                            x.abilitySprite = LoadSprite("AttackIcon_TwoIntoOne");
                            x.visuals = GetAnyAbility("FlayTheFlesh_A").visuals;
                            x.animationTarget = TargettingLibrary.Relative(false, -1, 1);
                            x._abilityName = "Two Into One";
                            x._description = "Deal 5 damage to the left and right enemies.\nIf the left and right enemies are different enemies of the same type, deal double damage and merge them if they survive.";
                            x.effects = new EffectInfo[]
                            {
                                new()
                                {
                                    condition = null,
                                    effect = CreateScriptable<CheckDuplicateEnemiesEffect>(),
                                    entryVariable = 0,
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                },
                                new()
                                {
                                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = false; }),
                                    effect = CreateScriptable<DamageEffect>(),
                                    entryVariable = 5,
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                },
                                new()
                                {
                                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 2; x.wasSuccessful = true; }),
                                    effect = CreateScriptable<DamageEffect>(),
                                    entryVariable = 10,
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                },
                                new()
                                {
                                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 3; x.wasSuccessful = true; }),
                                    effect = CreateScriptable<CheckDuplicateEnemiesEffect>(),
                                    entryVariable = 0,
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                },
                                new()
                                {
                                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                                    effect = CreateScriptable<AnimationVisualsEffect>(x => { x._visuals = GetAnyAbility("Entwined_1_A").visuals; x._animationTarget = TargettingLibrary.Relative(false, -1, 1); }),
                                    entryVariable = 0,
                                    targets = null
                                },
                                new()
                                {
                                    condition = CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = 1; x.wasSuccessful = true; }),
                                    effect = CreateScriptable<MergeEnemiesEffect>(),
                                    entryVariable = 0,
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                },
                            };
                            x.intents = new IntentTargetInfo[]
                            {
                                new()
                                {
                                    targetIntents = new IntentType[]
                                    {
                                        IntentType.Damage_3_6,
                                        IntentType.Damage_3_6,
                                        IntentType.Misc
                                    },
                                    targets = TargettingLibrary.Relative(false, -1, 1)
                                }
                            };
                        })
                    };
                })
            };
            hacksaw.AddItem();
            hacksaw.item.AttachGadget(GadgetDB.GetGadget("Deck of Wonder"));
        }
    }
}
