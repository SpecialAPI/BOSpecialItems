using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.TextCore;
using Utility.SerializableCollection;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public class StartingCharacterSelector_FullRandom : StartingCharacterSelectorBase
    {
        public override bool IsSelectable => false;

        public override CharacterSO GetCharacter(SelectableCharactersSO chars, int selectedId, List<CharacterSO> current, bool hasDPS, bool hasSupport, ref bool countAsDPSOrSupport, ref int ignoredAbility)
        {
            return RandomlySelect(chars, current, hasDPS, hasSupport, out ignoredAbility);
        }

        public static CharacterSO RandomlySelect(SelectableCharactersSO chars, List<CharacterSO> current, bool hasDPS, bool hasSupport, out int ignoredAbility)
        {
            ignoredAbility = -1;
            var validChars = chars.Characters.Where(x => x.HasCharacter && !x.IgnoreRandomSelection && !current.Contains(x.LoadedCharacter)).ToList();

            if (hasDPS != hasSupport)
            {
                var possible = new List<CharacterSO>();
                var key = default(CharacterRefString);
                var dict = hasDPS ? chars._dpsCharacters : chars._supportCharacters;
                foreach (var ch in validChars)
                {
                    var character = ch.LoadedCharacter;

                    key.character = character.name;
                    if (dict.ContainsKey(key))
                    {
                        possible.Add(character);
                    }
                }

                if (possible.Count <= 0)
                {
                    return null;
                }

                var randomStuff = possible.GetRandomElement();
                key.character = randomStuff.name;
                if (dict.TryGetValue(key, out var abilities))
                {
                    var ab = abilities.ignoredAbilities;
                    if (ab == null || ab.Count <= 0)
                    {
                        ignoredAbility = Random.Range(0, 3);
                    }
                    else
                    {
                        ignoredAbility = ab.GetRandomElement();
                    }
                    return randomStuff;
                }

                return null;
            }
            else
            {

                if(validChars.Count <= 0)
                {
                    return null;
                }

                ignoredAbility = Random.Range(0, 3);
                return validChars.GetRandomElement().LoadedCharacter;
            }
        }

        public override Sprite GetImage()
        {
            return UISprites.CharacterSelect_FullRandom;
        }

        public override bool Deterministic(SelectableCharactersSO chars, int selectedId)
        {
            return false;
        }
    }
}
