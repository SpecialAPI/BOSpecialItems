using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class Conditions
    {
        public static EffectConditionSO Previous(int previousAmount = 1, bool wasSuccessful = true)
        {
            return CreateScriptable<PreviousEffectCondition>(x => { x.previousAmount = previousAmount; x.wasSuccessful = wasSuccessful; });
        }

        public static EffectConditionSO MultiPrevious(params (int previousAmount, bool wasSuccessful)[] previouses)
        {
            return CreateScriptable<MultiPreviousEffectCondition>(x => { x.previousAmount = previouses.Select(x => x.previousAmount).ToArray(); x.wasSuccessful = previouses.Select(x => x.wasSuccessful).ToArray(); });
        }

        public static EffectConditionSO Chance(int chance)
        {
            return CreateScriptable<PercentageEffectCondition>(x => x.percentage = chance);
        }
    }
}
