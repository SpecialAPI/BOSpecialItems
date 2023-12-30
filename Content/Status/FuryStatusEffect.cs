using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class FuryStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            base.OnTriggerAttached(caller);
            CombatManager.Instance.AddObserver(AdditionalFuryActions, CustomEvents.PRE_ABILITY_USED, caller);
        }

        public void AdditionalFuryActions(object sender, object args)
        {
            if(args is AbilityContext context && sender is IUnit u)
            {
                var furyStacks = StatusContent + Restrictor;
                var ab = context.ability;
                if (furyStacks > 0 && !ab.name.Contains("NoFuryRepeat"))
                {
                    //CombatManager.Instance.AddUIAction(new PlayStatusEffectSoundAndWaitUIAction("event:/FuryApply", 1f));
                    for (int i = 0; i < furyStacks; i++)
                    {
                        CombatManager.Instance.AddRootAction(new PlayAbilityAnimationAction(ab.visuals, ab.animationTarget, u));
                        CombatManager.Instance.AddRootAction(new EffectAction(ab.effects, u));
                        //CombatManager.Instance.AddRootAction(new EndAbilityFuryAction(u.ID, u.IsUnitCharacter));
                        //CombatManager.Instance.AddRootAction(new EndAbilityContextAction(u, abid, ab, cost ?? new FilledManaCost[0], true));
                        if (!ab.name.Contains("NoFuryTick"))
                        {
                            CombatManager.Instance.AddRootAction(new TickFuryAction(u, this));
                        }
                    }
                }
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            base.OnTriggerDettached(caller);
            CombatManager.Instance.RemoveObserver(AdditionalFuryActions, CustomEvents.PRE_ABILITY_USED, caller);
        }
    }
}
