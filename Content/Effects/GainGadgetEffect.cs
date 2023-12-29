using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class GainGadgetEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            foreach(var t in targets)
            {
                if(t.HasUnit && t.Unit.HasUsableItem)
                {
                    var g = GadgetDB.TryGetGadgetForItem(t.Unit.HeldItem.name);
                    if(g != null)
                    {
                        caster.AddExtraAbility(g);
                        exitAmount++;
                        if (g.ability.name.Contains("ShowCurrencyOnObtain"))
                        {
                            CombatManager.Instance.AddUIAction(new ShowCombatCurrencyEffectUIAction(true));
                        }
                    }
                }
            }
            return exitAmount > 0;
        }
    }
}
