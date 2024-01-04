using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class SilverMirror
    {
        public static void Init()
        {
            var mirror = NewItem<PerformEffectWearable>("Silvered Mirror", "\"Twisted reflections\"", "At the beginning of combat, gain an additional ability based on the held item of the left ally.", "SilverMirror", ItemPools.Treasure);
            mirror.triggerOn = TriggerCalls.OnFirstTurnStart;
            mirror.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<GainGadgetEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.Relative(true, -1)
                }
            };
            mirror.AttachGadget(GadgetDB.GetGadget("Infinity"));
        }
    }
}
