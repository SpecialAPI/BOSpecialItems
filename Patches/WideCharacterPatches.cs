using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class WideCharacterPatches
    {
        [HarmonyPatch(typeof(CharacterCombat), MethodType.Constructor, typeof(int), typeof(int), typeof(CharacterSO), typeof(bool), typeof(int), typeof(int), typeof(int), typeof(int), typeof(BaseWearableSO), typeof(WearableStaticModifiers), typeof(bool), typeof(string), typeof(bool))]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.TransformCharacter))]
        [HarmonyPostfix]
        public static void IncreaseSize(CharacterCombat __instance)
        {
            if(__instance.Character.name == "Widewak_CH")
            {
                __instance._size = 2;
            }
        }

        [HarmonyPatch(typeof(SlotsCombat), nameof(SlotsCombat.AddCharacterToSlot))]
        [HarmonyPostfix]
        public static void AddToMoreSlots(SlotsCombat __instance, IUnit character, int slotID)
        {
            if(character.Size > 1)
            {
                for(int i = 1; i < character.Size; i++)
                {
                    __instance.CharacterSlots[slotID + i].SetUnit(character);
                }
            }
        }

        [HarmonyPatch(typeof(SlotsCombat), nameof(SlotsCombat.GetFrontOpponentSlotTargets))]
        [HarmonyPostfix]
        public static void AddMoreSlots(SlotsCombat __instance, List<TargetSlotInfo> __result, int originSlotID, bool isOriginCharacter)
        {
            if (isOriginCharacter)
            {
                var size = 1;
                if (originSlotID >= 0 && originSlotID < __instance.CharacterSlots.Length && __instance.CharacterSlots[originSlotID].HasUnit)
                {
                    size = __instance.CharacterSlots[originSlotID].Unit.Size;
                }
                if(size > 1)
                {
                    for (int i = 1; i < size; i++)
                    {
                        var enemyTargetSlot = __instance.GetEnemyTargetSlot(originSlotID, i);
                        if (enemyTargetSlot != null && !__result.Contains(enemyTargetSlot))
                        {
                            __result.Add(enemyTargetSlot);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SlotsCombat), nameof(SlotsCombat.GetAllSelfSlots))]
        [HarmonyPostfix]
        public static void AddMoreSlotsSelf(SlotsCombat __instance, List<TargetSlotInfo> __result, int originSlotID, bool isOriginCharacter)
        {
            if (isOriginCharacter)
            {
                var size = 1;
                if (originSlotID >= 0 && originSlotID < __instance.CharacterSlots.Length && __instance.CharacterSlots[originSlotID].HasUnit)
                {
                    size = __instance.CharacterSlots[originSlotID].Unit.Size;
                }
                if (size > 1)
                {
                    for (int i = 1; i < size; i++)
                    {
                        var enemyTargetSlot = __instance.GetCharacterTargetSlot(originSlotID, i);
                        if (enemyTargetSlot != null && !__result.Contains(enemyTargetSlot))
                        {
                            __result.Add(enemyTargetSlot);
                        }
                    }
                }
            }
        }

        public static CombatManager SwapMoreStuff(CombatManager combatman, SlotsCombat slots, int firstslot, int secondslot, bool mandatory, ref int[] characterIds, ref int[] newids)
        {
            var firstguy = slots.CharacterSlots[secondslot].Unit;
            var secondguy = slots.CharacterSlots[firstslot].Unit;

            var firstguysize = 1;
            var secondguysize = 1;

            var firstguysid = firstslot;
            var secondguysid = secondslot;

            if(firstguy != null)
            {
                firstguysize = firstguy.Size;
                firstguysid = firstguy.SlotID;
            }
            if (secondguy != null)
            {
                secondguysize = secondguy.Size;
                secondguysid = secondguy.SlotID;
            }

            for(int i = 0; i < firstguysize; i++)
            {
                slots.CharacterSlots[firstguysid + i].SetUnit(null);
            }
            for (int i = 0; i < secondguysize; i++)
            {
                slots.CharacterSlots[secondguysid + i].SetUnit(null);
            }

            int maxsize;
            int maxsizesid;

            int othersize;
            int othersizeid;

            if(firstguysize > secondguysize)
            {
                maxsize = firstguysize;
                maxsizesid = firstguysid;

                othersize = secondguysize;
                othersizeid = secondguysid;
            }
            else
            {
                maxsize = secondguysize;
                maxsizesid = secondguysid;

                othersize = firstguysize;
                othersizeid = firstguysid;
            }

            if(maxsize != othersize)
            {
                for(int i = 0; i < maxsize; i++)
                {
                    var movefrom = maxsizesid + i;
                    var moveto = othersizeid + i;

                    if(slots.CharacterSlots[moveto].HasUnit)
                    {
                        slots.CharacterSlots[movefrom].SetUnit(slots.CharacterSlots[moveto].Unit);

                        if(Array.IndexOf(characterIds, slots.CharacterSlots[movefrom].Unit.ID) < 0)
                        {
                            characterIds = characterIds.AddToArray(slots.CharacterSlots[movefrom].Unit.ID);
                            newids = newids.AddToArray(movefrom);
                        }
                    }
                }
            }

            for (int i = 0; i < firstguysize; i++)
            {
                slots.CharacterSlots[secondguysid + i].SetUnit(firstguy);
            }
            for (int i = 0; i < secondguysize; i++)
            {
                slots.CharacterSlots[firstguysid + i].SetUnit(secondguy);
            }

            return combatman;
        }

        [HarmonyPatch(typeof(SlotsCombat), nameof(SlotsCombat.SwapCharacters))]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> OverrideSwap(IEnumerable<CodeInstruction> instructions)
        {
            foreach(var instruction in instructions)
            {
                yield return instruction;
                if (instruction.Calls(EventPatches.cmi))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldarg_2);
                    yield return new CodeInstruction(OpCodes.Ldarg_3);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 1);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 2);
                    yield return new CodeInstruction(OpCodes.Call, sms);
                }
            }
        }

        public static MethodInfo sms = AccessTools.Method(typeof(WideCharacterPatches), nameof(SwapMoreStuff));
    }
}
