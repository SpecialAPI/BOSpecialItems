using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.TextCore;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public class StartingCharacterSelector_Selectable : StartingCharacterSelectorBase
    {
        public override bool IsSelectable => true;

        public override bool Deterministic(SelectableCharactersSO chars, int selectedId)
        {
            return selectedId >= 0 && selectedId < chars.Characters.Length && chars.Characters[selectedId].HasCharacter;
        }

        public override CharacterSO GetCharacter(SelectableCharactersSO chars, int selectedId, List<CharacterSO> current, bool hasDPS, bool hasSupport, ref bool countAsDPSOrSupport, ref int ignoredAbility)
        {
            countAsDPSOrSupport = true;

            if (selectedId < 0 || selectedId >= chars.Characters.Length || !chars.Characters[selectedId].HasCharacter)
            {
                return StartingCharacterSelector_FullRandom.RandomlySelect(chars, current, hasDPS, hasSupport, out ignoredAbility);
            }
            else
            {
                if (ignoredAbility < 0 || ignoredAbility >= 3)
                {
                    ignoredAbility = Random.Range(0, 3);
                }

                return chars.Characters[selectedId].LoadedCharacter;
            }
        }

        public override Sprite GetImage()
        {
            return UISprites.CharacterSelect_Select;
        }
    }
}
