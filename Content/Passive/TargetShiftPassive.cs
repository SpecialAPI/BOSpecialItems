using System;
using System.Linq;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public class TargetShiftPassive : BasePassiveAbilitySO
    {
        public int shift;
        public override bool IsPassiveImmediate => false;

        public override bool DoesPassiveTrigger => false;

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager.Instance.AddObserver(ApplyTargetShift, CustomEvents.TARGETTING_ORIGIN_SID, unit);
        }

        public void ApplyTargetShift(object sender, object args)
        {
            if(args is TargetChangeInfo info && (info.Action == null || (sender is IUnit u && u.UnitExt().EffectsBeingPerformed.Contains(info.Action))))
            {
                info.SlotIDRef.value += shift;
            }
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager.Instance.RemoveObserver(ApplyTargetShift, CustomEvents.TARGETTING_ORIGIN_SID, unit);
        }

        public override void TriggerPassive(object sender, object args)
        {
        }
    }
}
