using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public class AbsorbantPlatingPassiveAbility : BasePassiveAbilitySO
    {
        public override bool IsPassiveImmediate => true;
        public override bool DoesPassiveTrigger => true;
        public int percentage = 100;

        public override void OnPassiveConnected(IUnit unit)
        {
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }

        public override void CustomOnTriggerAttached(IPassiveEffector caller)
        {
            base.CustomOnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(TryTriggerPassive, CustomEvents.MODIFY_PIGMENT_PRODUCED, caller);
        }

        public override void CustomOnTriggerDettached(IPassiveEffector caller)
        {
            base.CustomOnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(TryTriggerPassive, CustomEvents.MODIFY_PIGMENT_PRODUCED, caller);
        }

        public override void TriggerPassive(object sender, object args)
        {
            if(args is PigmentGenerationContext context && sender is IUnit unit && IsOpposing(unit, context.generator) && (context.generator.HealthColor.pigmentType & unit.HealthColor.pigmentType) != PigmentType.None)
            {
                var healAmount = Mathf.Max(Mathf.Min(unit.MaximumHealth - unit.CurrentHealth, context.amount), 0);

                if (healAmount > 0)
                {
                    context.amount -= healAmount;

                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, GetPassiveLocData().text, passiveIcon));
                    unit.Heal(Mathf.Max(Mathf.FloorToInt(healAmount * percentage / 100f), Mathf.Min(1, healAmount), 0), HealType.Heal, false);
                }
            }
        }
    }
}
