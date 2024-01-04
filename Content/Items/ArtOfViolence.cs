using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class ArtOfViolence
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("The Art of Violence", "\"The world is your canvas, so take up your brush and paint it <color=#ff0000>R E D</color>\"", "On combat start, change all enemies' health colors to grey and give them Leaky and Red Core as passives.", "ArtOfViolence", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnCombatStart;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<ChangeToRandomHealthColorEffect>(x => x._healthColors = new ManaColorSO[] { Pigments.Grey }),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllEnemies
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddPassiveEffect>(x => x._passiveToAdd = Passives.Leaky()),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllEnemies
                },
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddPassiveEffect>(x => x._passiveToAdd = CustomPassives.RedHealth),
                    entryVariable = 0,
                    targets = TargettingLibrary.AllEnemies
                }
            };
        }
    }
}
