using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class TargettingAllSlots : BaseCombatTargettingSO
    {
        public override bool AreTargetAllies => true;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            return slots.CharacterSlots.Concat(slots.EnemySlots).Select(x => x.TargetSlotInformation).ToArray();
        }
    }
}
