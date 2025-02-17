using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Condition
{
    public class CurrentTurnIsSpecificTurnInRotationCondition : EffectConditionSO
    {
        public int Rotation;
        public int PositionInRotation;

        public static CurrentTurnIsSpecificTurnInRotationCondition Create(int pos, int rot)
        {
            return CreateScriptable<CurrentTurnIsSpecificTurnInRotationCondition>(x => { x.Rotation = rot; x.PositionInRotation = pos; });
        }

        public override bool MeetCondition(IUnit caster, EffectInfo[] effects, int currentIndex)
        {
            return CombatManager._instance._stats.TurnsPassed % Rotation + 1 == PositionInRotation;
        }
    }
}
