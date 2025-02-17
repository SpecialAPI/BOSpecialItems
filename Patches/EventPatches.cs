using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class EventPatches
    {
        #region Modify Wrong Pigment
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
        #endregion

        #region Targetting
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
        #endregion

        #region Misc Setters
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
        #endregion

        #region Ability Usage
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
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
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
                        yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
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

        public static void PreAbilityTriggeredNotification(CombatManager _, IUnit u, ref AbilitySO ab, int abid, FilledManaCost[] cost)
        {
            var context = new AbilityContext(ab, abid, cost ?? new FilledManaCost[0]);
            CombatManager.Instance.PostNotification(CustomEvents.MODIFY_USED_ABILITY, u, context);

            if(context.ability != ab)
            {
                CombatManager.Instance.AddUIAction(new ShowAttackInformationUIAction(u.ID, u.IsUnitCharacter, context.ability.GetAbilityLocData().text));
            }

            ab = context.ability;

            CombatManager.Instance.PostNotification(CustomEvents.PRE_ABILITY_USED, u, new AbilityContext(ab, abid, cost ?? new FilledManaCost[0]));
        }
        #endregion

        #region Pigment Producing
        [HarmonyPatch(typeof(AddManaToManaBarAction), nameof(AddManaToManaBarAction.Execute))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ModifyPigmentAmount(IEnumerable<CodeInstruction> instructions)
        {
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (instruction.LoadsField(amt))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Call, mpa);
                }
            }
        }

        public static int ModPigmentAmount(int current, AddManaToManaBarAction action, CombatStats stats)
        {
            IUnit generator = null;
            if (action._isGeneratorCharacter)
            {
                if(stats.Characters.TryGetValue(action._id, out var cc))
                {
                    generator = cc;
                }
            }
            else
            {
                if(stats.Enemies.TryGetValue(action._id, out var ec))
                {
                    generator = ec;
                }
            }

            var context = new PigmentGenerationContext()
            {
                amount = current,
                generator = generator
            };

            stats.DoForEachUnit(x => CombatManager.Instance.PostNotification(CustomEvents.MODIFY_PIGMENT_PRODUCED, x, context));

            return context.amount;
        }
        #endregion

        #region Status Effects
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.ApplyStatusEffect))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.ApplyStatusEffect))]
        [HarmonyILManipulator]
        public static void InsertToApplyEffect(ILContext ctx)
        {
            var cursor = new ILCursor(ctx);

            if(cursor.JumpToNext(x => x.Calls(pn), 2))
            {
                cursor.Emit(MmCodes.Ldarg_0);
                cursor.Emit(MmCodes.Ldarg_1);
                cursor.Emit(MmCodes.Ldarg_2);
                cursor.Emit(MmCodes.Call, pfan);

                if (cursor.JumpToNext(x => x.Calls(pn)))
                {
                    cursor.Emit(MmCodes.Ldarg_0);
                    cursor.Emit(MmCodes.Ldarg_1);
                    cursor.Emit(MmCodes.Ldarg_2);
                    cursor.Emit(MmCodes.Call, pan);

                    cursor.Emit(MmCodes.Ldarg_0);
                    cursor.Emit(MmCodes.Ldarg_1);
                    cursor.Emit(MmCodes.Ldarg_2);
                    cursor.Emit(MmCodes.Call, pin);
                }
            }
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.IncreaseStatusEffects))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.IncreaseStatusEffects))]
        [HarmonyILManipulator]
        public static void InsertToIncreaseEffects(ILContext ctx)
        {
            var cursor = new ILCursor(ctx);

            if (cursor.JumpToNext(x => x.Calls(pn)))
            {
                cursor.Emit(MmCodes.Ldarg_0);
                cursor.Emit(MmCodes.Ldloc_2);
                cursor.Emit(MmCodes.Ldloc_2);
                cursor.Emit(MmCodes.Callvirt, sc);
                cursor.Emit(MmCodes.Call, pin);
            }
        }

        public static void PostFirstAppliedNotif(IUnit whoWasThisAppliedTo, IStatusEffect effect, int amount)
        {
            var cm = CombatManager.Instance;

            cm._stats.DoForEachUnit(x => cm.PostNotification(CustomEvents.STATUS_EFFECT_FIRST_APPLIED, x, new TargettedStatusEffectApplication(whoWasThisAppliedTo, effect, amount)));
        }

        public static void PostAppliedNotif(IUnit whoWasThisAppliedTo, IStatusEffect effect, int amount)
        {
            var cm = CombatManager.Instance;

            cm._stats.DoForEachUnit(x => cm.PostNotification(CustomEvents.STATUS_EFFECT_APPLIED, x, new TargettedStatusEffectApplication(whoWasThisAppliedTo, effect, amount)));
        }

        public static void PostIncreasedNotif(IUnit whoWasThisAppliedTo, IStatusEffect effect, int amount)
        {
            var cm = CombatManager.Instance;

            cm._stats.DoForEachUnit(x => cm.PostNotification(CustomEvents.STATUS_EFFECT_INCREASED, x, new TargettedStatusEffectApplication(whoWasThisAppliedTo, effect, amount)));
        }
        #endregion

        #region Damage

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.WillApplyDamage))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.WillApplyDamage))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddStuff(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(gmv))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Call, context);
                }
                yield return instruction;
            }
        }

        private static DamageDealtValueChangeException AddContextTrigger(DamageDealtValueChangeException ex, IUnit attacker, IUnit target)
        {
            CombatManager.Instance.PostNotification(CustomEvents.WILL_APPLY_DAMAGE_CONTEXT, attacker, new WillApplyDamgeContext(ex, target));
            return ex;
        }
        #endregion

        #region Hell
        public static MethodInfo patn = AccessTools.Method(typeof(EventPatches), nameof(PreAbilityTriggeredNotification));
        public static MethodInfo ieaca = AccessTools.Method(typeof(EventPatches), nameof(InsertEndAbilityContextAction));
        public static MethodInfo atl = AccessTools.Method(typeof(EventPatches), nameof(AddToList));
        public static MethodInfo rfl = AccessTools.Method(typeof(EventPatches), nameof(RemoveFromList));
        public static MethodInfo ccc = AccessTools.Method(typeof(EventPatches), nameof(ChangeCasterCharacter));
        public static MethodInfo ccsi = AccessTools.Method(typeof(EventPatches), nameof(ChangeCasterSlotId));
        public static MethodInfo gic = AccessTools.Method(typeof(EventPatches), nameof(GetEffectCharacter));
        public static MethodInfo mod = AccessTools.Method(typeof(EventPatches), nameof(ModifyWrong));
        public static MethodInfo mpa = AccessTools.Method(typeof(EventPatches), nameof(ModPigmentAmount));
        public static MethodInfo pfan = AccessTools.Method(typeof(EventPatches), nameof(PostFirstAppliedNotif));
        public static MethodInfo pan = AccessTools.Method(typeof(EventPatches), nameof(PostAppliedNotif));
        public static MethodInfo pin = AccessTools.Method(typeof(EventPatches), nameof(PostIncreasedNotif));
        public static MethodInfo context = AccessTools.Method(typeof(EventPatches), nameof(AddContextTrigger));

        public static MethodInfo cmi = AccessTools.PropertyGetter(typeof(CombatManager), nameof(CombatManager.Instance));
        public static MethodInfo lcwmGet = AccessTools.PropertyGetter(typeof(CharacterCombat), nameof(CharacterCombat.LastCalculatedWrongMana));
        public static MethodInfo sc = AccessTools.PropertyGetter(typeof(IStatusEffect), nameof(IStatusEffect.StatusContent));

        public static FieldInfo amt = AccessTools.Field(typeof(AddManaToManaBarAction), nameof(AddManaToManaBarAction._amount));

        public static MethodInfo pn = AccessTools.Method(typeof(CombatManager), nameof(CombatManager.PostNotification));

        public static MethodInfo gmv = AccessTools.Method(typeof(DamageDealtValueChangeException), nameof(DamageDealtValueChangeException.GetModifiedValue));
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

    public class PigmentGenerationContext : ITargettedNotificationInfo
    {
        public IUnit generator;
        public int amount;

        public IUnit Target => generator;
    }

    public class TargettedStatusEffectApplication(IUnit guy, IStatusEffect statusEffect, int amountToApply) : ITargettedNotificationInfo
    {
        public IUnit unit = guy;
        public IStatusEffect statusEffect = statusEffect;
        public int amountToApply = amountToApply;

        public IUnit Target => unit;
    }

    public interface ITargettedNotificationInfo
    {
        public IUnit Target { get; }
    }

    public class WillApplyDamgeContext(DamageDealtValueChangeException ex, IUnit t) : ITargettedNotificationInfo
    {
        public DamageDealtValueChangeException exception = ex;
        public IUnit target = t;

        public IUnit Target => target;
    }
}
