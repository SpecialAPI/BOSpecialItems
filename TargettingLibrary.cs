using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class TargettingLibrary
    {
        public static BaseCombatTargettingSO ThisSlot = CreateScriptable<Targetting_BySlot_Index>(x =>
        {
            x.allSelfSlots = true;
            x.getAllies = true;
            x.slotPointerDirections = new int[]
            {
                0
            };
        });

        public static BaseCombatTargettingSO OpposingSlot = CreateScriptable<Targetting_BySlot_Index>(x =>
        {
            x.allSelfSlots = true;
            x.getAllies = false;
            x.slotPointerDirections = new int[]
            {
                0
            };
        });

        public static BaseCombatTargettingSO ThisSide = CreateScriptable<GenericTargetting_BySlot_Index>(x =>
        {
            x.getAllies = true;
            x.slotPointerDirections = new int[]
            {
                0,
                1,
                2,
                3,
                4
            };
        });

        public static BaseCombatTargettingSO OpposingSide = CreateScriptable<GenericTargetting_BySlot_Index>(x =>
        {
            x.getAllies = false;
            x.slotPointerDirections = new int[]
            {
                0,
                1,
                2,
                3,
                4
            };
        });

        public static BaseCombatTargettingSO AllAlliesVisual = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO AllAllies = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO AllAlliesButThisVisual = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = true;
        });

        public static BaseCombatTargettingSO AllAlliesButThis = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = true;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = true;
        });

        public static BaseCombatTargettingSO AllEnemiesVisual = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = false;
            x.getAllUnitSlots = true;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO AllEnemies = CreateScriptable<Targetting_ByUnit_Side>(x =>
        {
            x.getAllies = false;
            x.getAllUnitSlots = false;
            x.ignoreCastSlot = false;
        });

        public static BaseCombatTargettingSO Relative(bool allies, params int[] offsets)
        {
            return CreateScriptable<Targetting_BySlot_Index>(x =>
            {
                x.allSelfSlots = true;
                x.getAllies = allies;
                x.slotPointerDirections = offsets;
            });
        }

        public static BaseCombatTargettingSO Absolute(bool allies, params int[] positions)
        {
            return CreateScriptable<GenericTargetting_BySlot_Index>(x =>
            {
                x.getAllies = allies;
                x.slotPointerDirections = positions;
            });
        }
    }
}
