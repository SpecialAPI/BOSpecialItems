using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    [HarmonyPatch]
    public class FuryStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.UseAbility))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DoFuryAfterAbilityCharacter(IEnumerable<CodeInstruction> instructions)
        {
            var insertFuryAfterNextInstruction = false;
            var insertEndAbilityContextActionAfterNextInstruction = false;
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (insertFuryAfterNextInstruction)
                {
                    insertFuryAfterNextInstruction = false;
                    yield return new CodeInstruction(OpCodes.Ldnull);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Call, furyactions);
                }
                if(instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas && bas.DeclaringType == typeof(StartAbilityCostAction))
                {
                    insertFuryAfterNextInstruction = true;
                }

                if (insertEndAbilityContextActionAfterNextInstruction)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Call, endabilitycontextaction);
                    insertEndAbilityContextActionAfterNextInstruction = false;
                }
                if (instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas2 && bas2.DeclaringType == typeof(EndAbilityAction))
                {
                    insertEndAbilityContextActionAfterNextInstruction = true;
                }
            }
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.UseAbility))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DoFuryAfterAbilityEnemy(IEnumerable<CodeInstruction> instructions)
        {
            var insertFuryAfterNextInstruction = 0;
            var insertEndAbilityContextActionAfterNextInstruction = false;
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (insertFuryAfterNextInstruction > 0)
                {
                    insertFuryAfterNextInstruction--;
                    if(insertFuryAfterNextInstruction == 0)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return new CodeInstruction(OpCodes.Ldnull);
                        yield return new CodeInstruction(OpCodes.Call, furyactions);
                        yield return new CodeInstruction(OpCodes.Call, combatmanagerinst);
                    }
                }
                if (instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas && bas.DeclaringType == typeof(ShowAttackInformationUIAction))
                {
                    insertFuryAfterNextInstruction = 2;
                }

                if (insertEndAbilityContextActionAfterNextInstruction)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldnull);
                    yield return new CodeInstruction(OpCodes.Call, endabilitycontextaction);
                    insertEndAbilityContextActionAfterNextInstruction = false;
                }
                if (instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas2 && bas2.DeclaringType == typeof(EndAbilityAction))
                {
                    insertEndAbilityContextActionAfterNextInstruction = true;
                }
            }
        }

        public static void InsertEndAbilityContextAction(IUnit u, int abilityid, AbilitySO ability, FilledManaCost[] cost)
        {
            CombatManager.Instance.AddRootAction(new EndAbilityContextAction(u, abilityid, ability, cost ?? new FilledManaCost[0]));
        }

        public static void DoAdditionalFuryActions(CombatManager _, IUnit u, AbilitySO ab, int abid, FilledManaCost[] cost)
        {
            if(u != null)
            {
                var furyStacks = ((u is CharacterCombat c) ? c.StatusEffects : (u is EnemyCombat e) ? e.StatusEffects : null)?.Find(x => x is FuryStatusEffect)?.StatusContent ?? 0;
                if (furyStacks > 0 && !ab.name.Contains("NoFuryRepeat"))
                {
                    //CombatManager.Instance.AddUIAction(new PlayStatusEffectSoundAndWaitUIAction("event:/FuryApply", 1f));
                    for (int i = 0; i < furyStacks; i++)
                    {
                        CombatManager.Instance.AddRootAction(new PlayAbilityAnimationAction(ab.visuals, ab.animationTarget, u));
                        CombatManager.Instance.AddRootAction(new EffectAction(ab.effects, u));
                        //CombatManager.Instance.AddRootAction(new EndAbilityFuryAction(u.ID, u.IsUnitCharacter));
                        //CombatManager.Instance.AddRootAction(new EndAbilityContextAction(u, abid, ab, cost ?? new FilledManaCost[0], true));
                        if (!ab.name.Contains("NoFuryTick"))
                        {
                            CombatManager.Instance.AddRootAction(new TickFuryAction(u));
                        }
                    }
                }
            }
        }

        public static MethodInfo furyactions = AccessTools.Method(typeof(FuryStatusEffect), nameof(DoAdditionalFuryActions));
        public static MethodInfo combatmanagerinst = AccessTools.PropertyGetter(typeof(CombatManager), nameof(CombatManager.Instance));
        public static MethodInfo endabilitycontextaction = AccessTools.Method(typeof(FuryStatusEffect), nameof(FuryStatusEffect.InsertEndAbilityContextAction));

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(EffectTick, "BOSpecialItems_FuryTriggered", caller);
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(EffectTick, "BOSpecialItems_FuryTriggered", caller);
        }
    }
}
