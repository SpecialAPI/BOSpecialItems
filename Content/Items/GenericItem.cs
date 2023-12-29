using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BOSpecialItems.Content.Items
{
    public class GenericItem<T> : Item where T : BaseWearableSO
    {
        public T item;

        public GenericItem(string name, string flavor, string description, string sprite, ItemPools pools, int shopprice = 0)
        {
            item = ScriptableObject.CreateInstance<T>();
            item.name = item._itemName = this.name = name;
            item._flavourText = flavor;
            item._description = description;
            item.wearableImage = LoadSprite(sprite);
            itemPools = pools;
            item.isShopItem = pools.HasFlag(ItemPools.Shop);
            item.shopPrice = shopprice;
            item.staticModifiers = new WearableStaticModifierSetterSO[0];
        }

        public override BaseWearableSO Wearable()
        {
            return item;
        }
    }
}
