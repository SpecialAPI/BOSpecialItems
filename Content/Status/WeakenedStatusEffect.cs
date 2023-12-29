using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    [HarmonyPatch]
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
            guy = caller as IUnit;
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
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
            if (guy != null && guy is CharacterCombat cc)
            {
                cc.SetUpDefaultAbilities(true);
            }
        }

        public static int ModifyAbilityRank(int current, CharacterCombat cc)
        {
            var weak = cc.StatusEffects.Find(x => x.EffectType == GetEffectType<WeakenedStatusEffect>());
            var strong = cc.StatusEffects.Find(x => x.EffectType == GetEffectType<PoweredUpStatusEffect>());
            return cc.Character.ClampRank(current - (weak != null ? weak.StatusContent + weak.Restrictor : 0) + (strong != null ? strong.StatusContent + strong.Restrictor : 0));
        }
    }
}
