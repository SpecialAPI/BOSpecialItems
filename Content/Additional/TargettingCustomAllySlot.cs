using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class TargettingCustomAllySlot : BaseCombatTargettingSO
    {
        public List<int> targetOffsets;
        public List<int> frontOffsets;

        public override bool AreTargetAllies => false;

        public override bool AreTargetSlots => true;

        public override TargetSlotInfo[] GetTargets(SlotsCombat slots, int casterSlotID, bool isCasterCharacter)
        {
            var targets = new List<TargetSlotInfo>();
            foreach(var offs in targetOffsets)
            {
                if (offs == 0)
                {
                    foreach (var t in slots.GetAllSelfSlots(casterSlotID, isCasterCharacter))
                    {
                        foreach (int frontOffs in frontOffsets)
                        {
                            if (t.SlotID == casterSlotID + frontOffs)
                            {
                                targets.Add(t);
                            }
                        }
                    }
                }
                else
                {
                    var t = slots.GetAllySlotTarget(casterSlotID, offs, isCasterCharacter);
                    if (t != null)
                    {
                        targets.Add(t);
                    }
                }
            }
            return targets.ToArray();
        }
    }
}
