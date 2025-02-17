using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class AllAbilitiesAbilityItem
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("Abandoned Music Box", "\"An eerie tune\"", "At the start of combat, add an ability to this party member that performs all of their abilities. The cost for the ability is set to the combined cost of every ability currently on this party member.", "MusicBox", ItemPools.Shop, 5);
            item.triggerOn = TriggerCalls.OnCombatStart;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    condition = null,
                    effect = CreateScriptable<AddExtraAbilityWithCombinedCostEffect>(x => x.ability = CreateScriptable<AbilitySO>(x =>
                    {
                        x.visuals = null;
                        x.abilitySprite = LoadSprite("AttackIcon_Everything");
                        x.animationTarget = null;
                        x.intents = new IntentTargetInfo[]
                        {
                            new()
                            {
                                targetIntents = new IntentType[]
                                {
                                    IntentType.Misc
                                },
                                targets = TargettingLibrary.ThisSlot
                            }
                        };
                        x.effects = new EffectInfo[]
                        {
                            new()
                            {
                                condition = null,
                                effect = CreateScriptable<PerformAllAbilitiesButThisOffsettedEffect>(),
                                entryVariable = 0,
                                targets = null
                            }
                        };
                        x._abilityName = "Chaos Melody";
                        x._description = "Make this party member perform all of their abilities except this one. Abilities will be performed as if this party member's position is shifted to that ability's position relative to the position of the middle ability.";
                    }))
                }
            };
        }
    }
}
