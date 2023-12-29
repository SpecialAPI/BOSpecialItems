using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class EndAbilityContextAction : CombatAction
    {
		public const string NOTIFICATION_NAME = "OnAbilityUsedContext";

		public readonly IUnit Unit;
        public readonly int AbilityID;
        public readonly AbilitySO Ability;
        public readonly FilledManaCost[] Cost;
		public readonly bool DontMessWithWrongMana;

		public EndAbilityContextAction(IUnit u, int abilityid, AbilitySO ability, FilledManaCost[] cost, bool dontMessWithWrongMana = false)
        {
			Unit = u;
			AbilityID = abilityid;
			Ability = ability;
			Cost = cost;
			DontMessWithWrongMana = dontMessWithWrongMana;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
			if (Unit.IsUnitCharacter)
			{
				var characterCombat = stats.TryGetCharacterOnField(Unit.ID);
				if (characterCombat != null && characterCombat.IsAlive)
				{
                    if (!DontMessWithWrongMana)
					{
						characterCombat.CalculateAbilityCostsDamage(AbilityID, Cost);
					}
					CombatManager.Instance.PostNotification(NOTIFICATION_NAME, characterCombat, this);
                    if (!DontMessWithWrongMana)
					{
						characterCombat.LastCalculatedWrongMana = 0;
					}
				}
			}
			else
			{
				var enemyCombat = stats.TryGetEnemyOnField(Unit.ID);
				if (enemyCombat != null && enemyCombat.IsAlive)
				{
					CombatManager.Instance.PostNotification(NOTIFICATION_NAME, enemyCombat, this);
				}
			}
			yield return null;
        }
    }
}
