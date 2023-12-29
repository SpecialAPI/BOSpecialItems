using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Potential
    {
        public static void Init()
        {
            var weaken = 2;

            var potential = new GenericItem<PerformEffectWearable>("Potential", "\"There is potential\"", $"This party member is 1 level higher than they would be otherwise.\nOn combat start, inflict {weaken} Weakened to this party member.", "Potential", ItemPools.Treasure, 0);
            potential.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<RankChange_Wearable_SMS>(x =>
                {
                    x._rankAdditive = 1;
                })
            };
            potential.item.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplyWeakenedEffect>(),
                    condition = null,
                    entryVariable = weaken,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            potential.item.triggerOn = TriggerCalls.OnCombatStart;
            potential.item.doesItemPopUp = true;
            potential.AddItem();
            potential.item.AttachGadget(GadgetDB.GetGadget("Dumb Down"));
        }
    }
}
