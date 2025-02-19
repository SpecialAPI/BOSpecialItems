﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class Pigments
    {
        public static ManaColorSO Red;
        public static ManaColorSO Blue;
        public static ManaColorSO Yellow;
        public static ManaColorSO Purple;
        public static ManaColorSO Green;
        public static ManaColorSO Grey;

        public static Texture2D optionalTemplate;
        public static Texture2D punishingTemplate;

        public static Texture2D optionalPigmentTemplate;
        public static Texture2D punishingPigmentTemplate;

        private const int SPLIT_PIGMENT_LIMIT = 4;

        private readonly static Dictionary<ManaColorSO[], ManaColorSO> AlreadyMadeSplitPigment = new();
        private readonly static Dictionary<ManaColorSO, ManaColorSO> AlreadyMadeOptionalPigment = new();
        private readonly static Dictionary<ManaColorSO, ManaColorSO> AlreadyMadePunishingPigment = new();

        private static Dictionary<Sprite, Sprite> readableVersions = new();

        private static Sprite Readable(Sprite orig)
        {
            if(orig != null)
            {
                if (orig.texture.isReadable)
                {
                    return orig;
                }
                else if(readableVersions.TryGetValue(orig, out var rw))
                {
                    return rw;
                }
            }
            return null;
        }

        public static void Init()
        {
            Red = LoadedAssetsHandler.LoadCharacter("Burnout_CH").healthColor;
            Blue = LoadedAssetsHandler.LoadCharacter("Cranes_CH").healthColor;
            Yellow = LoadedAssetsHandler.LoadCharacter("Dimitri_CH").healthColor;
            Purple = LoadedAssetsHandler.LoadCharacter("Nowak_CH").healthColor;
            Grey = LoadedAssetsHandler.LoadCharacter("Gospel_CH").healthColor;

            Green = CreateScriptable<ManaColorSO>(x =>
            {
                x.canGenerateMana = true;
                x.pigmentType = PigmentType.Green;
                x.dealsCostDamage = true;
                x.healthColor = Color.green;
                x.manaSprite = LoadSprite("GreenMana");
                x.manaUsedSprite = LoadSprite("GreenManaUsed");
                x.manaCostSprite = LoadSprite("GreenManaCostUnselected");
                x.manaCostSelectedSprite = LoadSprite("GreenManaCostSelected");
                x.healthSprite = LoadSprite("GreenManaHealth");
                x.manaSoundEvent = Red.manaSoundEvent;

                x.name = "Pigment_Green";
            });

            optionalTemplate = LoadTexture("Optional_2");
            punishingTemplate = LoadTexture("Optional_3");

            optionalPigmentTemplate = LoadTexture("Optional_2_Pigment");
            punishingPigmentTemplate = LoadTexture("Optional_3_Pigment");

            readableVersions = new()
            {
                { Red.manaSprite,                   LoadSprite("RedMana") },
                { Red.manaUsedSprite,               LoadSprite("RedManaUsed") },
                { Red.manaCostSprite,               LoadSprite("RedManaCostUnselected") },
                { Red.manaCostSelectedSprite,       LoadSprite("RedManaCostSelected") },
                { Red.healthSprite,                 LoadSprite("RedManaHealth") },

                { Blue.manaSprite,                  LoadSprite("BlueMana") },
                { Blue.manaUsedSprite,              LoadSprite("BlueManaUsed") },
                { Blue.manaCostSprite,              LoadSprite("BlueManaCostUnselected") },
                { Blue.manaCostSelectedSprite,      LoadSprite("BlueManaCostSelected") },
                { Blue.healthSprite,                LoadSprite("BlueManaHealth") },
                
                { Yellow.manaSprite,                LoadSprite("YellowMana") },
                { Yellow.manaUsedSprite,            LoadSprite("YellowManaUsed") },
                { Yellow.manaCostSprite,            LoadSprite("YellowManaCostUnselected") },
                { Yellow.manaCostSelectedSprite,    LoadSprite("YellowManaCostSelected") },
                { Yellow.healthSprite,              LoadSprite("YellowManaHealth") },
                
                { Purple.manaSprite,                LoadSprite("PurpleMana") },
                { Purple.manaUsedSprite,            LoadSprite("PurpleManaUsed") },
                { Purple.manaCostSprite,            LoadSprite("PurpleManaCostUnselected") },
                { Purple.manaCostSelectedSprite,    LoadSprite("PurpleManaCostSelected") },
                { Purple.healthSprite,              LoadSprite("PurpleManaHealth") },
                
                { Grey.manaSprite,                  LoadSprite("GreyMana") },
                { Grey.manaUsedSprite,              LoadSprite("GreyManaUsed") },
                { Grey.manaCostSprite,              LoadSprite("GreyManaCostUnselected") },
                { Grey.manaCostSelectedSprite,      LoadSprite("GreyManaCostSelected") },
                { Grey.healthSprite,                LoadSprite("GreyManaHealth") },
            };
        }

        public static ManaColorSO Optional(this ManaColorSO orig)
        {
            if(orig == null)
            {
                return null;
            }
            if (AlreadyMadeOptionalPigment.TryGetValue(orig, out var alreadyExists))
            {
                return alreadyExists;
            }

            return CreateScriptable<ManaColorSOAdvanced>(x =>
            {
                x.Inherit(orig);

                var pigment = StitchTextures(optionalPigmentTemplate, new List<Sprite>() { orig.manaSprite });
                var cost = StitchTextures(optionalTemplate, new List<Sprite>() { Readable(orig.manaCostSprite) });
                var selected = StitchTextures(optionalTemplate, new List<Sprite>() { Readable(orig.manaCostSelectedSprite) });

                x.manaSprite = Sprite.Create(pigment, new Rect(0f, 0f, pigment.width, pigment.height), new Vector2(0.5f, 0.5f));
                x.manaCostSprite = Sprite.Create(cost, new Rect(0f, 0f, cost.width, cost.height), new Vector2(0.5f, 0.5f));
                x.manaCostSelectedSprite = Sprite.Create(selected, new Rect(0f, 0f, selected.width, selected.height), new Vector2(0.5f, 0.5f));

                x.requiresPigment = false;
                x.noPigmentCountsAsDamage = false;

                x.name = $"{orig.name}_Optional";

                AlreadyMadeOptionalPigment[orig] = x;
            });
        }

        public static ManaColorSO Punishing(this ManaColorSO orig)
        {
            if (orig == null)
            {
                return null;
            }
            if (AlreadyMadePunishingPigment.TryGetValue(orig, out var alreadyExists))
            {
                return alreadyExists;
            }

            return CreateScriptable<ManaColorSOAdvanced>(x =>
            {
                x.Inherit(orig);

                var pigment = StitchTextures(punishingPigmentTemplate, new List<Sprite>() { orig.manaSprite });
                var cost = StitchTextures(punishingTemplate, new List<Sprite>() { orig.manaCostSprite });
                var selected = StitchTextures(punishingTemplate, new List<Sprite>() { orig.manaCostSelectedSprite });

                x.manaSprite = Sprite.Create(pigment, new Rect(0f, 0f, pigment.width, pigment.height), new Vector2(0.5f, 0.5f));
                x.manaCostSprite = Sprite.Create(cost, new Rect(0f, 0f, cost.width, cost.height), new Vector2(0.5f, 0.5f));
                x.manaCostSelectedSprite = Sprite.Create(selected, new Rect(0f, 0f, selected.width, selected.height), new Vector2(0.5f, 0.5f));

                x.requiresPigment = false;
                x.noPigmentCountsAsDamage = true;

                x.name = $"{orig.name}_Punishing";

                AlreadyMadePunishingPigment[orig] = x;
            });
        }

        public static ManaColorSO SplitPigment(params ManaColorSO[] stuff)
        {
            if(stuff.Length <= 0)
            {
                Debug.LogError("Attempting to create a split pigment comprised of 0 pigment..?");
                return null;
            }
            else if (stuff.Length == 1)
            {
                Debug.LogError("Attempting to create a split pigment comprised of 1 pigment... why");
                return stuff[0];
            }
            else if(stuff.Length > SPLIT_PIGMENT_LIMIT)
            {
                Debug.LogError($"Attempting to create a split pigment comprised of {stuff.Length} pigment, which is over the split pigment limit of {SPLIT_PIGMENT_LIMIT}.");
                return null;
            }
            if(AlreadyMadeSplitPigment.TryGetValue(stuff, out var alreadyExists))
            {
                return alreadyExists;
            }
            var split = CreateScriptable<ManaColorSOAdvanced>(x =>
            {
                var name = "Pigment";

                var type = PigmentType.None;
                var dealsWrongPigmentDamage = true;
                var canGenerate = false;

                var requirePigment = true;
                var noPigmentCountsAsDamage = true;

                var manaSprites = new List<Sprite>();
                var usedSprites = new List<Sprite>();
                var costSprites = new List<Sprite>();
                var selectedCostSprites = new List<Sprite>();
                var healthSprites = new List<Sprite>();

                foreach (var pigment in stuff)
                {
                    type |= pigment.pigmentType;
                    dealsWrongPigmentDamage &= pigment.dealsCostDamage;
                    canGenerate |= pigment.canGenerateMana;

                    if(pigment is ManaColorSOAdvanced adv)
                    {
                        requirePigment &= adv.requiresPigment;
                        noPigmentCountsAsDamage &= adv.noPigmentCountsAsDamage;
                    }

                    manaSprites.Add(Readable(pigment.manaSprite));
                    usedSprites.Add(Readable(pigment.manaUsedSprite));
                    costSprites.Add(Readable(pigment.manaCostSprite));
                    selectedCostSprites.Add(Readable(pigment.manaCostSelectedSprite));
                    healthSprites.Add(Readable(pigment.healthSprite));

                    name += $"_{pigment.pigmentType.ToString().TrimGuid()[0]}";
                }

                x.pigmentType = type;
                x.dealsCostDamage = dealsWrongPigmentDamage;
                x.canGenerateMana = canGenerate;
                x.manaSoundEvent = stuff[0].manaSoundEvent;

                x.requiresPigment = requirePigment;
                x.noPigmentCountsAsDamage = noPigmentCountsAsDamage;

                var manaSprite = StitchTextures(LoadTexture($"Split{stuff.Length}_Pigment"), manaSprites);
                var usedSprite = StitchTextures(LoadTexture($"Split{stuff.Length}_Selected"), usedSprites);
                var costSprite = StitchTextures(LoadTexture($"Split{stuff.Length}_Cost"), costSprites);
                var selectedCostSprite = StitchTextures(LoadTexture($"Split{stuff.Length}_Cost"), selectedCostSprites);
                var healthSprite = StitchTextures(LoadTexture($"Split{stuff.Length}_Health"), healthSprites);

                x.manaSprite = Sprite.Create(manaSprite, new Rect(0f, 0f, manaSprite.width, manaSprite.height), new Vector2(0.5f, 0.5f));
                x.manaUsedSprite = Sprite.Create(usedSprite, new Rect(0f, 0f, usedSprite.width, usedSprite.height), new Vector2(0.5f, 0.5f));
                x.manaCostSprite = Sprite.Create(costSprite, new Rect(0f, 0f, costSprite.width, costSprite.height), new Vector2(0.5f, 0.5f));
                x.manaCostSelectedSprite = Sprite.Create(selectedCostSprite, new Rect(0f, 0f, selectedCostSprite.width, selectedCostSprite.height), new Vector2(0.5f, 0.5f));
                x.healthSprite = Sprite.Create(healthSprite, new Rect(0f, 0f, healthSprite.width, healthSprite.height), new Vector2(0.5f, 0.5f));

                x.name = name;
            });
            AlreadyMadeSplitPigment[stuff] = split;
            return split;
        }

        private static Texture2D StitchTextures(Texture2D template, IList<Sprite> sprites)
        {
            var tex = new Texture2D(template.width, template.height);
            tex.filterMode = FilterMode.Point;

            for(int x = 0; x < tex.width; x++)
            {
                for(int y = 0; y < tex.width; y++)
                {
                    var templatePixel = template.GetPixel(x, y);
                    var index = Array.IndexOf(stitchReplacements, templatePixel);
                    if (index >= 0 && index < sprites.Count)
                    {
                        var target = sprites[index];
                        tex.SetPixel(x, y, target.texture.GetPixel((int)(target.rect.width / 2) + (x - (template.width / 2)), (int)(target.rect.height / 2) + (y - (template.height / 2))));
                    }
                    else
                    {
                        tex.SetPixel(x, y, templatePixel);
                    }
                }
            }

            tex.Apply();

            return tex;
        }

        private static Color[] stitchReplacements = new Color[]
        {
            new(1f, 0f, 0f),
            new(0f, 1f, 0f),
            new(0f, 0f, 1f),
            new(1f, 1f, 0f),
        };
    }
}
