using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Additional
{
    public class OffsettedEffectAction(EffectInfo[] effects, int offset, IUnit caster, int startResult = 0) : EffectAction(effects, caster, startResult)
    {
        public int offset = offset;

        public override IEnumerator Execute(CombatStats stats)
        {
            var resultValue = _startResult;
            for (int i = 0; i < _effects.Length; i++)
            {
                var condition = _effects[i].condition;
                if (condition == null || condition.Equals(null) || condition.MeetCondition(_caster, _effects, i))
                {
                    TargetSlotInfo[] possibleTargets = _effects[i].targets != null ? _effects[i].targets.GetTargets(stats.combatSlots, EventPatches.ChangeCasterSlotId(_caster.SlotID + offset, _caster, null, 0), EventPatches.ChangeCasterCharacter(_caster.IsUnitCharacter, _caster, null, 0)) : new TargetSlotInfo[0];
                    var areTargetSlots = !(_effects[i].targets != null) || _effects[i].targets.AreTargetSlots;
                    resultValue = _effects[i].StartEffect(stats, _caster, possibleTargets, areTargetSlots, resultValue);
                }
                else
                {
                    resultValue = _effects[i].FailEffect(resultValue);
                }
                yield return null;
            }
        }
    }
}
