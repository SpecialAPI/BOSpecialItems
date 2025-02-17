using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AddGravityEffect : EffectSO
    {
        public AddGravityUIAction.ColliderType collider;
        public Vector3 launchForce;
        public float randomForce;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;

            foreach(var t in targets)
            {
                if (t.HasUnit && !t.Unit.IsUnitCharacter)
                {
                    CombatManager.Instance.AddUIAction(new AddGravityUIAction(t.Unit.ID, collider, launchForce, randomForce));
                    exitAmount++;
                }
            }

            return exitAmount > 0;
        }
    }

    public class AddGravityUIAction(int enemyid, AddGravityUIAction.ColliderType collider, Vector3 launchForce, float randomForce) : CombatAction
    {
        public int enemyId = enemyid;
        public ColliderType collider = collider;
        public Vector3 launchForce = launchForce;
        public float randomForce = randomForce;

        public override IEnumerator Execute(CombatStats stats)
        {
            if(stats.combatUI._enemiesInCombat.TryGetValue(enemyId, out var enemy) && enemy.FieldID >= 0)
            {
                var eif = stats.combatUI._enemyZone._enemies[enemy.FieldID].FieldEntity;

                var rb = eif.GetOrAddComponent<Rigidbody>();

                if(collider != ColliderType.None)
                {
                    Type keepType = collider switch
                    {
                        ColliderType.Cube => typeof(BoxCollider),
                        ColliderType.Sphere => typeof(SphereCollider),
                        ColliderType.Capsule => typeof(CapsuleCollider),
                        _ => null
                    };

                    if(keepType != null)
                    {
                        var comps = eif.GetComponents<Collider>();

                        foreach(var comp in comps)
                        {
                            if(comp.GetType() != keepType)
                            {
                                Object.Destroy(comp);
                            }
                        }

                        if(eif.GetComponent(keepType) == null)
                        {
                            eif.gameObject.AddComponent(keepType);
                        }
                    }
                }

                rb.AddForce(launchForce + randomForce * Random.insideUnitSphere);
            }

            yield break;
        }

        public enum ColliderType
        {
            None,
            Cube,
            Sphere,
            Capsule
        }
    }
}
