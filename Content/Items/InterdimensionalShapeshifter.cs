using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class InterdimensionalShapeshifter
    {
        public static void Init()
        {
            var item = NewItem<BasicWearable>("Inter-Dimensional Shape-Shifter", "\"Some see it as a pawn\"", "Adds Shape-Shifter to this party member as a passive.", "InterdimensionalShapeshifter", ItemPools.Treasure);
            item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x => x._extraPassiveAbility = CustomPassives.Shapeshifter)
            };
        }
    }
}
