using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class TheSquirrel
    {
        public static void Init()
        {
            var squirrel = new GenericItem<ModifyWrongPigmentWearable>("The Squirrel", "\"Free Sacrifice\"", "The first wrong pigment used in an ability deals no damage and doesn't trigger Delicate.", "TheSquirrel", ItemPools.Treasure);
            squirrel.item.addWrongPigment = -1;
            squirrel.item.doesItemPopUp = false;
            squirrel.AddItem();
            squirrel.item.AttachGadget(GadgetDB.GetGadget("Concentrate"));
        }
    }
}
