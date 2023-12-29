using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    [HarmonyPatch]
    public class ModifyWrongPigmentWearable : BaseWearableSO
    {
        public const string EVENT_NAME = "ModifyWrongPigment";

        public override bool IsItemImmediate => true;

        public override bool DoesItemTrigger => false;

        public int addWrongPigment;
        public int multiplyWrongPigment = 1;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            CombatManager.Instance.AddObserver(TryConsumeWearable, EVENT_NAME, caller);
        }

        public override void TriggerPassive(object sender, object args)
        {
            if (args is IntegerReference i)
            {
                i.value = (i.value * multiplyWrongPigment) + addWrongPigment;
            }
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            CombatManager.Instance.RemoveObserver(TryConsumeWearable, EVENT_NAME, caller);
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.CalculateAbilityCostsDamage))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ModifyWrongPigment(IEnumerable<CodeInstruction> instructions)
        {
            int lcwmCount = 0;
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (instruction.Calls(lcwmGet))
                {
                    lcwmCount++;
                    if(lcwmCount == 2)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Call, mod);
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Call, lcwmGet);
                    }
                }
            }
        }

        public static void ModifyWrong(int current, CharacterCombat cc)
        {
            var intRef = new IntegerReference(current);
            CombatManager.Instance.PostNotification(EVENT_NAME, cc, intRef);
            cc.LastCalculatedWrongMana = Mathf.Max(intRef.value, 0);
        }

        public static MethodInfo mod = AccessTools.Method(typeof(ModifyWrongPigmentWearable), nameof(ModifyWrong));
        public static MethodInfo lcwmGet = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.LastCalculatedWrongMana));
    }
}
