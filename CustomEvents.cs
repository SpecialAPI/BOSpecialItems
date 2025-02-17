using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class CustomEvents
    {
        public const string TARGETTING_ORIGIN_SID =         "BOSpecialItems_TargettingOriginSlotId";
        public const string TARGETTING_UNIT_CHARACTER =     "BOSpecialItems_TargettingUnitCharacter";

        public const string MODIFY_WRONG_PIGMENT =          "BOSpecialItems_ModifyWrongPigment";

        public const string PRE_ABILITY_USED =              "BOSpecialItems_PreAbilityUsed";
        public const string ABILITY_USED_CONTEXT =          "BOSpecialItems_OnAbilityUsedContext";

        public const string MODIFY_ABILITIES_RANK =         "BOSpecialItems_ModifyAbilityRank";

        public const string MODIFY_USED_ABILITY =           "BOSpecialItems_ModifyUsedAbility";

        public const string MODIFY_PIGMENT_PRODUCED =       "BOSpecialItems_ModifyPigmentProduced";

        public const string STATUS_EFFECT_FIRST_APPLIED =   "BOSpecialItems_StatusEffectFirstAplied";
        public const string STATUS_EFFECT_APPLIED =         "BOSpecialItems_StatusEffectApplied";
        public const string STATUS_EFFECT_INCREASED =       "BOSpecialItems_StatusEffectIncreased";

        public const string WILL_HEAL_UNIT =                "BOSpcialItems_WillHealUnit";

        public const string WILL_APPLY_DAMAGE_CONTEXT =     "BOSpecialItems_WillApplyDamageContext";
    }
}
