using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class TickFuryAction : CombatAction
    {
        public IUnit unit;
        public FuryStatusEffect fury;

        public TickFuryAction(IUnit u, FuryStatusEffect f)
        {
            unit = u;
            fury = f;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            fury.EffectTick(unit, null);
            yield return null;
        }
    }
}
