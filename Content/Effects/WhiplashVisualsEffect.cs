using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class WhiplashVisualsEffect : EffectSO
    {
        public AttackVisualsSO visuals;
        public bool prioritizeLeft;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            var farthestEnemies = new List<IUnit>();
            var farthestDistance = -1;

            foreach (var target in targets)
            {
                if (target.HasUnit)
                {
                    int dist;

                    var left = target.Unit.LastSlotId() < caster.SlotID;
                    var right = target.Unit.SlotID > caster.LastSlotId();

                    if (left && !right)
                    {
                        dist = Mathf.Abs(target.Unit.SlotID - caster.SlotID);
                    }
                    else if (right && !left)
                    {
                        dist = Mathf.Abs(target.Unit.LastSlotId() - caster.LastSlotId());
                    }
                    else
                    {
                        continue;
                    }

                    if (dist > farthestDistance)
                    {
                        farthestEnemies.Clear();
                        farthestDistance = dist;
                    }
                    if (dist >= farthestDistance)
                    {
                        farthestEnemies.Add(target.Unit);
                    }
                }
            }

            IUnit targetEnemy = null;

            if (farthestEnemies.Count <= 0)
            {
                return true;
            }
            else if (farthestEnemies.Count >= 1)
            {
                foreach (var target in farthestEnemies)
                {
                    if (target.LastSlotId() < caster.SlotID == prioritizeLeft)
                    {
                        targetEnemy = target;
                    }
                }
            }

            if (targetEnemy == null)
            {
                targetEnemy = farthestEnemies[0];
            }

            if (targetEnemy != null)
            {
                var targetSlot = stats.combatSlots.GetGenericAllySlotTarget(targetEnemy.SlotID, targetEnemy.IsUnitCharacter);

                if (targetSlot != null)
                {
                    CombatManager.Instance.AddUIAction(new PlayAbilityAnimationAtSpecificSlotsAction(visuals, [targetSlot], caster, false));
                }
            }

            return true;
        }
    }

    public class PlayAbilityAnimationAtSpecificSlotsAction(AttackVisualsSO visuals, TargetSlotInfo[] targets, IUnit caster, bool areTargetsSlots = false) : CombatAction
    {
        public AttackVisualsSO visuals = visuals;
        public TargetSlotInfo[] targets = targets;
        public IUnit caster = caster;
        public bool areTargetsSlots = areTargetsSlots;

        public override IEnumerator Execute(CombatStats stats)
        {
            yield return stats.combatUI.PlayAbilityAnimation(visuals, targets, areTargetsSlots);
        }
    }
}
