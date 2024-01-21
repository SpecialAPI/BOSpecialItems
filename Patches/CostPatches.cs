using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class CostPatches
    {
        [HarmonyPatch(typeof(CombatVisualizationController), nameof(CombatVisualizationController.TrySetUpCostInformation))]
        [HarmonyPrefix]
        public static void AddNewSlots(CombatVisualizationController __instance, ManaColorSO[] slotCost)
        {
            if(__instance._characterCost != null && __instance._characterCost._costSlots != null && slotCost != null && slotCost.Length > __instance._costBarInfo.Length)
            {
                var toAdd = slotCost.Length - __instance._costBarInfo.Length;
                var newCostSlots = __instance._characterCost._costSlots.ToList();
                for(int i = 0; i < toAdd; i++)
                {
                    var stuff = Object.Instantiate(newCostSlots[0].gameObject, newCostSlots[0].transform.parent).GetComponent<ManaSlotLayout>();
                    stuff.SetManaSlotIDs(ManaBarType.Cost, newCostSlots.Count);
                    stuff.transform.SetAsFirstSibling();
                    newCostSlots.Add(stuff);
                }
                __instance._costBarInfo = [.. __instance._costBarInfo, .. new CostSlotUIInfo[toAdd].Select(x => new CostSlotUIInfo())];
                __instance._characterCost._costSlots = [.. newCostSlots];
                __instance._characterCost.CurrentCost = [.. __instance._characterCost.CurrentCost, .. new ManaColorSO[toAdd].Populate(null)];
            }
        } 

        [HarmonyPatch(typeof(AttackCostLayout), nameof(AttackCostLayout.SetSlotActivity))]
        [HarmonyPostfix]
        public static void MakeCostNotFuckedUp(AttackCostLayout __instance, int index, bool enabled)
        {
            if(index >= 6)
            {
                __instance._costSlots[index].gameObject.SetActive(enabled);
            }
        }
    }
}
