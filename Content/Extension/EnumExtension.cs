using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BOSpecialItems.Content.Extension
{
    [HarmonyPatch]
    public static class EnumExtension // i would like to thank past me for figuring all of this out so present me doesnt have to
    {
		private readonly static Dictionary<Type, Dictionary<string, int>> extendedEnums = new();
		private readonly static Dictionary<object, Dictionary<string, ulong>> enumsToAdd = new();

		public static T ExtendEnum<T>(/*string guid, */string name) where T : Enum
		{
			return (T)ExtendEnum(/*guid, */name, typeof(T));
		}

		public static string RemoveUnacceptableCharactersForEnum(this string str)
		{
			if (str == null)
			{
				return null;
			}
			return str.Replace("\"", "").Replace("\\", "").Replace(" ", "").Replace("-", "_").Replace("\n", "");
		}

		public static object ExtendEnum(/*string guid, */string name, Type t) //good thing this isnt an api so i can remove the guid arg altogether :gold_per_spin:
		{
			if (!t.IsEnum)
			{
				return 0;
			}
			var guid = Plugin.GUID;
			name = name.RemoveUnacceptableCharactersForEnum().Replace(".", "");
			guid = guid.RemoveUnacceptableCharactersForEnum();
			var guidandname = $"{guid}.{name}";
			//var values = valuesForEnum.Where(x => x.Key.guid == ginfo.guid && x.Key.info == ginfo.info); //bad thing this isnt an api so i cant use this shortcut here
			object value = null;
            try
            {
				value = Enum.Parse(t, guidandname); // this may look jank, but it actually is jank
            }
            catch { }
            if (value != null)
            {
				return value;
			}
			else
			{
				var max = 0;
				try
				{
					max = Enum.GetValues(t).Cast<int>().Max();
				}
				catch
				{
				}
				int val;
				if (t.IsDefined(typeof(FlagsAttribute), false))
				{
					val = max == 0 ? 1 : max * 2;
				}
				else
				{
					val = max + 1;
				}
				Dictionary<string, int> valuesForEnum;
				if (!extendedEnums.ContainsKey(t))
				{
					valuesForEnum = new();
					extendedEnums.Add(t, valuesForEnum);
				}
				else
				{
					valuesForEnum = extendedEnums[t];
				}
				valuesForEnum.Add(guidandname, val);


				Dictionary<string, ulong> valuesForEnum2;
				if (!enumsToAdd.ContainsKey(t))
				{
					valuesForEnum2 = new();
					enumsToAdd.Add(t, valuesForEnum2);
				}
				else
				{
					valuesForEnum2 = enumsToAdd[t];
				}
				valuesForEnum2.Add(guidandname, (ulong)val);
				return val;
			}
		}

		[HarmonyPatch(typeof(Enum), "GetCachedValuesAndNames")]
		[HarmonyPostfix]
		public static void AddStuff(object __result, object enumType, bool getNames)
        {
			if(enumsToAdd.ContainsKey(enumType) && enumsToAdd[enumType].Count > 0)
            {
				var names = ((string[])valuesandnamesNames.GetValue(__result)).Concat(enumsToAdd[enumType].Keys).ToArray();
				var values = ((ulong[])valuesandnamesValues.GetValue(__result)).Concat(enumsToAdd[enumType].Values).ToArray();
				Array.Sort(values, names, Comparer<ulong>.Default);
				valuesandnamesNames.SetValue(__result, names);
				valuesandnamesValues.SetValue(__result, values);
				enumsToAdd[enumType].Clear();
			}
        }

		public static FieldInfo valuesandnamesNames = AccessTools.Field(AccessTools.TypeByName("System.Enum+ValuesAndNames"), "Names");
		public static FieldInfo valuesandnamesValues = AccessTools.Field(AccessTools.TypeByName("System.Enum+ValuesAndNames"), "Values");
	}
}
