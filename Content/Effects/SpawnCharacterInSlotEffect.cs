using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class SpawnCharacterInSlotEffect : EffectSO
    {
        public string characterName;
        public int slot;
        public bool alwaysTrySpawn;
        public bool permanent;
        public int rank;
        public bool addToCasterRank;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            exitAmount = 0;
            var character = LoadedAssetsHandler.GetCharcater(characterName);

            if(character == null)
            {
                return false;
            }

            var targetRank = rank + (addToCasterRank && caster is CharacterCombat cc ? cc.Rank : 0);
            var health = character.GetMaxHealth(targetRank);

            character.GenerateAbilities(out var firstAbility, out var secondAbility);

            CombatManager.Instance.AddSubAction(new SpawnCharacterAction(character, slot, alwaysTrySpawn, "", permanent, targetRank, firstAbility, secondAbility, health));

            return true;
        }
    }
}
