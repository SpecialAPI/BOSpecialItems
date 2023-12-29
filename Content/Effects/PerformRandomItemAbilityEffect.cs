using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class PerformRandomItemAbilityEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
			exitAmount = 0;
			var abs = GetRandomItemAbilities(entryVariable);
			foreach (var t in targets)
			{
				foreach (var ab in abs)
				{
					if (t.HasUnit && t.Unit.TryPerformRandomAbility(ab))
					{
						exitAmount++;
					}
				}
			}
			return exitAmount > 0;
		}

		public List<AbilitySO> GetRandomItemAbilities(int amount)
		{
			var abs = new List<AbilitySO>();
			var itemabs = new List<string>(itemAbilities);
			while (amount > 0 && itemabs.Count > 0)
			{
				var idx = Random.Range(0, itemabs.Count);
				var name = itemabs[idx];
				itemabs.RemoveAt(idx);
				var ab = LoadedAssetsHandler.GetCharacterAbility(name);
				if (ab != null)
				{
					abs.Add(ab);
					amount--;
				}
			}
			return abs;
		}

		public static List<string> itemAbilities = new()
		{
			"Slap_Big_A",
			"Slap_Snap_A",
			"Extra_SeamRipper_A",
			"Extra_OriginalSin_A",
			"Extra_CrownOfThorns_A",
			"Extra_BladedMalediction_A",
			"Extra_WormSkin_A",
			"Extra_Icarus_A",
			"Extra_InhumanUtterings_A",
			"Extra_Trout_A",
			"Extra_Sabbath_A",
			"Extra_BloodGamble_A",
			"Extra_Cannibalize_A",
			"Extra_Penance_A",
			"Extra_AlchemicalAbomination_A",
			"Extra_AbjectRejection_A"
		};
    }
}
