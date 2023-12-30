using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    [HarmonyPatch]
    public static class CombatAbilityModifiers
    {
        public static void CharacterAbilityModifier(bool _, CharacterCombat cc)
        {
            if (cc.CharacterWearableModifiers.HasFlag<MoveSlapRightFlag>() && (cc.CharacterWearableModifiers.UsesBasicBool ? cc.CharacterWearableModifiers.UsesBasicAbilityModifier : cc.Character.usesBasicAbility) && cc.CombatAbilities.Count > 2)
            {
                cc.CombatAbilities.Swap(0, 1);
            }
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.SetUpDefaultAbilities))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> InsertAbilityModifier(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Ldarg_1)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, abilitymodifier);

                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                }
                if (instruction.Calls(clampedrank))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, modify);
                }
            }
        }

        public static int ModifyAbilityRank(int current, CharacterCombat cc)
        {
            var intref = new IntegerReference(current);
            CombatManager.Instance.PostNotification(CustomEvents.MODIFY_ABILITIES_RANK, cc, intref);
            return cc.Character.ClampRank(intref.value);
        }

        public static MethodInfo abilitymodifier = AccessTools.Method(typeof(CombatAbilityModifiers), nameof(CombatAbilityModifiers.CharacterAbilityModifier));
        public static MethodInfo clampedrank = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.ClampedRank));
        public static MethodInfo modify = AccessTools.Method(typeof(CombatAbilityModifiers), nameof(CombatAbilityModifiers.ModifyAbilityRank));
    }
}
