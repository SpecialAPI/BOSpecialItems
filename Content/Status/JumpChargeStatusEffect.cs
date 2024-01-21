using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public class JumpChargeStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;
        public override bool CanBeRemoved => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(RefreshMove, TriggerCalls.OnMoved.ToString(), caller);
        }

        public void RefreshMove(object sender, object args)
        {
            if (sender is IUnit unit && unit is IStatusEffector effector)
            {
                CombatManager.Instance.AddSubAction(new JumpChargeActivatedAction(unit, this, effector));
            }
        }

        public void Reduce(IStatusEffector effector)
        {
            if (StatusDurationCanBeReduced)
            {
                int duration = StatusContent;
                StatusContent = Mathf.Max(0, StatusContent - 1);
                if (!TryRemoveStatusEffect(effector) && duration != StatusContent)
                {
                    effector.StatusEffectValuesChanged(EffectType, StatusContent - duration);
                }
            }
        }

        public override void OnTriggerDettached(IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(RefreshMove, TriggerCalls.OnMoved.ToString(), caller);
        }
    }

    public class JumpChargeActivatedAction(IUnit u, JumpChargeStatusEffect se, IStatusEffector effector) : CombatAction
    {
        public IUnit unit = u;
        public JumpChargeStatusEffect jump = se;
        public IStatusEffector owner = effector;

        public override IEnumerator Execute(CombatStats stats)
        {
            jump.Reduce(owner);
            var targets = stats.combatSlots.GetFrontOpponentSlotTargets(unit.SlotID, unit.IsUnitCharacter);

            var chars = new List<(IUnit unit, bool swapRight)>();
            var enemies = new List<(IUnit, bool swapRight)>();

            foreach (var target in targets)
            {
                if (target.HasUnit)
                {
                    var u = target.Unit;
                    u.Damage(unit.WillApplyDamage(10, u), unit, DeathType.Basic, target.SlotID - u.SlotID, true, true, false);
                }
            }

            var left = stats.combatSlots.GetAllySlotTarget(unit.SlotID, -1, unit.IsUnitCharacter);
            var right = stats.combatSlots.GetAllySlotTarget(unit.SlotID, 1, unit.IsUnitCharacter);

            if (left != null && left.HasUnit)
            {
                chars.Add((left.Unit, false));
            }
            if(right != null && right.HasUnit)
            {
                chars.Add((right.Unit, true));
            }

            foreach ((var ch, var swapRight) in chars)
            {
                var num = swapRight ? 1 : -1;
                for (int i = 0; i < 4; i++)
                {
                    if (ch.SlotID + num < 0 || ch.SlotID + num >= stats.combatSlots.CharacterSlots.Length || !stats.combatSlots.SwapCharacters(ch.SlotID, ch.SlotID + num, isMandatory: true))
                    {
                        break;
                    }
                }
            }
            foreach ((var en, var swapRight) in enemies)
            {
                var num = swapRight ? en.Size : -1;
                for (int i = 0; i < 4; i++)
                {
                    if (!stats.combatSlots.CanEnemiesSwap(en.SlotID, en.SlotID + num, out var firstSlotSwap, out var secondSlotSwap) || !stats.combatSlots.SwapEnemies(en.SlotID, firstSlotSwap, en.SlotID + num, secondSlotSwap))
                    {
                        break;
                    }
                }
            }
            yield return null;
        }
    }
}
