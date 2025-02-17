using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    [HarmonyPatch]
    public class ManaColorSOAdvanced : ManaColorSO
    {
        public bool requiresPigment = true;
        public bool noPigmentCountsAsDamage = true;

        public bool DealsCostDamageAdvanced(ManaColorSO stuffInCost)
        {
            if (stuffInCost == null || stuffInCost.Equals(null))
            {
                return noPigmentCountsAsDamage;
            }
            var shared = pigmentType & stuffInCost.pigmentType;
            if (dealsCostDamage && stuffInCost.dealsCostDamage)
            {
                return shared == PigmentType.None;
            }
            return false;
        }

        public void Inherit(ManaColorSO orig)
        {
            pigmentType = orig.pigmentType;
            manaSprite = orig.manaSprite;
            manaUsedSprite = orig.manaUsedSprite;
            manaCostSprite = orig.manaCostSprite;
            manaCostSelectedSprite = orig.manaCostSelectedSprite;
            healthSprite = orig.healthSprite;
            healthColor = orig.healthColor;
            canGenerateMana = orig.canGenerateMana;
            dealsCostDamage = orig.dealsCostDamage;
            manaSoundEvent = orig.manaSoundEvent;

            if(orig is ManaColorSOAdvanced adv)
            {
                requiresPigment = adv.requiresPigment;
                noPigmentCountsAsDamage = adv.noPigmentCountsAsDamage;
            }
        }

        [HarmonyPatch(typeof(ManaColorSO), nameof(DealsCostDamage))]
        [HarmonyPrefix]
        public static bool OverrideForAdvancedMana(ManaColorSO __instance, ref bool __result, ManaColorSO otherMana)
        {
            if(__instance is ManaColorSOAdvanced adv)
            {
                __result = adv.DealsCostDamageAdvanced(otherMana);
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(AttackCostLayout), nameof(AttackCostLayout.CalculatePerformAttackButtonState))]
        [HarmonyILManipulator]
        public static void OverrideRequirePigment(ILContext ctx)
        {
            var cursor = new ILCursor(ctx);

            while(cursor.JumpToNext(x => x.Calls(dshm)))
            {
                cursor.Emit(MmCodes.Ldarg_0);
                cursor.Emit(MmCodes.Ldloc_0);

                cursor.Emit(MmCodes.Call, rpc);
            }
        }

        [HarmonyPatch(typeof(FilledManaCost), MethodType.Constructor, typeof(ManaSlotUIInfo))]
        [HarmonyPrefix]
        public static bool MaybeDontDoAnything(ManaSlotUIInfo info)
        {
            return info != null;
        }

        [HarmonyPatch(typeof(CombatVisualizationController), nameof(CombatVisualizationController.CurrentCostArray), MethodType.Getter)]
        [HarmonyILManipulator]
        public static void DontIncludeNullCosts(ILContext ctx)
        {
            var cursor = new ILCursor(ctx);

            foreach(var m in cursor.MatchBefore(x => x.Calls(ta)))
            {
                cursor.Emit(MmCodes.Call, rnc);
            }
        }

        public static List<FilledManaCost> RemoveNullCosts(List<FilledManaCost> current)
        {
            current.RemoveAll(x => x.Mana == null);
            return current;
        }

        public static bool RequirePigmentChecks(bool current, AttackCostLayout l, int slotIndex)
        {
            if(slotIndex >= 0 && slotIndex < l.CurrentCost.Length && l.CurrentCost[slotIndex] is ManaColorSOAdvanced adv)
            {
                return current || !adv.requiresPigment;
            }

            return current;
        }

        public static MethodInfo dshm = AccessTools.PropertyGetter(typeof(ManaSlotLayout), nameof(ManaSlotLayout.DoesSlotHaveMana));
        public static MethodInfo rpc = AccessTools.Method(typeof(ManaColorSOAdvanced), nameof(RequirePigmentChecks));
        public static MethodInfo ta = AccessTools.Method(typeof(List<FilledManaCost>), nameof(List<FilledManaCost>.ToArray));
        public static MethodInfo rnc = AccessTools.Method(typeof(ManaColorSOAdvanced), nameof(RemoveNullCosts));
    }
}
