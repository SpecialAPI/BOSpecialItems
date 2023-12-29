using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class Bleach
    {
        public static void Init()
        {
            var bleach = new GenericItem<BasicWearable>("Magickal Bleach", "\"A cure for death, inanimacy and your head being on fire. Just drink it and you're good to go!\"", "This party member no longer has any passives.", "MagickalBleach", ItemPools.Treasure);
            bleach.item.staticModifiers = new WearableStaticModifierSetterSO[]
            {
                CreateScriptable<ExtraPassiveAbility_Wearable_SMS>(x =>
                {
                    x._extraPassiveAbility = CreateScriptable<BleachFlag>();
                })
            };
            bleach.AddItem();
            bleach.item.AttachGadget(GadgetDB.GetGadget("Cleanse"));
        }
    }
}
