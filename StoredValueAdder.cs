using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    [HarmonyPatch]
    public static class StoredValueAdder
    {
        private static Dictionary<UnitStoredValueNames, StoredValueInfo> AddedStoredValues = new();

        public static void AddStoredValue(string name, StoredValueInfo info)
        {
            AddedStoredValues[StoredValue(name)] = info;
        }

        public static UnitStoredValueNames StoredValue(string name)
        {
            return ExtendEnum<UnitStoredValueNames>(name);
        }

        [HarmonyPatch(typeof(TooltipTextHandlerSO), nameof(TooltipTextHandlerSO.ProcessStoredValue))]
        [HarmonyPostfix]
        private static void AddStoredValues(TooltipTextHandlerSO __instance, ref string __result, UnitStoredValueNames storedValue, int value)
        {
            if (string.IsNullOrEmpty(__result) && AddedStoredValues.TryGetValue(storedValue, out var info) && info.MeetsCondition(__instance, value))
            {
                __result = info.FormatStoredValue(__instance, value);
            }
        }
    }

    public class StoredValueInfo
    {
        public bool stringIsDynamic;
        public string staticString;
        public StoredValueDelegate<string> dynamicString;
        public ColorType colorType;
        public Color customColor = Color.white;
        public StoredValueDelegate<Color> dynamicColor;
        public StoredValueCondition condition;
        public StoredValueDelegate<bool> customCondition;

        public string FormatStoredValue(TooltipTextHandlerSO handler, int value)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(GetColor(handler, value))}>{GetString(handler, value)}</color>";
        }

        public string GetString(TooltipTextHandlerSO handler, int value)
        {
            return stringIsDynamic ? dynamicString(value, handler) : string.Format(staticString, value);
        }

        public Color GetColor(TooltipTextHandlerSO handler, int value)
        {
            return colorType switch
            {
                ColorType.Positive => handler._positiveSTColor,
                ColorType.Negative => handler._negativeSTColor,
                ColorType.Rare => handler._rareSTColor,
                ColorType.Dynamic => dynamicColor(value, handler),
                _ => customColor
            };
        }

        public bool MeetsCondition(TooltipTextHandlerSO handler, int value)
        {
            return condition switch
            {
                StoredValueCondition.Negative => value < 0,
                StoredValueCondition.Positive => value > 0,
                StoredValueCondition.Custom => customCondition(value, handler),
                _ => true
            };
        }

        public enum ColorType
        {
            Positive,
            Negative,
            Rare,
            Custom,
            Dynamic
        }

        public enum StoredValueCondition
        {
            None,
            Positive,
            Negative,
            Custom
        }

        public delegate TResult StoredValueDelegate<TResult>(int value, TooltipTextHandlerSO handler);
    }
}
