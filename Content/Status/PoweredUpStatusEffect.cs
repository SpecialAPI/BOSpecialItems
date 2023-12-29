using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class PoweredUpStatusEffect : GenericStatusEffect
    {
        public IUnit guy;
        public int Restrict;
        public int Duration;

        public override bool IsPositive => false;

        public override int Restrictor
        {
            get => Restrict;
            set
            {
                var oldRes = Restrict;
                Restrict = value;
                if (value != oldRes && guy != null && guy is CharacterCombat cc)
                {
                    cc.SetUpDefaultAbilities(true);
                }
            }
        }

        public override int StatusContent
        {
            get => Duration;
            set
            {
                var oldDur = Duration;
                Duration = value;
                if (value != oldDur && guy != null && guy is CharacterCombat cc)
                {
                    cc.SetUpDefaultAbilities(true);
                }
            }
        }

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(OnWillDamageTriggered, TriggerCalls.OnWillApplyDamage.ToString(), caller);
            guy = caller as IUnit;
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
            }
        }

        public void OnWillDamageTriggered(object sender, object args)
        {
            if (guy is EnemyCombat)
            {
                (args as DamageDealtValueChangeException).AddModifier(new PercentageValueModifier(true, 25 * (StatusContent + Restrictor), true));
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(OnWillDamageTriggered, TriggerCalls.OnWillApplyDamage.ToString(), caller);
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
            }
        }
    }
}
