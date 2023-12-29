using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class TickFuryAction : CombatAction
    {
        public IUnit unit;

        public TickFuryAction(IUnit u)
        {
            unit = u;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            CombatManager.Instance.PostNotification("BOSpecialItems_FuryTriggered", unit, null);
            yield return null;
        }
    }
}
