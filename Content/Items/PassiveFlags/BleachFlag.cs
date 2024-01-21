using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.PassiveFlags
{
    [HarmonyPatch]
    public class BleachFlag : WearableStaticFlagPassive
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.DefaultPassiveAbilityInitialization))]
        public static bool UseBleach(CharacterCombat __instance)
        {
            return !__instance.CharacterWearableModifiers.HasFlag<BleachFlag>();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.TrySetUpNewItem))]
        public static void GildedMirrorBleach(CharacterCombat __instance, bool __result)
        {
            if (__result && __instance.CharacterWearableModifiers.HasFlag<BleachFlag>())
            {
				if (__instance.ExternalPassives != null)
				{
					foreach (BasePassiveAbilitySO externalPassife in __instance.ExternalPassives)
					{
						if (__instance.PassiveAbilities.Contains(externalPassife))
						{
							__instance.PassiveAbilities.Remove(externalPassife);
							externalPassife.OnTriggerDettached(__instance);
							externalPassife.OnPassiveDisconnected(__instance);
						}
					}
				}
				else if (__instance.Character.passiveAbilities != null)
				{
					var passiveAbilities = __instance.Character.passiveAbilities;
					foreach (BasePassiveAbilitySO basePassiveAbilitySO in passiveAbilities)
					{
						if (__instance.PassiveAbilities.Contains(basePassiveAbilitySO))
						{
							__instance.PassiveAbilities.Remove(basePassiveAbilitySO);
							basePassiveAbilitySO.OnTriggerDettached(__instance);
							basePassiveAbilitySO.OnPassiveDisconnected(__instance);
						}
					}
				}
				if (__instance.ExtraPassives != null)
				{
					foreach (BasePassiveAbilitySO extraPassife in __instance.ExtraPassives)
					{
						if (__instance.PassiveAbilities.Contains(extraPassife))
						{
							__instance.PassiveAbilities.Remove(extraPassife);
							extraPassife.OnTriggerDettached(__instance);
							extraPassife.OnPassiveDisconnected(__instance);
						}
					}
				}
				if (__instance.ItemExtraPassives == null)
				{
					return;
				}
				foreach (BasePassiveAbilitySO itemExtraPassife in __instance.ItemExtraPassives)
				{
					if (__instance.PassiveAbilities.Contains(itemExtraPassife))
					{
						__instance.PassiveAbilities.Remove(itemExtraPassife);
						itemExtraPassife.OnTriggerDettached(__instance);
						itemExtraPassife.OnPassiveDisconnected(__instance);
					}
				}
			}
        }
    }
}
