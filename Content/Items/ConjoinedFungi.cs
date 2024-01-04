using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class ConjoinedFungi
    {
        public static void Init()
        {
            var percentageIncrease = 20;

            var item = NewItem<PerformEffectWearable>("Conjoined Fungi", "\"<size=50%>We are not welcome elsewhere.</size>\"", $"On combat start, increase the health of all enemies by {percentageIncrease}%, then merge all enemies of the same type.", "ConjoinedFungi", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnCombatStart;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeMaxHealthFilledEffect>(x => x.entryAsPercentage = true),
                    entryVariable = percentageIncrease,
                    targets = TargettingLibrary.AllEnemies
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<MergeEnemiesEffect>(),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllEnemies
                }
            };
        }
    }
}
