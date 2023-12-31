using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class ForceGeneratePigmentColorEffect : EffectSO
	{
		public bool usePreviousExitValue;
		public ManaColorSO pigmentColor;

		public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
		{
			if (usePreviousExitValue)
			{
				entryVariable *= PreviousExitValue;
			}
			exitAmount = entryVariable;
			CombatManager.Instance.ProcessImmediateAction(new ForceAddPigmentAction(pigmentColor, entryVariable, caster.IsUnitCharacter, caster.ID));
			return true;
		}
	}

	public class ForceAddPigmentAction : IImmediateAction
    {
		public ManaColorSO pigment;
		public int amount;
		public bool isGeneratorCharacter;
		public int id;

		public ForceAddPigmentAction(ManaColorSO pigment, int amount, bool isGeneratorCharacter, int id)
		{
			this.pigment = pigment;
			this.amount = amount;
			this.isGeneratorCharacter = isGeneratorCharacter;
			this.id = id;
		}

		public void Execute(CombatStats stats)
		{
			if (pigment != null)
			{
				stats.MainManaBar.AddManaAmount(pigment, amount, stats.GenerateUnitJumpInformation(id, isGeneratorCharacter));
			}
		}
	}
}
