using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables.TriggerEffects
{
    public class SetHealingToOneTriggerEffect : TriggerEffect
    {
        public override void DoEffect(IUnit sender, object args, EffectsAndTriggerBase effectsAndTrigger)
        {
            if(args is HealedUnitValueChangeException ex)
            {
                ex.AddModifier(new SetToValueValueModifier(1));
            }
        }
    }

    public class SetToValueValueModifier(int val) : IntValueModifier(10)
    {
        public int val = val;

        public override int Modify(int value)
        {
            return val;
        }
    }
}
