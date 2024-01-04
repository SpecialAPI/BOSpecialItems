using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Survivorship
    {
        public static void Init()
        {
            var survivorship = NewItem<PerformEffectWearable>("Survivorship", "\"Yet you stand\"", "On combat start, apply 1 Survive to this party member.", "Survivorship", ItemPools.Treasure, 0);
            survivorship.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplySurviveEffect>(),
                    condition = null,
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            survivorship.triggerOn = TriggerCalls.OnCombatStart;
            survivorship.doesItemPopUp = true;
            survivorship.AttachGadget(GadgetDB.GetGadget("Fake Death"));
        }
    }
}
