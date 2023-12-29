using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class TargetSwapStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => false;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(ApplyTargetSwap, TargetShiftPassive.EVENT_NAME_UNIT_CHARACTER, caller);
        }

        public void ApplyTargetSwap(object sender, object args)
        {
            if (args is TargetChangeInfo info && (info.Action == null || (sender is IUnit u && u.UnitExt().EffectsBeingPerformed.Contains(info.Action))))
            {
                var valid = true;
                if(info.Action != null)
                {
                    var effectInfo = info.Action._effects[info.EffectIDX];
                    var effect = effectInfo.effect;
                    valid = (effect is not AddPassiveEffect ape || ape._passiveToAdd.type != PassiveAbilityTypes.Fleeting) && (effect is not DamageEffect de || effectInfo.entryVariable < 500) && effect is not DirectDeathEffect and not FleeTargetEffect;
                }
                if (valid)
                {
                    info.IsUnitCharacterRef.value = !info.IsUnitCharacterRef.value;
                }
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(ApplyTargetSwap, TargetShiftPassive.EVENT_NAME_UNIT_CHARACTER, caller);
        }
    }
}
