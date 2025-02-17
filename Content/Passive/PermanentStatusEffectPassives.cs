using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public class TargetSwappedPassiveAbility : PermanentStatusEffectPassiveAbility<TargetSwapStatusEffect>
    {
    }

    public class FrenziedPassiveAbility : PermanentStatusEffectPassiveAbility<BerserkStatusEffect>
    {
    }

    public class FuriousPassiveAbility : PermanentStatusEffectPassiveAbility<FuryStatusEffect>
    {
    }

    public class WeakPassiveAbility : PermanentStatusEffectPassiveAbility<WeakenedStatusEffect>
    {
    }

    public class EnergizedPassiveAbility : PermanentStatusEffectPassiveAbility<PoweredUpStatusEffect>
    {
    }
}
