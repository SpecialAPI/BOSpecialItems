using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace BOSpecialItems.Content.Passive
{
    [HarmonyPatch]
    public static class UntetheredHealthColor
    {

        [HarmonyPatch(typeof(CombatVisualizationController), nameof(CombatVisualizationController.FirstInitialization))]
        [HarmonyPrefix]
        public static void CopyButton(CombatVisualizationController __instance)
        {
            var button = __instance.transform.Find("Canvas").Find("LowerZone").Find("InformationCanvas").Find("CharacterCostZone").Find("AttackCostLayout").Find("LuckyManaLayout").Find("ChangeLuckyBlueButton");
            var healthlayout = __instance.transform.Find("Canvas").Find("LowerZone").Find("InformationCanvas").Find("InfoZoneLayout").Find("PortraitZone").Find("PortraitHolder").Find("PortraitSprite").Find("CombatHealthBarLayout");
            var newButton = Object.Instantiate(button.gameObject, healthlayout);
            newButton.SetActive(false);
            newButton.transform.localPosition = newButton.AddComponent<LocalPositionConstantSetter>().targetPos = new Vector3(125f, 0f, 0f);
            newButton.transform.localScale = new Vector3(3f, 3f, 3f);
            var buttonComp = newButton.GetComponent<Button>();
            var holder = healthlayout.gameObject.AddComponent<ChangeHealthColorButtonHolder>();
            holder.originalSprite = newButton.GetComponent<Image>().sprite;
            holder.button = newButton;
            buttonComp.onClick = new();
            buttonComp.onClick.AddListener(holder.ButtonClicked);
            var colors = buttonComp.colors;
            colors.disabledColor = __instance._characterCost._performAttackButton.colors.disabledColor;
            buttonComp.colors = colors;
        }

        [HarmonyPatch(typeof(CombatHealthBarLayout), nameof(CombatHealthBarLayout.SetInformation))]
        [HarmonyPrefix]
        public static void UpdateButton(CombatHealthBarLayout __instance)
        {
            if(CombatManager.Instance != null && CombatManager.Instance._combatUI != null)
            {
                IUnit u;
                if (CombatManager.Instance._combatUI.IsInfoFromCharacter)
                {
                    u = CombatManager.Instance._stats.TryGetCharacterOnField(CombatManager.Instance._combatUI.UnitInInfoID);
                }
                else
                {
                    u = CombatManager.Instance._stats.TryGetEnemyOnField(CombatManager.Instance._combatUI.UnitInInfoID);
                }
                bool buttonActive = u != null && u.UnitExt().HealthColors.Count > 0;
                var buttonhold = __instance.GetComponent<ChangeHealthColorButtonHolder>();
                if(buttonhold != null && buttonhold.button != null)
                {
                    buttonhold.button.SetActive(buttonActive);
                    buttonhold.button.transform.localPosition = new Vector3(125f, 0f, 0f);
                }
            }
        }

        [HarmonyPatch(typeof(AttackCostLayout), nameof(AttackCostLayout.UpdatePerformAttackButton))]
        [HarmonyPostfix]
        public static void UpdateButtonInteractability(AttackCostLayout __instance)
        {
            if (CombatManager.Instance != null && CombatManager.Instance._combatUI != null)
            {
                var buttonhold = CombatManager.Instance._combatUI._infoZone._unitHealthBar.GetComponent<ChangeHealthColorButtonHolder>();
                if (buttonhold != null && buttonhold.button != null)
                {
                    buttonhold.button.GetComponent<Image>().sprite = (buttonhold.button.GetComponent<Button>().interactable = !__instance.PlayerInputLocked) ? buttonhold.originalSprite : ChangeHealthColorButtonHolder.disabledSprite;
                }
            }
        }

        [HarmonyPatch(typeof(CombatStats), nameof(CombatStats.AddCharacterToField))]
        [HarmonyPostfix]
        public static void SetupCharacterExt(CombatStats __instance, int characterID)
        {
            __instance.Characters[characterID].Ext();
        }

        [HarmonyPatch(typeof(CombatStats), nameof(CombatStats.AddEnemyToField))]
        [HarmonyPostfix]
        public static void SetupEnemyExt(CombatStats __instance, int enemyID)
        {
            __instance.Enemies[enemyID].Ext();
        }

        [HarmonyPatch(typeof(CharacterCombat), nameof(CharacterCombat.ChangeHealthColor))]
        [HarmonyPostfix]
        public static void ChangeCharacterColorOption(CharacterCombat __instance, ManaColorSO manaColor)
        {
            var ext = __instance.Ext();
            ext.HealthColors[ext.CurrentHealthColor % ext.HealthColors.Count] = manaColor;
        }

        [HarmonyPatch(typeof(EnemyCombat), nameof(EnemyCombat.ChangeHealthColor))]
        [HarmonyPostfix]
        public static void ChangeCharacterColorOption(EnemyCombat __instance, ManaColorSO manaColor)
        {
            var ext = __instance.Ext();
            ext.HealthColors[ext.CurrentHealthColor % ext.HealthColors.Count] = manaColor;
        }
    }

    public class LocalPositionConstantSetter : MonoBehaviour
    {
        public Vector3 targetPos;

        public void Update()
        {
            transform.localPosition = targetPos;
        }
    }

    public class ChangeHealthColorButtonHolder : MonoBehaviour
    {
        public GameObject button;
        public CombatHealthBarLayout layout;
        public Sprite originalSprite;
        public static Sprite disabledSprite;

        public void ButtonClicked()
        {
            if (CombatManager.Instance != null && CombatManager.Instance._combatUI != null)
            {
                IUnit u;
                if (CombatManager.Instance._combatUI.IsInfoFromCharacter)
                {
                    u = CombatManager.Instance._stats.TryGetCharacterOnField(CombatManager.Instance._combatUI.UnitInInfoID);
                }
                else
                {
                    u = CombatManager.Instance._stats.TryGetEnemyOnField(CombatManager.Instance._combatUI.UnitInInfoID);
                }
                if(u != null)
                {
                    var ext = u.UnitExt();
                    ext.CurrentHealthColor = (ext.CurrentHealthColor + 1) % ext.HealthColors.Count;
                    u.ChangeHealthColor(ext.HealthColors[ext.CurrentHealthColor]);
                }
            }
        }
    }
}
