using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class UntetheredHealthItem
    {
        public static BasePassiveAbilitySO UntetheredHealth;
        public static BasePassiveAbilitySO RedHealth;
        public static BasePassiveAbilitySO BlueHealth;
        public static BasePassiveAbilitySO YellowHealth;
        public static BasePassiveAbilitySO PurpleHealth;

        public static void Init()
        {
            UntetheredHealth = CreateScriptable<PerformEffectOnAttachPassive>(x =>
            {
                x._passiveName = "Untethered Core";
                x._characterDescription = "Allows this party member's health to be toggled to any basic color.";
                x._enemyDescription = "Allows this enemy's health to be toggled to any basic color.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.Count };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("UntetheredHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("UntetheredHealthColor");
                x.immediate = false;
                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddHealthOptionIfUnitDoesntHaveItEffect>(x => x.healthColorsToAdd = new()
                        {
                            Pigments.Red,
                            Pigments.Blue,
                            Pigments.Yellow,
                            Pigments.Purple
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });
            RedHealth = CreateScriptable<PerformEffectOnAttachPassive>(x =>
            {
                x._passiveName = "Red Core";
                x._characterDescription = "Allows this party member's health to be toggled to red.";
                x._enemyDescription = "Allows this enemy's health to be toggled to red.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.Count };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("RedHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("RedHealthColor");
                x.immediate = false;
                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddHealthOptionIfUnitDoesntHaveItEffect>(x => x.healthColorsToAdd = new()
                        {
                            Pigments.Red
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });
            BlueHealth = CreateScriptable<PerformEffectOnAttachPassive>(x =>
            {
                x._passiveName = "Blue Core";
                x._characterDescription = "Allows this party member's health to be toggled to blue.";
                x._enemyDescription = "Allows this enemy's health to be toggled to blue.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.Count };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("BlueHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("BlueHealthColor");
                x.immediate = false;
                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddHealthOptionIfUnitDoesntHaveItEffect>(x => x.healthColorsToAdd = new()
                        {
                            Pigments.Blue
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });
            YellowHealth = CreateScriptable<PerformEffectOnAttachPassive>(x =>
            {
                x._passiveName = "Yellow Core";
                x._characterDescription = "Allows this party member's health to be toggled to yellow.";
                x._enemyDescription = "Allows this enemy's health to be toggled to yellow.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.Count };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("YellowHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("YellowHealthColor");
                x.immediate = false;
                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddHealthOptionIfUnitDoesntHaveItEffect>(x => x.healthColorsToAdd = new()
                        {
                            Pigments.Yellow
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });
            PurpleHealth = CreateScriptable<PerformEffectOnAttachPassive>(x =>
            {
                x._passiveName = "Purple Core";
                x._characterDescription = "Allows this party member's health to be toggled to purple.";
                x._enemyDescription = "Allows this enemy's health to be toggled to purple.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.Count };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("PurpleHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("PurpleHealthColor");
                x.immediate = false;
                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddHealthOptionIfUnitDoesntHaveItEffect>(x => x.healthColorsToAdd = new()
                        {
                            Pigments.Purple
                        }),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });

            var untetheredHealthItem = new GenericItem<PerformEffectWearable>("Artist's Palette", "\"Primary Colors\"", "Adds Untethered Core to this party member as a passive.\nAt the beginning of combat, add Untethered Core to the opposing enemy as a passive.\nCore allows health color to be toggled to other colors.", "Palette", ItemPools.Treasure);
            untetheredHealthItem.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = UntetheredHealth;
                })
            };
            untetheredHealthItem.item.triggerOn = TriggerCalls.OnCombatStart;
            untetheredHealthItem.item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddPassiveEffect>(x => x._passiveToAdd = UntetheredHealth),
                    entryVariable = 0,
                    targets = TargettingLibrary.OpposingSlot
                }
            };
            untetheredHealthItem.AddItem();
            untetheredHealthItem.item.AttachGadget(GadgetDB.GetGadget("Flood"));
        }
    }
}
