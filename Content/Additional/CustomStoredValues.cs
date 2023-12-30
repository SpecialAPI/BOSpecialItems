using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public static class CustomStoredValues
    {
        public static void Init()
        {
            AddStoredValue("MergedCount", new()
            {
                colorType = StoredValueInfo.ColorType.Positive,
                condition = StoredValueInfo.StoredValueCondition.Positive,
                staticString = "Merged Enemies: {0}"
            });
        }
    }
}
