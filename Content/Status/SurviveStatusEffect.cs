using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class SurviveStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(Survive, TriggerCalls.OnBeingDamaged.ToString(), caller);
        }

        public void Survive(object sender, object args)
        {
            (args as DamageReceivedValueChangeException).AddModifier(new SurviveIntValueModifier(1, this, sender as IStatusEffector, sender as IUnit));
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(Survive, TriggerCalls.OnBeingDamaged.ToString(), caller);
        }
    }

    public class SurviveIntValueModifier : IntValueModifier
    {
        public int surviveHealth;
        public SurviveStatusEffect surviveEffect;
        public IStatusEffector effector;
        public IUnit unit;

        public SurviveIntValueModifier(int survivingHealth, SurviveStatusEffect survive, IStatusEffector effector, IUnit unit) : base(999999)
        {
            surviveHealth = survivingHealth;
            surviveEffect = survive;
            this.effector = effector;
            this.unit = unit;
        }

        public override int Modify(int value)
        {
            if(unit.CurrentHealth - value < surviveHealth)
            {
                surviveEffect.ReduceContent(effector, 1);
                return Mathf.Max(0, unit.CurrentHealth - surviveHealth);
            }
            return value;
        }
    }
}
