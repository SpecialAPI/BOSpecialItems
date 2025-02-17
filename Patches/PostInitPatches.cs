using System;
using System.Collections.Generic;
using System.Text;
using static UnityEngine.GraphicsBuffer;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class PostInitPatches
    {
        public static void Init()
        {
            var t = AccessTools.AllTypes();

            foreach(var type in t)
            {
                if(type.IsSubclassOf(typeof(EffectSO)) && !type.IsAbstract && !type.IsInterface)
                {
                    var mthd = type.GetMethod(nameof(EffectSO.PerformEffect), BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(CombatStats), typeof(IUnit), typeof(TargetSlotInfo[]), typeof(bool), typeof(int), typeof(int).MakeByRefType() }, null);

                    if(mthd != null && !mthd.IsAbstract && mthd.IsDeclaredMember())
                    {
                        Plugin.harmony.Patch(mthd, ilmanipulator: new(hp));
                    }
                }
            }
        }

        public static void HealingPatch(ILContext ctx)
        {
            var cursor = new ILCursor(ctx);

            while(cursor.TryGotoNext(x => x.Calls(h)))
            {
                cursor.Emit(MmCodes.Ldarg_2);
                cursor.Emit(MmCodes.Call, sh);
                if(cursor.JumpToNext(x => x.Calls(h)))
                {
                    cursor.Emit(MmCodes.Call, rh);
                }
            }
        }

        public static bool SetHealer(bool _, IUnit caster)
        {
            healer = caster;
            return _;
        }

        public static int ResetHealer(int _)
        {
            healer = null;
            return _;
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.Heal))]
        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.Heal))]
        [HarmonyPrefix]
        public static void ChangeHeal(IUnit __instance, ref int amount, HealType healType, bool directHeal)
        {
            if(directHeal && healer != null)
            {
                var ex = new HealedUnitValueChangeException(amount, __instance, healType);
                CombatManager.Instance.PostNotification(CustomEvents.WILL_HEAL_UNIT, healer, ex);
                amount = ex.GetModifiedValue();
            }
            healer = null; // for extra safety
        }

        public static MethodInfo hp = AccessTools.Method(typeof(PostInitPatches), nameof(HealingPatch));
        public static MethodInfo sh = AccessTools.Method(typeof(PostInitPatches), nameof(SetHealer));
        public static MethodInfo rh = AccessTools.Method(typeof(PostInitPatches), nameof(ResetHealer));

        public static MethodInfo h = AccessTools.Method(typeof(IUnit), nameof(IUnit.Heal));

        public static IUnit healer;
    }

    public class HealedUnitValueChangeException(int amount, IUnit target, HealType ht) : BaseException(true)
    {
        public readonly int amount = amount;
        public readonly IUnit target = target;
        public readonly HealType healType = ht;
        private List<IntValueModifier> modifiers;

        public void AddModifier(IntValueModifier m)
        {
            if (modifiers == null)
            {
                modifiers = new List<IntValueModifier>();
            }
            modifiers.Add(m);
        }

        public int GetModifiedValue()
        {
            var amt = amount;
            if (modifiers == null)
            {
                return amt;
            }
            modifiers.Sort((x, y) => x.sortOrder.CompareTo(y.sortOrder));
            foreach(var mod in modifiers)
            {
                amt = mod.Modify(amt);
            }
            return amt;
        }
    }
}
