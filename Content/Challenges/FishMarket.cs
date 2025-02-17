using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges
{
    public class FishMarket : ChallengeBase
    {
        public override string Name => "Fish Market";
        public override string ID => "FishMarket";

        public override string RulesText => "Money chests no longer appear.\nBronzo no longer apppears.\nCombat rewards give no money, unless a Purple Heart was used.";

        public override StartingCharacterInfo[] StartingCharacters => new StartingCharacterInfo[]
        {
            new(new StartingCharacterSelector_Specific("Nowak_CH"), 0, true),

            new(new StartingCharacterSelector_Selectable(), 0),
            new(new StartingCharacterSelector_FullRandom(), 0),

            new(new StartingCharacterSelector_Specific("Mung_CH"), 0),
            new(new StartingCharacterSelector_Specific("Mung_CH"), 0),
            new(new StartingCharacterSelector_Specific("Mung_CH"), 0),
        };

        public override string[] StartingItems => new string[]
        {
            "PurpleHeart_SW",
            "PurpleHeart_SW",
            "PurpleHeart_SW",
            "PurpleHeart_SW",
        };
    }
}
