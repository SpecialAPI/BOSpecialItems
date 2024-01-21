using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOSpecialItems.Content.Effects
{
    [HarmonyPatch]
    public class EquipRandomTreasureEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach (var t in targets)
            {
                if(t != null && t.HasUnit && itemPool != null && t.Unit is CharacterCombat cc)
                {
                    var notifs = NtfUtils.notifications._table;

                    List<Action<object, object>> oldPreCombatStart = null;
                    if(notifs.TryGetValue(TriggerCalls.OnBeforeCombatStart.ToString(), out var pcs) && pcs.TryGetValue(cc, out var pcs2) && pcs2 != null)
                    {
                        oldPreCombatStart = new(pcs2);
                    }
                    List<Action<object, object>> oldCombatStart = null;
                    if (notifs.TryGetValue(TriggerCalls.OnCombatStart.ToString(), out var cs) && cs.TryGetValue(cc, out var cs2) && cs2 != null)
                    {
                        oldCombatStart = new(cs2);
                    }
                    List<Action<object, object>> oldFirstTurnStart = null;
                    if (notifs.TryGetValue(TriggerCalls.OnFirstTurnStart.ToString(), out var fts) && fts.TryGetValue(cc, out var fts2) && fts2 != null)
                    {
                        oldFirstTurnStart = new(fts2);
                    }

                    if (cc.TrySetUpNewItem(GetTotallyRandomTreasure()))
                    {
                        exitAmount++;

                        if (notifs.TryGetValue(TriggerCalls.OnBeforeCombatStart.ToString(), out var pcs3) && pcs3.TryGetValue(cc, out var preCombatStart) && preCombatStart != null)
                        {
                            foreach(var call in preCombatStart)
                            {
                                if (oldPreCombatStart == null || !oldPreCombatStart.Contains(call))
                                {
                                    call?.Invoke(cc, null);
                                }
                            }
                        }
                        if (notifs.TryGetValue(TriggerCalls.OnCombatStart.ToString(), out var cs3) && cs3.TryGetValue(cc, out var combatStart) && combatStart != null)
                        {
                            foreach (var call in combatStart)
                            {
                                if (oldCombatStart == null || !oldCombatStart.Contains(call))
                                {
                                    call?.Invoke(cc, null);
                                }
                            }
                        }
                        if (notifs.TryGetValue(TriggerCalls.OnFirstTurnStart.ToString(), out var fts3) && fts3.TryGetValue(cc, out var firstTurnStart) && firstTurnStart != null)
                        {
                            foreach (var call in firstTurnStart)
                            {
                                if (oldFirstTurnStart == null || !oldFirstTurnStart.Contains(call))
                                {
                                    call?.Invoke(cc, null);
                                }
                            }
                        }
                    }
                }
            }
            return exitAmount > 0;
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.RemoveAndDisconnectAllItemExtraPassiveAbilities))]
        [HarmonyPrefix]
        public static void DontRemoveShapeshifter(CharacterCombat __instance)
        {
            __instance.ItemExtraPassives.RemoveAll(x => x.type == ExtendEnum<PassiveAbilityTypes>("Shapeshifter"));
        }
    }
}
