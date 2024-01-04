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

            var potential = NewItem<PerformEffectWearable>("Potential", "\"There is potential\"", $"This party member is 1 level higher than they would be otherwise.\nOn combat start, inflict {weaken} Weakened to this party member.", "Potential", ItemPools.Treasure, 0);
            potential.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<RankChange_Wearable_SMS>(x =>
                {
                    x._rankAdditive = 1;
                })
            };
            potential.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplyWeakenedEffect>(),
                    condition = null,
                    entryVariable = weaken,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            potential.triggerOn = TriggerCalls.OnCombatStart;
            potential.doesItemPopUp = true;
            potential.AttachGadget(GadgetDB.GetGadget("Dumb Down"));
        }
    }
}
