using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddInfestationEffect : EffectSO
    {
		public BasePassiveAbilitySO infestation;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			var passiveLocData = infestation.GetPassiveLocData();
			foreach (var t in targets)
            {
                if (t.HasUnit)
                {
                    if (!t.Unit.ContainsPassiveAbility(infestation.type))
                    {
						t.Unit.AddPassiveAbility(infestation);
                    }
                    else
                    {
						t.Unit.SetStoredValue(UnitStoredValueNames.InfestationPA, t.Unit.GetStoredValue(UnitStoredValueNames.InfestationPA) + 1);
                    }
					CombatManager.Instance.AddUIAction(new ShowPassiveInformationUIAction(t.Unit.ID, t.Unit.IsUnitCharacter, passiveLocData.text, infestation.passiveIcon));
                    exitAmount++;
				}
            }
			return exitAmount > 0;
		}
    }
}
