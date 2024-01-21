using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BOSpecialItems.Content.Status
{
    public class RamChargeStatusEffect : GenericStatusEffect
    {
        public override bool IsPositive => true;
        public override bool CanBeRemoved => true;

        public override void OnTriggerAttached(IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(RefreshMove, TriggerCalls.OnMoved.ToString(), caller);
        }

        public void RefreshMove(object sender, object args)
        {
            if (sender is IUnit unit && unit is IStatusEffector effector && args is IntegerReference intref)
            {
                CombatManager.Instance.AddSubAction(new RamChargeActivatedAction(unit, intref.value, this, effector));
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

    public class RamChargeActivatedAction(IUnit u, int oldsid, RamChargeStatusEffect se, IStatusEffector effector) : CombatAction
    {
        public IUnit unit = u;
        public int oldSlotId = oldsid;
        public RamChargeStatusEffect ram = se;
        public IStatusEffector owner = effector;

        public override IEnumerator Execute(CombatStats stats)
        {
            ram.Reduce(owner);
            var targets = stats.combatSlots.GetFrontOpponentSlotTargets(unit.SlotID, unit.IsUnitCharacter);

            var chars = new List<IUnit>();
            var enemies = new List<IUnit>();

            var swapRight = oldSlotId < unit.SlotID;

            foreach (var target in targets)
            {
                if (target.HasUnit)
                {
                    var u = target.Unit;
                    if (u.IsUnitCharacter && !chars.Contains(u))
                    {
                        chars.Add(u);
                    }
                    else if (!u.IsUnitCharacter && !enemies.Contains(u))
                    {
                        enemies.Add(u);
                    }
                    u.Damage(unit.WillApplyDamage(10, u), unit, DeathType.Basic, target.SlotID - u.SlotID, true, true, false);
                }
            }

            foreach (var ch in chars)
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
            foreach (var en in enemies)
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
