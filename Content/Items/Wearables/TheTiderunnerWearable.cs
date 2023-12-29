using MUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables
{
    [HarmonyPatch]
    public class TheTiderunnerWearable : BaseWearableSO
    {
        public override bool IsItemImmediate => false;

        public override bool DoesItemTrigger => true;

        public EffectInfo[] leftEffects;
        public EffectInfo[] rightEffects;

        public override void CustomOnTriggerAttached(IWearableEffector caller)
        {
            CombatManager.Instance.AddObserver(TryConsumeWearable, EndAbilityContextAction.NOTIFICATION_NAME, caller);
        }

        public override void CustomOnTriggerDettached(IWearableEffector caller)
        {
            CombatManager.Instance.RemoveObserver(TryConsumeWearable, EndAbilityContextAction.NOTIFICATION_NAME, caller);
        }

        public override void TriggerPassive(object sender, object args)
        {
            if(args is EndAbilityContextAction context && sender is CharacterCombat cc && cc.CombatAbilities.Count > 1)
            {
                if(context.AbilityID == 0)
                {
                    CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction((sender as IWearableEffector).ID, GetItemLocData().text, false, wearableImage));
                    if (IsItemImmediate)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(leftEffects, cc));
                    }
                    else
                    {
                        CombatManager.Instance.AddSubAction(new EffectAction(leftEffects, cc));
                    }
                }
                else if(context.AbilityID == cc.CombatAbilities.Count - 1)
                {
                    CombatManager.Instance.AddUIAction(new ShowItemInformationUIAction((sender as IWearableEffector).ID, GetItemLocData().text, false, wearableImage));
                    if (IsItemImmediate)
                    {
                        CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(leftEffects, cc));
                    }
                    else
                    {
                        CombatManager.Instance.AddSubAction(new EffectAction(rightEffects, cc));
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.SetUpDefaultAbilities))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> InsertAbilityModifier(IEnumerable<CodeInstruction> instructions)
        {
            foreach(var instruction in instructions)
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

        public static MethodInfo abilitymodifier = AccessTools.Method(typeof(TheTiderunnerWearable), nameof(TheTiderunnerWearable.CharacterAbilityModifier));
        public static MethodInfo clampedrank = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.ClampedRank));
        public static MethodInfo modify = AccessTools.Method(typeof(WeakenedStatusEffect), nameof(WeakenedStatusEffect.ModifyAbilityRank));

        public static void CharacterAbilityModifier(bool _, CharacterCombat cc)
        {
            if (cc.CharacterWearableModifiers.HasFlag<MoveSlapRightFlag>() && (cc.CharacterWearableModifiers.UsesBasicBool ? cc.CharacterWearableModifiers.UsesBasicAbilityModifier : cc.Character.usesBasicAbility) && cc.CombatAbilities.Count > 2)
            {
                cc.CombatAbilities.Swap(0, 1);
            }
        }
    }

    public class MoveSlapRightFlag : WearableStaticFlagPassive
    {
    }
}
