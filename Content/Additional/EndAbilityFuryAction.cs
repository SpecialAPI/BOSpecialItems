using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
	public class EndAbilityFuryAction : CombatAction
	{
		public int unitID;

		public bool isUnitCharacter;

		public EndAbilityFuryAction(int unitID, bool isUnitCharacter)
		{
			this.unitID = unitID;
			this.isUnitCharacter = isUnitCharacter;
		}

		public override IEnumerator Execute(CombatStats stats)
		{
			if (isUnitCharacter)
			{
				CharacterCombat characterCombat = stats.TryGetCharacterOnField(unitID);
				if (characterCombat != null)
				{
					if (characterCombat.IsAlive)
					{
						characterCombat.AbilityHasFinished();
					}
				}
			}
			else
			{
				EnemyCombat enemyCombat = stats.TryGetEnemyOnField(unitID);
				if (enemyCombat != null && enemyCombat.IsAlive)
				{
					enemyCombat.AbilityHasFinished();
				}
			}
			foreach (CharacterCombat value in stats.CharactersOnField.Values)
			{
				value.AnyAbilityHasFinished();
			}
			foreach (EnemyCombat value2 in stats.EnemiesOnField.Values)
			{
				value2.AnyAbilityHasFinished();
			}
			yield return null;
		}
	}
}
