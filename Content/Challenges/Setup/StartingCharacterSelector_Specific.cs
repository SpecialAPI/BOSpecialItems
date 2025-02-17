using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public class StartingCharacterSelector_Specific(string characterName, int ignoredAbility = 0) : StartingCharacterSelectorBase
    {
        public string name = characterName;
        public int ignored = ignoredAbility;

        public override bool IsSelectable => false;

        public override bool Deterministic(SelectableCharactersSO chars, int selectedId)
        {
            return true;
        }

        public override CharacterSO GetCharacter(SelectableCharactersSO chars, int selectedId, List<CharacterSO> current, bool hasDPS, bool hasSupport, ref bool countAsDPSOrSupport, ref int ignoredAbility)
        {
            ignoredAbility = ignored;
            return LoadedAssetsHandler.LoadCharacter(name);
        }

        public override Sprite GetImage()
        {
            return LoadedAssetsHandler.LoadCharacter(name).characterSprite;
        }
    }
}
