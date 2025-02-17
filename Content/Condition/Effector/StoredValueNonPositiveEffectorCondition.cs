using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Condition.Effector
{
    public class StoredValueNonPositiveEffectorCondition : EffectorConditionSO
    {
        public UnitStoredValueNames value;

        public override bool MeetCondition(IEffectorChecks effector, object args)
        {
            if(effector is IUnit u)
            {
                return u.GetStoredValue(value) <= 0;
            }
            return false;
        }
    }
}
