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
            Dictionary<EnemySO, List<EnemyCombat>> mergeData = new();
            foreach (var target in targets)
            {
                if (target != null && target.HasUnit && target.Unit is EnemyCombat ec && ec.IsAlive && ec.CurrentHealth > 0)
                {
                    if (!mergeData.ContainsKey(ec.Enemy))
                    {
                        mergeData.Add(ec.Enemy,new());
                    }
                    if(mergeData.TryGetValue(ec.Enemy, out var dat) && !dat.Contains(ec))
                    {
                        dat.Add(ec);
                    }
                }
            }
            foreach(var kvp in mergeData)
            {
                var en = kvp.Key;
                var targettedEnemies = kvp.Value;
                if (en != null && targettedEnemies.Count > 1)
                {
                    var currenthealth = 0;
                    var maxhealth = 0;
                    var extraabilities = -1;
                    foreach (var e in targettedEnemies)
                    {
                        currenthealth += e.CurrentHealth;
                        maxhealth += e.MaximumHealth;
                        extraabilities += e.GetStoredValue(StoredValue("MergedCount")) + 1;
                    }
                    if (currenthealth > 0 && maxhealth > 0 && extraabilities >= 0)
                    {
                        foreach (var e in targettedEnemies)
                        {
                            e.DirectDeath(null, false);
                        }
                        CombatManager.Instance.AddSubAction(new TrySpawnMergedEnemyAction(en, true, maxhealth, currenthealth, extraabilities));
                        exitAmount += targettedEnemies.Count;
                    }
                }
            }
            return exitAmount > 0;
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
