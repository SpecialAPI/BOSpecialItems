using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public abstract class StartingCharacterSelectorBase
    {
        public abstract Sprite GetImage();
        public abstract CharacterSO GetCharacter(SelectableCharactersSO chars, int selectedId, List<CharacterSO> current, bool hasDPS, bool hasSupport, ref bool countAsDPSOrSupport, ref int ignoredAbility);
        public abstract bool Deterministic(SelectableCharactersSO chars, int selectedId);
        public abstract bool IsSelectable { get; }
    }
}
