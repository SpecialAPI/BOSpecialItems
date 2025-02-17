using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items.Wearables.TriggerEffects
{
    public class ConverterTriggerEffect : TriggerEffect
    {
        public static UnitStoredValueNames triggering = ExtendEnum<UnitStoredValueNames>("ConverterTriggering");

        public float healToDamageModifier = 1f;
        public float damageToHealModifier = 1f;

        public override void DoEffect(IUnit sender, object args, EffectsAndTriggerBase effectsAndTrigger)
        {
            if (args is HealedUnitValueChangeException heal)
            {
                sender.SetStoredValue(triggering, 1);

                var dmg = heal.target.Damage(sender.WillApplyDamage(Mathf.Max(Mathf.RoundToInt(heal.amount * healToDamageModifier), 0), heal.target), sender, DeathType.None, -1, true, true, false, DamageType.None).damageAmount;
                heal.AddModifier(new SetToValueValueModifier(0));

                if(dmg > 0)
                {
                    sender.DidApplyDamage(dmg);
                }

                sender.SetStoredValue(triggering, 0);
            }
            else if (args is WillApplyDamgeContext damage)
            {
                sender.SetStoredValue(triggering, 1);

                damage.target.Heal(Mathf.Max(Mathf.RoundToInt(damage.exception.amount * damageToHealModifier), 0), HealType.Heal, true);
                damage.exception.AddModifier(new SetToValueValueModifier(0));

                sender.SetStoredValue(triggering, 0);
            }
        }
    }
}
