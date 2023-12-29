using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Extension
{
    public class CharacterCombatExt : IUnitExt
    {
        public IUnit BaseUnit { get; }
        public List<ManaColorSO> HealthColors { get; }
        public int CurrentHealthColor { get; set; }
        public bool PerformingAbility { get; set; }
        public AbilitySO AbilityCurrentlyBeingPerformed { get; set; }
        public int AbilityCurrentlyBeingPerformedId { get; set; }
        public List<EffectAction> EffectsBeingPerformed { get; }
        public ManaColorSO[] CurrentAbilityFilledCosts;

        public CharacterCombatExt(IUnit u)
        {
            BaseUnit = u;
            HealthColors = new() { u.HealthColor };
            EffectsBeingPerformed = new();
        }

        public void AddHealthColor(ManaColorSO color)
        {
            HealthColors.Add(color);
            CombatManager.Instance._combatUI.TryUpdateCharacterIDInformation(BaseUnit.ID);
        }
    }
}
