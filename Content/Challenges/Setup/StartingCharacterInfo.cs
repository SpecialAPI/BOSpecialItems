using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public class StartingCharacterInfo(StartingCharacterSelectorBase selector, int rank = 0, bool mainCharacter = false)
    {
        public StartingCharacterSelectorBase Selector = selector;
        public int Rank = rank;
        public bool MainCharacter = mainCharacter;
    }
}
