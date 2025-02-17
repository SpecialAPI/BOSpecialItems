using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOSpecialItems.Content.Effects
{
    public class GainLootRandomCustomCharacterEffect : EffectSO
    {
        public List<string> possibleCharacters;
        public int rank;
        public NameAdditionLocID nameAddition;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            var ch = LoadedAssetsHandler.GetCharcater(possibleCharacters[Random.Range(0, possibleCharacters.Count)]);
            if (ch == null)
            {
                return false;
            }
            var maxHealth = ch.GetMaxHealth(rank);
            ch.GenerateAbilities(out var firstAbility, out var secondAbility);
            var nameAdditionData = LocUtils.LocDB.GetNameAdditionData(nameAddition);
            for (int i = 0; i < entryVariable; i++)
            {
                var newCharacter = new SpawnedCharacterAddition(ch, nameAdditionData, rank, firstAbility, secondAbility, maxHealth);
                stats.GainCharacterLoot(newCharacter);
            }
            exitAmount = entryVariable;
            return true;
        }
    }
}
