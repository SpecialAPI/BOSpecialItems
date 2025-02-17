using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public static class CustomPassives
    {
        public static BasePassiveAbilitySO TargetShift_Left;
        public static BasePassiveAbilitySO TargetShift_Right;
        public static BasePassiveAbilitySO TargetShift_FarLeft;
        public static BasePassiveAbilitySO TargetShift_FarRight;

        public static BasePassiveAbilitySO Merged;

        public static BasePassiveAbilitySO UntetheredHealth;
        public static BasePassiveAbilitySO RedHealth;
        public static BasePassiveAbilitySO BlueHealth;
        public static BasePassiveAbilitySO YellowHealth;
        public static BasePassiveAbilitySO PurpleHealth;

        public static BasePassiveAbilitySO Shapeshifter;

        public static BasePassiveAbilitySO TargetSwapped;
        public static BasePassiveAbilitySO Frenzied;

        private static readonly Dictionary<int, BasePassiveAbilitySO> Furiouses = new();
        private static readonly Dictionary<int, BasePassiveAbilitySO> Weaks = new();
        private static readonly Dictionary<int, BasePassiveAbilitySO> Energizedses = new();

        public static BasePassiveAbilitySO Furious(int amount)
        {
            return Furiouses.LookForOrCreate(amount, a => CreatePassive<FuriousPassiveAbility>($"Furious ({a})", $"Permanently applies Fury ({a}) to self.\nFury makes abilities performed by this {{0}} be repeated for each stack of Fury.", "Furious", x =>
            {
                x.amount = a;
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
            }, "Furious"));
        }

        public static BasePassiveAbilitySO Weak(int amount)
        {
            return Weaks.LookForOrCreate(amount, a => CreatePassive<WeakPassiveAbility>($"Weak ({a})", $"Permanently applies Weakened ({a}) to self.\n", "Weak", x =>
            {
                x.amount = a;
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];

                x._characterDescription += "Weakened makes this party member's abilities act as if they were 1 level lower for each stack of Weakened.";
                x._enemyDescription += "Weakened multiplies damage dealt by this enemy by 0.85 for each stack of Weakened.";
            }, "Weak"));
        }

        public static BasePassiveAbilitySO Energized(int amount)
        {
            return Energizedses.LookForOrCreate(amount, a => CreatePassive<EnergizedPassiveAbility>($"Energized ({a})", $"Permanently applies Powered Up ({a}) to self.\n", "Energized", x =>
            {
                x.amount = a;
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];

                x._characterDescription += "Powered Up makes this party member's abilities act as if they were 1 level higher for each stack of Powered Up.";
                x._enemyDescription += "Powered Up increases damage dealt by this enemy by 25% for each stack of Powered Up.";
            }, "Energized"));
        }

        public static void Init()
        {
            TargetShift_Left = CreateScriptable<TargetShiftPassive>(x =>
            {
                x.shift = -1;
                x.passiveIcon = LoadSprite("TargetShift_L");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_Left");
                x._passiveName = "TargetShift (Left)";
                x._characterDescription = "All abilities performed by this party member are performed as if the caster is on the space left of them.";
                x._enemyDescription = "All abilities performed by this enemy are performed as if the caster is on the space left of them.";
            });
            TargetShift_Right = CreateScriptable<TargetShiftPassive>(x =>
            {
                x.shift = 1;
                x.passiveIcon = LoadSprite("TargetShift_R");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_Right");
                x._passiveName = "TargetShift (Right)";
                x._characterDescription = "All abilities performed by this party member are performed as if the caster is on the space right of them.";
                x._enemyDescription = "All abilities performed by this enemy are performed as if the caster is on the space right of them.";
            });
            TargetShift_FarLeft = CreateScriptable<TargetShiftPassive>(x =>
            {
                x.shift = -1;
                x.passiveIcon = LoadSprite("TargetShift_FL");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_FarLeft");
                x._passiveName = "TargetShift (Far Left)";
                x._characterDescription = "All abilities performed by this party member are performed as if the caster is on the space far left of them.";
                x._enemyDescription = "All abilities performed by this enemy are performed as if the caster is on the space far left of them.";
            });
            TargetShift_FarRight = CreateScriptable<TargetShiftPassive>(x =>
            {
                x.shift = 1;
                x.passiveIcon = LoadSprite("TargetShift_FR");
                x.type = ExtendEnum<PassiveAbilityTypes>("TargetShift_FarRight");
                x._passiveName = "TargetShift (Far Right)";
                x._characterDescription = "All abilities performed by this party member are performed as if the caster is on the space far right of them.";
                x._enemyDescription = "All abilities performed by this enemy are performed as if the caster is on the space far right of them.";
            });

            Merged = CreateScriptable<IntegerSetterByStoredValuePassiveAbility>(x =>
            {
                x._valueName = x.specialStoredValue = StoredValue("MergedCount");
                x._postIncreaseStored = false;
                x.postIncreaseValue = 0;
                x._passiveName = "Merged";
                x._characterDescription = "This character is Merged";
                x._enemyDescription = "This enemy will perform an additional ability for each enemy merged into it.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.AttacksPerTurn };
                x.doesPassiveTriggerInformationPanel = false;
                x.passiveIcon = LoadSprite("Merged");
                x.type = ExtendEnum<PassiveAbilityTypes>("Merged");
            });

            UntetheredHealth = CreateScriptable<Connection_PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Untethered Core";
                x._characterDescription = "Allows this party member's health to be toggled to any basic color.";
                x._enemyDescription = "Allows this enemy's health to be toggled to any basic color.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("UntetheredHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("UntetheredHealthColor");
                x.connectionEffects = new EffectInfo[]
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
                x.immediateEffect = false;
                x.disconnectionEffects = new EffectInfo[0];
            });
            RedHealth = CreateScriptable<Connection_PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Red Core";
                x._characterDescription = "Allows this party member's health to be toggled to red.";
                x._enemyDescription = "Allows this enemy's health to be toggled to red.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("RedHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("RedHealthColor");
                x.connectionEffects = new EffectInfo[]
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
                x.immediateEffect = false;
                x.disconnectionEffects = new EffectInfo[0];
            });
            BlueHealth = CreateScriptable<Connection_PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Blue Core";
                x._characterDescription = "Allows this party member's health to be toggled to blue.";
                x._enemyDescription = "Allows this enemy's health to be toggled to blue.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("BlueHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("BlueHealthColor");
                x.connectionEffects = new EffectInfo[]
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
                x.immediateEffect = false;
                x.disconnectionEffects = new EffectInfo[0];
            });
            YellowHealth = CreateScriptable<Connection_PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Yellow Core";
                x._characterDescription = "Allows this party member's health to be toggled to yellow.";
                x._enemyDescription = "Allows this enemy's health to be toggled to yellow.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("YellowHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("YellowHealthColor");
                x.connectionEffects = new EffectInfo[]
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
                x.immediateEffect = false;
                x.disconnectionEffects = new EffectInfo[0];
            });
            PurpleHealth = CreateScriptable<Connection_PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Purple Core";
                x._characterDescription = "Allows this party member's health to be toggled to purple.";
                x._enemyDescription = "Allows this enemy's health to be toggled to purple.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("PurpleHealthColor");
                x.type = ExtendEnum<PassiveAbilityTypes>("PurpleHealthColor");
                x.connectionEffects = new EffectInfo[]
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
                x.immediateEffect = false;
                x.disconnectionEffects = new EffectInfo[0];
            });

            Shapeshifter = CreateScriptable<PerformEffectPassiveAbility>(x =>
            {
                x._passiveName = "Shape-Shifter";
                x._characterDescription = "At the start of each turn, unequip this party member's held item and equip a random treasure item. Attempt to trigger that item's on combat start effects. If this passive ability was granted by an item, it will not be removed when the item is unequipped in combat.";
                x._enemyDescription = "This passive is not meant for enemies.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnTurnStart };
                x.doesPassiveTriggerInformationPanel = true;
                x.passiveIcon = LoadSprite("Shapeshifter");
                x.type = ExtendEnum<PassiveAbilityTypes>("Shapeshifter");

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<EquipRandomTreasureEffect>(),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    }
                };
            });

            TargetSwapped = CreatePassive<TargetSwappedPassiveAbility>("TargetSwapped", "Permanently applies TargetSwap to self.\nTargetSwap makes this {0}'s abilities target as if they were on the {1} side.", "TargetSwapped", x =>
            {
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
            });
            Frenzied = CreatePassive<FrenziedPassiveAbility>("Frenzied", "Permanently applies Berserk to self.\nBerserk makes this {0} deal double damage.", "Frenzied", x =>
            {
                x.conditions = null;
                x._triggerOn = new TriggerCalls[0];
            });
        }
    }
}
