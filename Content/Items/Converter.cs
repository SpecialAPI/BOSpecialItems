using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Converter
    {
        public static void Init()
        {
            var item = NewItem<MultiCustomTriggerEffectWearable>("Negative Teapot", "\"Double Negative\"", "This character has permanent TargetSwap as a passive ability.\nAll damage dealt by this character is converted into healing.\nAll healing done by this character is converted into 50% more damage.", "NegativeTeapot", ItemPools.Treasure, 0, false);

            item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = CustomPassives.TargetSwapped; 
                })
            };

            item.triggerEffects = new()
            {
                new EffectsAndMultipleCustomTriggersPair()
                {
                    conditions = new List<EffectorConditionSO>() { CreateScriptable<StoredValueNonPositiveEffectorCondition>(x => x.value = ConverterTriggerEffect.triggering) },
                    doesPopup = true,
                    effect = new ConverterTriggerEffect()
                    {
                        healToDamageModifier = 1.5f,
                        damageToHealModifier = 1f
                    },
                    getsConsumed = false,
                    immediate = true,
                    customTriggers = new() { CustomEvents.WILL_HEAL_UNIT, CustomEvents.WILL_APPLY_DAMAGE_CONTEXT }
                }
            };
        }
    }
}
