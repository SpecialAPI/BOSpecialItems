using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Survivorship
    {
        public static void Init()
        {
            var survivorship = new GenericItem<PerformEffectWearable>("Survivorship", "\"Yet you stand\"", "On combat start, apply 1 Survive to this party member.", "Survivorship", ItemPools.Treasure, 0);
            survivorship.item.effects = new EffectInfo[]
            {
                new()
                {
                    effect = CreateScriptable<ApplySurviveEffect>(),
                    condition = null,
                    entryVariable = 1,
                    targets = TargettingLibrary.ThisSlot
                }
            };
            survivorship.item.triggerOn = TriggerCalls.OnCombatStart;
            survivorship.item.doesItemPopUp = true;
            survivorship.AddItem();
            survivorship.item.AttachGadget(GadgetDB.GetGadget("Fake Death"));
        }
    }
}
