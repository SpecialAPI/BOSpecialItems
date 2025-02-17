using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class OilPaints
    {
        public static void Init()
        {
            var item = NewItem<MultiCustomTriggerEffectWearable>("Oil Paints", "", "When an enemy or a party member is inflicted with Oil-Slicked, give them a random Pigment Core that they don't have yet.", "OilPaints", ItemPools.Shop, 2);

            item.triggerEffects = new()
            {
                new EffectsAndCustomTriggerPair()
                {
                    conditions = new() { CreateScriptable<TargettedEffectApplicationMatchesEffectEffectorCondition>(x => x.targetEffect = StatusEffectType.OilSlicked) },
                    doesPopup = true,
                    immediate = false,
                    customTrigger = CustomEvents.STATUS_EFFECT_APPLIED,
                    effect = new DoEffectOnNotificationTargetTriggerEffect()
                    {
                        targetEffect = new PerformEffectTriggerEffect()
                        {
                            effects = new EffectInfo[]
                            {
                                new()
                                {
                                    condition = null,
                                    effect = CreateScriptable<AddNewPassiveNotInHealthOptionsEffect>(x => x.passives = new()
                                    {
                                        { PigmentType.Red,     CustomPassives.RedHealth },
                                        { PigmentType.Blue,    CustomPassives.BlueHealth },
                                        { PigmentType.Yellow,  CustomPassives.YellowHealth },
                                        { PigmentType.Purple,  CustomPassives.PurpleHealth }
                                    }),
                                    entryVariable = 0,
                                    targets = TargettingLibrary.ThisSlot
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
