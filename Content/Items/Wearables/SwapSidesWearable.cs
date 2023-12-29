using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    [HarmonyPatch]
    public class SwapSidesWearable : BaseWearableSO
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(EffectInfo), nameof(EffectInfo.StartEffect))]
        public static IEnumerable<CodeInstruction> SwapEffects(IEnumerable<CodeInstruction> instructions)
        {
            var timesLoadingEffect = 0;
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (instruction.LoadsField(effect) && timesLoadingEffect < 2)
                {
                    timesLoadingEffect++;
                    if(timesLoadingEffect == 2)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_2);
                        yield return new CodeInstruction(OpCodes.Call, converteffect);
                    }
                }
            }
        }

        public static EffectSO ConvertEffect(EffectSO input, IUnit caster)
        {
            if (caster.HeldItem != null && caster.HeldItem is SwapSidesWearable && input is DamageEffect or HealEffect or ApplyConstrictedSlotEffect or ApplyFireSlotEffect or ApplyShieldSlotEffect)
            {
                if(input is DamageEffect dmg)
                {
                    if(heal == null)
                    {
                        heal = CreateInstance<HealEffect>();
                        heal.name = "CONVERTER_TEMP_EFFECT_Heal";
                    }
                    heal.usePreviousExitValue = dmg._usePreviousExitValue;
                    return heal;
                }
                else if(input is HealEffect hl)
                {
                    if (damage == null)
                    {
                        damage = CreateInstance<DamageEffect>();
                        damage.name = "CONVERTER_TEMP_EFFECT_Damage";
                    }
                    damage._usePreviousExitValue = hl.usePreviousExitValue;
                    return damage;
                }
                else if(input is ApplyConstrictedSlotEffect or ApplyFireSlotEffect)
                {
                    if (shield == null)
                    {
                        shield = CreateInstance<ApplyShieldSlotEffect>();
                        shield.name = "CONVERTER_TEMP_EFFECT_Shield";
                    }
                    return shield;
                }
                else if(input is ApplyShieldSlotEffect)
                {
                    if (fire == null)
                    {
                        fire = CreateInstance<ApplyFireSlotEffect>();
                        fire.name = "CONVERTER_TEMP_EFFECT_Fire";
                    }
                    return fire;
                }
            }
            return input;
        }

        public static HealEffect heal;
        public static DamageEffect damage;
        public static ApplyShieldSlotEffect shield;
        public static ApplyFireSlotEffect fire;

        public static FieldInfo effect = AccessTools.Field(typeof(EffectInfo), nameof(EffectInfo.effect));
        public static MethodInfo converteffect = AccessTools.Method(typeof(SwapSidesWearable), nameof(ConvertEffect));
        //public static MethodInfo convertentry = AccessTools.Method(typeof(SwapSidesWearable), nameof(ConvertEntry));

        public override bool IsItemImmediate => false;
        public override bool DoesItemTrigger => false;
    }
}
