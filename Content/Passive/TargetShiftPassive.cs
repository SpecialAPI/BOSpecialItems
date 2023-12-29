using System;
using System.Linq;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    [HarmonyPatch]
    public class TargetShiftPassive : BasePassiveAbilitySO
    {
        public const string EVENT_NAME_SID = "TargettingOriginSlotId";
        public const string EVENT_NAME_UNIT_CHARACTER = "TargettingUnitCharacter";

        public int shift;
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager.Instance.AddObserver(ApplyTargetShift, EVENT_NAME_SID, unit);
        }

        public void ApplyTargetShift(object sender, object args)
        {
            if(args is TargetChangeInfo info && (info.Action == null || (sender is IUnit u && u.UnitExt().EffectsBeingPerformed.Contains(info.Action))))
            {
                info.SlotIDRef.value += shift;
            }
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager.Instance.RemoveObserver(ApplyTargetShift, EVENT_NAME_SID, unit);
        }

        public override void TriggerPassive(object sender, object args)
        {
        }

        #region patches
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
            if(prev == false)
            {
                act._caster.UnitExt().EffectsBeingPerformed.Remove(act);
            }
            return prev;
        }

        public static IUnit GetEffectCharacter(int id, bool ischaracter)
        {
            if (ischaracter)
            {
                if(CombatManager.Instance._stats.Characters.TryGetValue(id, out var cc))
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
            CombatManager.Instance.PostNotification(EVENT_NAME_UNIT_CHARACTER, caster, new TargetChangeInfo(null, boolRef, act, effectIdx));
            return boolRef.value;
        }

        public static int ChangeCasterSlotId(int orig, IUnit caster, EffectAction act, int effectIdx)
        {
            var intRef = new IntegerReference(orig);
            CombatManager.Instance.PostNotification(EVENT_NAME_SID, caster, new TargetChangeInfo(intRef, null, act, effectIdx));
            return intRef.value;
        }

        public static MethodInfo atl = AccessTools.Method(typeof(TargetShiftPassive), nameof(AddToList));
        public static MethodInfo rfl = AccessTools.Method(typeof(TargetShiftPassive), nameof(RemoveFromList));
        public static MethodInfo ccc = AccessTools.Method(typeof(TargetShiftPassive), nameof(ChangeCasterCharacter));
        public static MethodInfo ccsi = AccessTools.Method(typeof(TargetShiftPassive), nameof(ChangeCasterSlotId));
        public static MethodInfo gic = AccessTools.Method(typeof(TargetShiftPassive), nameof(GetEffectCharacter));
        #endregion
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
}
