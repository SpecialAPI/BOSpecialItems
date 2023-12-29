using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    [HarmonyPatch]
    public static class IntentAdder
    {
        private readonly static Dictionary<IntentType, IntentInfo> intentsToAdd = new();
        public static IntentHandlerSO intentDB;

        [HarmonyPatch(typeof(IntentHandlerSO), nameof(IntentHandlerSO.Initialize))]
        [HarmonyPostfix]
        private static void AddIntents(IntentHandlerSO __instance)
        {
            if(intentDB == null)
            {
                intentDB = __instance;
                foreach(var kvp in intentsToAdd)
                {
                    intentDB._intentDB[kvp.Key] = kvp.Value;
                }
                intentsToAdd.Clear();
            }
        }

        public static void AddIntent(string name, IntentInfo info)
        {
            var realType = Intent(name);
            info._type = realType;
            if(intentDB != null)
            {
                intentDB._intentDB[realType] = info;
            }
            else
            {
                intentsToAdd[realType] = info;
            }
        }

        public static IntentType Intent(string name)
        {
            return ExtendEnum<IntentType>(name);
        }
    }
}
