using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Converter
    {
        public static void Init()
        {
            var healswapper = new GenericItem<SwapSidesWearable>("Converter", "", "Healing abilities now damage the opposing targets. Attacking abilities now heal the opposing targets.\nConstricting and Fire-inflicting attacks now Shield opposing spaces. Shielding attacks inflict Fire on the opposing spaces.", "Placeholder", ItemPools.Treasure);
            healswapper.AddItem();
        }
    }
}
