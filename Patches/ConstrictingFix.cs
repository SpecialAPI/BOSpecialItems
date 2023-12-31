using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Patches
{
    [HarmonyPatch]
    public static class ConstrictingFix
    {
        [HarmonyPatch(typeof(ConstrictedConnectedAction), nameof(ConstrictedConnectedAction.Execute))]
        [HarmonyPostfix]
        public static IEnumerator DoNothingIfOwnerIsDead(IEnumerator orig, ConstrictedConnectedAction __instance)
        {
            var valid = false;
            if (__instance._isUnitCharacter)
            {
                var chars = CombatManager.Instance._stats.Characters;
                if (chars.ContainsKey(__instance._unitID))
                {
                    var u = chars[__instance._unitID];
                    valid = u.IsAlive && !u.IsBoxed && CombatManager.Instance._stats.TryGetCharacterOnField(u.ID) == u;
                }
            }
            else
            {
                var ens = CombatManager.Instance._stats.Enemies;
                if (ens.ContainsKey(__instance._unitID))
                {
                    var u = ens[__instance._unitID];
                    valid = u.IsAlive && !u.IsBoxed && CombatManager.Instance._stats.TryGetEnemyOnField(u.ID) == u;
                }
            }
            if (valid)
            {
                yield return orig;
            }
            else
            {
                yield break;
            }
        }

        [HarmonyPatch(typeof(ConstrictedDisconnectedAction), nameof(ConstrictedDisconnectedAction.Execute))]
        [HarmonyPostfix]
        public static IEnumerator DoNothingIfOwnerIsDead(IEnumerator orig, ConstrictedDisconnectedAction __instance)
        {
            yield return orig;
        }
    }
}
