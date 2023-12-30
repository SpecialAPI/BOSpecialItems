using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class UntetheredHealthItem
    {

        public static void Init()
        {
            var untetheredHealthItem = new GenericItem<PerformEffectWearable>("Artist's Palette", "\"Primary Colors\"", "Adds Untethered Core to this party member as a passive.\nAt the beginning of combat, add Untethered Core to the opposing enemy as a passive.\nCore allows health color to be toggled to other colors.", "Palette", ItemPools.Treasure);
            untetheredHealthItem.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = CustomPassives.UntetheredHealth;
                })
            };
            untetheredHealthItem.item.triggerOn = TriggerCalls.OnCombatStart;
            untetheredHealthItem.item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddPassiveEffect>(x => x._passiveToAdd = CustomPassives.UntetheredHealth),
                    entryVariable = 0,
                    targets = TargettingLibrary.OpposingSlot
                }
            };
            untetheredHealthItem.AddItem();
            untetheredHealthItem.item.AttachGadget(GadgetDB.GetGadget("Flood"));
        }
    }
}
