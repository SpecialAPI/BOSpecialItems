using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public class WidewakPassive : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => true;

        public override bool DoesPassiveTrigger => true;

        public float damageDealtMultPerSize;

        public override void OnPassiveConnected(IUnit unit)
        {
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }

        public override void TriggerPassive(object sender, object args)
        {
            if(sender is not IUnit owner)
            {
                return;
            }

            if(args is DamageReceivedValueChangeException receive)
            {
                receive.AddModifier(new WidewakDamageReceivedModifier(owner));
            }
            else if(args is DamageDealtValueChangeException deal)
            {
                deal.AddModifier(new WidewakDamageDealtModifier(owner, damageDealtMultPerSize));
            }
        }
    }

    public class WidewakDamageReceivedModifier(IUnit owner) : IntValueModifier(72)
    {
        public IUnit owner = owner;

        public override int Modify(int value)
        {
            return Mathf.Max(1, Mathf.RoundToInt((float)value / owner.Size));
        }
    }

    public class WidewakDamageDealtModifier(IUnit owner, float multPerSize) : IntValueModifier(72)
    {
        public IUnit owner = owner;
        public float multPerSize = multPerSize;

        public override int Modify(int value)
        {
            return value + Mathf.Max(0, Mathf.RoundToInt(value * multPerSize * (owner.Size - 1)));
        }
    }
}
