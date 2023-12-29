using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOSpecialItems.Content.Effects
{
    public class TryProduceLuckyPigmentEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			if (!stats.MainManaBar.IsManaBarFull)
			{
				var luckyManaPercentage = stats.LuckyManaPercentage;
				var luckyManaAmount = stats.LuckyManaAmount;
				var luckyBlueMaxPercentage = Utils.luckyBlueMaxPercentage;
				if (Random.Range(0, luckyBlueMaxPercentage) < luckyManaPercentage)
				{
					CombatManager.Instance.AddUIAction(new AddLuckyManaUIAction());
					var jumpInfo = new JumpAnimationInformation(startsOnUI: true, stats.combatUI.LuckyManaPosition);
					exitAmount += stats.MainManaBar.AddManaAmount(stats.LuckyManaColorOptions[stats.SelectedLuckyColor], luckyManaAmount, jumpInfo);
				}
			}
			return exitAmount > 0;
		}
    }
}
