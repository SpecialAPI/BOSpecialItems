using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOSpecialItems.Content.Items.PassiveFlags
{
    [HarmonyPatch]
    public class WearableStaticFlagPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void OnPassiveConnected(IUnit unit)
        {
            Debug.LogError("uh oh");
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            Debug.LogError("uh oh");
        }

        public override void TriggerPassive(object sender, object args)
        {
            Debug.LogError("uh oh");
        }

        [HarmonyPatch(typeof(CharacterCombat), MethodType.Constructor, typeof(int), typeof(int), typeof(CharacterSO), typeof(bool), typeof(int), typeof(int), typeof(int), typeof(int), typeof(BaseWearableSO), typeof(WearableStaticModifiers), typeof(bool), typeof(string), typeof(bool))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SkipFlagsInConstructor(IEnumerable<CodeInstruction> instructions)
        {
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (instruction.Calls(equals))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Call, check);
                }
            }
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.TrySetUpNewItem))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SkipFlagsInNewItem(IEnumerable<CodeInstruction> instructions)
        {
            var equalscount = 0;
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.Calls(equals) && equalscount < 3)
                {
                    equalscount++;
                    if(equalscount == 3)
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_3);
                        yield return new CodeInstruction(OpCodes.Call, check);
                    }
                }
            }
        }

        public static bool CheckIfWearableStaticFlag(bool prev, BasePassiveAbilitySO d)
        {
            return prev || d is WearableStaticFlagPassive;
        }

        public static MethodInfo extrapassives = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.ItemExtraPassives));
        public static MethodInfo check = AccessTools.Method(typeof(WearableStaticFlagPassive), nameof(CheckIfWearableStaticFlag));
        public static MethodInfo equals = AccessTools.Method(typeof(object), nameof(object.Equals), new Type[] { typeof(object) });
    }
}
