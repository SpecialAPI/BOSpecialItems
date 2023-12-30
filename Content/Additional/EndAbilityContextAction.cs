using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class EndAbilityContextAction : CombatAction
    {
		public readonly IUnit Unit;
        public readonly int AbilityID;
        public readonly AbilitySO Ability;
        public readonly FilledManaCost[] Cost;

		public EndAbilityContextAction(IUnit u, int abilityid, AbilitySO ability, FilledManaCost[] cost)
        {
			Unit = u;
			AbilityID = abilityid;
			Ability = ability;
			Cost = cost;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
			if (Unit.IsUnitCharacter)
			{
				var characterCombat = stats.TryGetCharacterOnField(Unit.ID);
				if (characterCombat != null && characterCombat.IsAlive)
				{
					characterCombat.CalculateAbilityCostsDamage(AbilityID, Cost);
					CombatManager.Instance.PostNotification(CustomEvents.ABILITY_USED_CONTEXT, characterCombat, new AbilityContext(Ability, AbilityID, Cost));
					characterCombat.LastCalculatedWrongMana = 0;
				}
			}
			else
			{
				var enemyCombat = stats.TryGetEnemyOnField(Unit.ID);
				if (enemyCombat != null && enemyCombat.IsAlive)
				{
					CombatManager.Instance.PostNotification(CustomEvents.ABILITY_USED_CONTEXT, enemyCombat, this);
				}
			}
			yield return null;
        }
    }
}
