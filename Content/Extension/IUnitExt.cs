using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Extension
{
    public interface IUnitExt
    {
        IUnit BaseUnit { get; }
        List<ManaColorSO> HealthColors { get; }
        int CurrentHealthColor { get; set; }
        bool PerformingAbility { get; set; }
        AbilitySO AbilityCurrentlyBeingPerformed { get; set; }
        List<EffectAction> EffectsBeingPerformed { get; }
        int AbilityCurrentlyBeingPerformedId { get; set; }
        void AddHealthColor(ManaColorSO color);
    }
}
