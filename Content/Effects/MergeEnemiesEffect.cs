using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    [HarmonyPatch]
    public class MergeEnemiesEffect : EffectSO
    {
        public static BasePassiveAbilitySO Merged;

        public static void SetupMergedPassive()
        {
            Merged = CreateScriptable<IntegerSetterByStoredValuePassiveAbility>(x =>
            {
                x._valueName = x.specialStoredValue = ExtendEnum<UnitStoredValueNames>("MergedCount");
                x._postIncreaseStored = false;
                x.postIncreaseValue = 0;
                x._passiveName = "Merged";
                x._characterDescription = "This character is Merged";
                x._enemyDescription = "This enemy will perform an additional ability for each enemy merged into it.";
                x.conditions = null;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.AttacksPerTurn };
                x.doesPassiveTriggerInformationPanel = false;
                x.passiveIcon = LoadSprite("Merged");
                x.type = ExtendEnum<PassiveAbilityTypes>("Merged");
            });
        }

        [HarmonyPatch(typeof(TooltipTextHandlerSO), nameof(TooltipTextHandlerSO.ProcessStoredValue))]
        [HarmonyPostfix]
        public static void AddMergedEnemiesStoredValue(TooltipTextHandlerSO __instance, ref string __result, UnitStoredValueNames storedValue, int value)
        {
            if (string.IsNullOrEmpty(__result) && storedValue == ExtendEnum<UnitStoredValueNames>("MergedCount") && value > 0)
            {
                __result = $"<color=#{ColorUtility.ToHtmlStringRGB(__instance._positiveSTColor)}>Merged Enemies: {value}</color>";
            }
        }

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
                        dat.Item3 += ec.GetStoredValue(ExtendEnum<UnitStoredValueNames>("MergedCount")) + 1;
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
                    en.SetStoredValue(ExtendEnum<UnitStoredValueNames>("MergedCount"), extraAbilities);
                    en.AddPassiveAbility(MergeEnemiesEffect.Merged);
                }
            }
            yield return null;
        }
    }
}
