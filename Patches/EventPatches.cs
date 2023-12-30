using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class EventPatches
    {
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.CalculateAbilityCostsDamage))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ModifyWrongPigment(IEnumerable<CodeInstruction> instructions)
        {
            int lcwmCount = 0;
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.Calls(lcwmGet))
                {
                    lcwmCount++;
                    if (lcwmCount == 2)
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
            CombatManager.Instance.PostNotification(CustomEvents.MODIFY_WRONG_PIGMENT, cc, intRef);
            cc.LastCalculatedWrongMana = Mathf.Max(intRef.value, 0);
        }

        [HarmonyPatch]
        public static class ExecutePatchNonImmediate
        {
            [HarmonyTargetMethod]
            public static MethodBase Method()
            {
                return AccessTools.Method(t, "MoveNext");
            }

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Execute(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var instruction in instructions)
                {
                    if (instruction.opcode == OpCodes.Ret)
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_1);
                        yield return new CodeInstruction(OpCodes.Call, rfl);
                    }
                    yield return instruction;
                    if (instruction.LoadsField(isCasterCharacter))
                    {
                        // caster
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load EffectAction
                        yield return new CodeInstruction(OpCodes.Ldfld, effectCaster); // read caster from EffectAction

                        // action
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load EffectAction

                        // effect idx
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // load this enumerator
                        yield return new CodeInstruction(OpCodes.Ldfld, effectIdx); // read effect idx;

                        yield return new CodeInstruction(OpCodes.Call, ccc);
                    }
                    if (instruction.Calls(slotid))
                    {
                        // caster
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load EffectAction
                        yield return new CodeInstruction(OpCodes.Ldfld, effectCaster); // read caster from EffectAction

                        // action
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load EffectAction

                        // effect idx
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // load this enumerator
                        yield return new CodeInstruction(OpCodes.Ldfld, effectIdx); // read effect idx;

                        yield return new CodeInstruction(OpCodes.Call, ccsi);
                    }
                }
            }

            public static Type t = AccessTools.TypeByName("EffectAction+<Execute>d__4");
            public static FieldInfo effectIdx = AccessTools.Field(t, "<i>5__4");
            public static FieldInfo isCasterCharacter = AccessTools.Field(t, "<isCasterCharacter>5__2");
            public static FieldInfo effectCaster = AccessTools.Field(typeof(EffectAction), nameof(EffectAction._caster));
            public static MethodInfo slotid = AccessTools.PropertyGetter(typeof(IUnit), nameof(IUnit.SlotID));
        }

        [HarmonyPatch]
        public static class VisualsPatch
        {
            [HarmonyTargetMethod]
            public static MethodBase Method()
            {
                return AccessTools.Method(t, "MoveNext");
            }

            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> Execute(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var instruction in instructions)
                {
                    yield return instruction;
                    if (instruction.Calls(character))
                    {
                        // caster
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load PlayAbilityAnimationAction
                        yield return new CodeInstruction(OpCodes.Ldfld, animCaster); // read caster from PlayAbilityAnimationAction

                        // action
                        yield return new CodeInstruction(OpCodes.Ldnull); // load null

                        // effect idx
                        yield return new CodeInstruction(OpCodes.Ldc_I4_0); // load 0

                        yield return new CodeInstruction(OpCodes.Call, ccc);
                    }
                    if (instruction.Calls(slotid))
                    {
                        // caster
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // load PlayAbilityAnimationAction
                        yield return new CodeInstruction(OpCodes.Ldfld, animCaster); // read caster from PlayAbilityAnimationAction

                        // action
                        yield return new CodeInstruction(OpCodes.Ldnull); // load null

                        // effect idx
                        yield return new CodeInstruction(OpCodes.Ldc_I4_0); // load 0

                        yield return new CodeInstruction(OpCodes.Call, ccsi);
                    }
                }
            }

            public static Type t = AccessTools.TypeByName("PlayAbilityAnimationAction+<Execute>d__4");
            public static FieldInfo animCaster = AccessTools.Field(typeof(PlayAbilityAnimationAction), nameof(PlayAbilityAnimationAction._caster));
            public static MethodInfo character = AccessTools.PropertyGetter(typeof(IUnit), nameof(IUnit.IsUnitCharacter));
            public static MethodInfo slotid = AccessTools.PropertyGetter(typeof(IUnit), nameof(IUnit.SlotID));
        }

        [HarmonyPatch]
        public static class AddEffectsPatch
        {
            [HarmonyTranspiler]
            public static IEnumerable<CodeInstruction> AddEffectsToList(IEnumerable<CodeInstruction> instructions)
            {
                foreach (var instruction in instructions)
                {
                    yield return instruction;
                    if (instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas && bas.DeclaringType == typeof(EffectAction))
                    {
                        yield return new CodeInstruction(OpCodes.Call, atl);
                    }
                }
            }

            [HarmonyTargetMethods]
            public static IEnumerable<MethodBase> Methods()
            {
                return new MethodBase[]
                {
                    AccessTools.Method(typeof(CharacterCombat), nameof(CharacterCombat.UseAbility)),
                    AccessTools.Method(typeof(CharacterCombat), nameof(CharacterCombat.TryPerformRandomAbility), new Type[0]),
                    AccessTools.Method(typeof(CharacterCombat), nameof(CharacterCombat.TryPerformRandomAbility), new Type[] { typeof(AbilitySO) }),
                    AccessTools.Method(typeof(EnemyCombat), nameof(EnemyCombat.UseAbility)),
                };
            }
        }

        [HarmonyPatch(typeof(CombatVisualizationController), nameof(CombatVisualizationController.ShowTargeting))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ModifyIntents(IEnumerable<CodeInstruction> instructions)
        {
            CodeInstruction prevInstruction = null;
            foreach (var instruction in instructions)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Ldarg_S && instruction.operand is byte b && b == 5 && prevInstruction != null && (prevInstruction.opcode == OpCodes.Ldarg_S || prevInstruction.opcode == OpCodes.Ldloc_S || prevInstruction.opcode == OpCodes.Stloc_S))
                {
                    // caster
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 5);
                    yield return new CodeInstruction(OpCodes.Call, gic);

                    // action
                    yield return new CodeInstruction(OpCodes.Ldnull);

                    // effect idx
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);

                    yield return new CodeInstruction(OpCodes.Call, ccc);
                }
                if (instruction.opcode == OpCodes.Ldarg_S && instruction.operand is byte b2 && b2 == 4)
                {
                    // caster
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 5);
                    yield return new CodeInstruction(OpCodes.Call, gic);

                    // action
                    yield return new CodeInstruction(OpCodes.Ldnull);

                    // effect idx
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);

                    yield return new CodeInstruction(OpCodes.Call, ccsi);
                }
                prevInstruction = instruction;
            }
        }

        public static EffectAction AddToList(EffectAction act)
        {
            act._caster.UnitExt().EffectsBeingPerformed.Add(act);
            return act;
        }

        public static bool RemoveFromList(bool prev, EffectAction act)
        {
            if (prev == false)
            {
                act._caster.UnitExt().EffectsBeingPerformed.Remove(act);
            }
            return prev;
        }

        public static IUnit GetEffectCharacter(int id, bool ischaracter)
        {
            if (ischaracter)
            {
                if (CombatManager.Instance._stats.Characters.TryGetValue(id, out var cc))
                {
                    return cc;
                }
            }
            else
            {
                if (CombatManager.Instance._stats.Enemies.TryGetValue(id, out var cc))
                {
                    return cc;
                }
            }
            return null;
        }

        public static bool ChangeCasterCharacter(bool orig, IUnit caster, EffectAction act, int effectIdx)
        {
            var boolRef = new BooleanReference(orig);
            CombatManager.Instance.PostNotification(CustomEvents.TARGETTING_UNIT_CHARACTER, caster, new TargetChangeInfo(null, boolRef, act, effectIdx));
            return boolRef.value;
        }

        public static int ChangeCasterSlotId(int orig, IUnit caster, EffectAction act, int effectIdx)
        {
            var intRef = new IntegerReference(orig);
            CombatManager.Instance.PostNotification(CustomEvents.TARGETTING_ORIGIN_SID, caster, new TargetChangeInfo(intRef, null, act, effectIdx));
            return intRef.value;
        }



        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.UseAbility))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> DoFuryAfterAbilityCharacter(IEnumerable<CodeInstruction> instructions)
        {
            var insertFuryAfterNextInstruction = false;
            var insertEndAbilityContextActionAfterNextInstruction = false;
            foreach (var instruction in instructions)
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
                    yield return new CodeInstruction(OpCodes.Call, patn);
                }
                if (instruction.opcode == OpCodes.Newobj && instruction.operand is MethodBase bas && bas.DeclaringType == typeof(StartAbilityCostAction))
                {
                    insertFuryAfterNextInstruction = true;
                }

                if (insertEndAbilityContextActionAfterNextInstruction)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Call, ieaca);
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
                    if (insertFuryAfterNextInstruction == 0)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldloc_0);
                        yield return new CodeInstruction(OpCodes.Ldarg_1);
                        yield return new CodeInstruction(OpCodes.Ldnull);
                        yield return new CodeInstruction(OpCodes.Call, patn);
                        yield return new CodeInstruction(OpCodes.Call, cmi);
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
                    yield return new CodeInstruction(OpCodes.Call, ieaca);
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

        public static void PreAbilityTriggeredNotification(CombatManager _, IUnit u, AbilitySO ab, int abid, FilledManaCost[] cost)
        {
            CombatManager.Instance.PostNotification(CustomEvents.PRE_ABILITY_USED, u, new AbilityContext(ab, abid, cost ?? new FilledManaCost[0]));
        }

        public static MethodInfo patn = AccessTools.Method(typeof(EventPatches), nameof(PreAbilityTriggeredNotification));
        public static MethodInfo cmi = AccessTools.PropertyGetter(typeof(CombatManager), nameof(CombatManager.Instance));
        public static MethodInfo ieaca = AccessTools.Method(typeof(EventPatches), nameof(InsertEndAbilityContextAction));
        public static MethodInfo atl = AccessTools.Method(typeof(EventPatches), nameof(AddToList));
        public static MethodInfo rfl = AccessTools.Method(typeof(EventPatches), nameof(RemoveFromList));
        public static MethodInfo ccc = AccessTools.Method(typeof(EventPatches), nameof(ChangeCasterCharacter));
        public static MethodInfo ccsi = AccessTools.Method(typeof(EventPatches), nameof(ChangeCasterSlotId));
        public static MethodInfo gic = AccessTools.Method(typeof(EventPatches), nameof(GetEffectCharacter));
        public static MethodInfo mod = AccessTools.Method(typeof(EventPatches), nameof(ModifyWrong));
        public static MethodInfo lcwmGet = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.LastCalculatedWrongMana));
    }

    public class TargetChangeInfo
    {
        public IntegerReference SlotIDRef;
        public BooleanReference IsUnitCharacterRef;

        public EffectAction Action;
        public int EffectIDX;

        public TargetChangeInfo(IntegerReference slotid, BooleanReference ischaracter, EffectAction action, int effectidx)
        {
            SlotIDRef = slotid;
            IsUnitCharacterRef = ischaracter;

            Action = action;
            EffectIDX = effectidx;
        }
    }

    public class AbilityContext
    {
        public AbilitySO ability;
        public int abilityId;
        public FilledManaCost[] abilityCost;

        public AbilityContext(AbilitySO ab, int id, FilledManaCost[] cost)
        {
            ability = ab;
            abilityId = id;
            abilityCost = cost;
        }
    }
}
