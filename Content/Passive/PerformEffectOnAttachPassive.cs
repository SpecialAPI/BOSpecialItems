using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public class PerformEffectOnAttachPassive : BasePassiveAbilitySO
    {
        public bool immediate;
        public EffectInfo[] effects;

        public override bool IsPassiveImmediate => immediate;

        public override bool DoesPassiveTrigger => false;

        public override void OnPassiveConnected(IUnit unit)
        {
            if (IsPassiveImmediate)
            {
                if (doesPassiveTriggerInformationPanel)
                {
                    CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(unit.ID, unit.IsUnitCharacter, GetPassiveLocData().text, passiveIcon));
                }
                TriggerPassive(unit, null);
            }
            else
            {
                CombatManager.Instance.AddSubAction(new PerformPassiveAction(this, unit, null));
            }
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
        }

        public override void TriggerPassive(object sender, object args)
        {
            if (IsPassiveImmediate)
            {
                CombatManager.Instance.ProcessImmediateAction(new ImmediateEffectAction(effects, (IUnit)sender));
            }
            else
            {
                CombatManager.Instance.AddSubAction(new EffectAction(effects, (IUnit)sender));
            }
        }
    }
}
