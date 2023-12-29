using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class TargettingStrongestUnit : BaseCombatTargettingSO
    {
        public bool isAllies;

        public override bool AreTargetAllies => isAllies;

        public override bool AreTargetSlots => false;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var unitSlots = slots.GetAllUnitTargetSlots(isAllies == isCasterCharacter, false, -1);
            var highestHealth = -1;
            List<TargetSlotInfo> results = new();
            foreach (var slot in unitSlots)
            {
                if (slot.HasUnit && slot.Unit.CurrentHealth >= highestHealth)
                {
                    if(slot.Unit.CurrentHealth > highestHealth)
                    {
                        highestHealth = slot.Unit.CurrentHealth;
                        results.Clear();
                    }
                    results.Add(slot);
                }
            }
            return results.ToArray();
        }
    }
}
