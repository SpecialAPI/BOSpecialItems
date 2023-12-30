using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class MergeEnemiesEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            if (targets.Length <= 1)
            {
                return false;
            }
            Dictionary<EnemySO, (int, int, int, List<EnemyCombat>)> mergeData = new();
            foreach (var target in targets)
            {
                if (target != null && target.HasUnit && target.Unit is EnemyCombat ec && ec.IsAlive && ec.CurrentHealth > 0)
                {
                    if (!mergeData.ContainsKey(ec.Enemy))
                    {
                        mergeData.Add(ec.Enemy, (0, 0, -1, new()));
                    }
                    if(mergeData.TryGetValue(ec.Enemy, out var dat) && !dat.Item4.Contains(ec))
                    {
                        dat.Item1 += ec.MaximumHealth;
                        dat.Item2 += ec.CurrentHealth;
                        dat.Item3 += ec.GetStoredValue(StoredValue("MergedCount")) + 1;
                        dat.Item4.Add(ec);
                    }
                }
            }
            var success = false;
            foreach(var kvp in mergeData)
            {
                var en = kvp.Key;
                var dat = kvp.Value;
                var currenthealth = dat.Item2;
                var maxhealth = dat.Item1;
                var extraabilities = dat.Item3;
                var targettedEnemies = dat.Item4;
                if (en != null && currenthealth > 0 && maxhealth > 0 && extraabilities >= 0 && targettedEnemies.Count > 1)
                {
                    foreach (var e in targettedEnemies)
                    {
                        e.DirectDeath(null, false);
                    }
                    CombatManager.Instance.AddSubAction(new TrySpawnMergedEnemyAction(en, true, maxhealth, currenthealth, extraabilities));
                    exitAmount += targettedEnemies.Count;
                    success = true;
                }
            }
            return success;
        }
    }

    public class TrySpawnMergedEnemyAction : CombatAction
    {
        public EnemySO enemy;
        public bool experience;
        public int healthMax;
        public int health;
        public int extraAbilities;

        public TrySpawnMergedEnemyAction(EnemySO enemy, bool givesExperience, int maxHealth, int currentHealth, int extraAbilities)
        {
            this.enemy = enemy;
            experience = givesExperience;
            healthMax = maxHealth;
            health = currentHealth;
            this.extraAbilities = extraAbilities;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            var slot = stats.GetRandomEnemySlot(enemy.size);
            if (slot != -1)
            {
                stats.AddNewEnemy(enemy, slot, experience, SpawnType.Basic);
                if(stats.combatSlots.EnemySlots[slot].Unit is EnemyCombat en)
                {
                    en.MaximumHealth = healthMax;
                    en.CurrentHealth = Math.Min(healthMax, health);
                    en.SetStoredValue(StoredValue("MergedCount"), extraAbilities);
                    en.AddPassiveAbility(CustomPassives.Merged);
                }
            }
            yield return null;
        }
    }
}
