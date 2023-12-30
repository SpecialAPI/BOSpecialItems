using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class WeakenedStatusEffect : GenericStatusEffect
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
            CombatManager.Instance.AddObserver(ModifyAbilityRank, CustomEvents.MODIFY_ABILITIES_RANK, caller);
            guy = caller as IUnit;
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
            }
        }

        public void ModifyAbilityRank(object sender, object args)
        {
            if(args is IntegerReference intref)
            {
                intref.value -= (Duration + Restrict);
            }
        }

        public void OnWillDamageTriggered(object sender, object args)
        {
            if(guy is EnemyCombat)
            {
                (args as DamageDealtValueChangeException).AddModifier(new PercentageValueModifier(true, Mathf.Max(Mathf.RoundToInt((Mathf.Pow(0.85f, StatusContent + Restrictor) - 1) * -100f), 0), false));
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(OnWillDamageTriggered, TriggerCalls.OnWillApplyDamage.ToString(), caller);
            CombatManager.Instance.RemoveObserver(ModifyAbilityRank, CustomEvents.MODIFY_ABILITIES_RANK, caller);
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
            }
        }
    }
}
