using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Items
{
    public static class AllSeeingEye
    {
        public static void Init()
        {
            var item = NewItem<PerformEffectWearable>("O OO00 OO00 - O00OO O0O O0O O00O OOO0 OOO 0 O0O OO00O O0O", "\"O00O O0O00 0 OO OOOO O00O0 O00O0 O0O0O O0000 O0O00 O00OO\"", "OOOO OOO0 0 OO OOOO OO0O O0 O O0O00 0 O0O OOO0 O00 , 0 O O00 O00 O00OO 0 O 0 O000 O O00O0 O0 O00O OOO0 OOO O0O O00O0 0 O O00OO 0 O 0 O0000 O0O O00O0 OO0O O OOO0 O0O OOO0 O0O00 0 O0000 O O00O0 O0O00 OO00O 0 OO0O O0O OO0O O0 O0O O00O0 . 0 O0O00 O000 O00O O00OO 0 O00O O0O00 O0O OO0O 0 O00O O00OO 0 O00 O0O O00OO O0O00 O00O0 OOOO OO00O O0O O00 0 O0O0O O0000 OOOO OOO0 0 O OO O0O00 O00O O0OO0 O O0O00 O00O OOOO OOO0 .\n[ PUT THIS BACK DOWN WHILE YOU STILL HAVE THE CHANCE. ]", "AllSeeingEye", ItemPools.Treasure);
            item.triggerOn = TriggerCalls.OnCombatEnd;
            item.effects = new EffectInfo[]
            {
                new()
                {
                    entryVariable = 1,
                    condition = null,
                    effect = CreateScriptable<GainLootCustomCharacterEffect>(x => { x._characterCopy = "BOSpecialItems_Harbinger_CH"; x._nameAddition = NameAdditionLocID.NameAdditionNone; x._rank = 0; })
                }
            };
            item.getsConsumedOnUse = true;
        }

        public static void Harbinger()
        {
            LoadedAssetsHandler.LoadedCharacters.Add("BOSpecialItems_Harbinger_CH", CreateScriptable<CharacterSO>(x =>
            {
                x._characterName = "O000 O O00O0 O0 O00O OOO0 OOO O0O O00O0";
                x.characterEntityID = ExtendEnum<EntityIDs>("Harbinger");
                x.healthColor = Pigments.Purple;
                x.usesBasicAbility = false;
                x.usesAllAbilities = true;
                x.rankedData = new CharacterRankedData[]
                {
                    new()
                    {
                        health = 10,
                        rankAbilities = new CharacterAbility[]
                        {
                            new()
                            {
                                cost = Cost()
                            },
                            new()
                            {
                                cost = Cost()
                            },
                            new()
                            {
                                cost = Cost()
                            }
                        }
                    }
                };
            }));
        }
    }
}
