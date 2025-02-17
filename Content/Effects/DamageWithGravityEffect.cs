using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class DamageWithGravityEffect : EffectSO
    {
        public AddGravityUIAction.ColliderType collider;
        public Vector3 launchForce;
        public float randomForce;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach (var t in targets)
            {
                if (t.HasUnit)
                {
                    CombatManager.Instance.AddUIAction(new AddGravityUIAction(t.Unit.ID, collider, launchForce, randomForce));

                    exitAmount += t.Unit.Damage(caster.WillApplyDamage(entryVariable, t.Unit), caster, DeathType.Basic, areTargetSlots ? (t.SlotID - t.Unit.SlotID) : (-1), true, true, false).damageAmount;
                }
            }

            if (exitAmount > 0)
            {
                caster.DidApplyDamage(exitAmount);
            }

            return exitAmount > 0;
        }
    }
}
