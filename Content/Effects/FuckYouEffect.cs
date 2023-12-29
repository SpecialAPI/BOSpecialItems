using System;
using System.Collections.Generic;
using System.Text;
using Tools;

namespace BOSpecialItems.Content.Effects
{
    public class FuckYouEffect : EffectSO
    {
        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            if (CombatManager.Instance._isGameRun && CombatManager.Instance._shouldRunSave)
            {
                SaveManager.DeleteRunSaveData();
                CombatManager.Instance._informationHolder.Game.SetIntData(DataUtils.winStreakVar, 0);
                CombatManager.Instance.PartiallySaveGame();
            }
            Application.Quit();
            exitAmount = 0;
            return false;
        }
    }
}
