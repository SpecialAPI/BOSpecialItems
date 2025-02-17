using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class UISprites
    {
        public static Sprite NineSlice_Colored;
        public static Sprite NineSlice_Purple;

        public static Sprite CharacterSelect_Select;
        public static Sprite CharacterSelect_FullRandom;

        public static Sprite ManaToggle_Disabled;

        public static void Load()
        {
            var ns_c = LoadTexture("UINineSliceTest");
            NineSlice_Colored = Sprite.Create(ns_c, new Rect(0f, 0f, ns_c.width, ns_c.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));

            var ns_p = LoadTexture("UINineSliceTest_3");
            NineSlice_Purple = Sprite.Create(ns_p, new Rect(0f, 0f, ns_p.width, ns_p.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));

            CharacterSelect_Select = LoadSprite("Characterselect_Select");
            CharacterSelect_FullRandom = LoadSprite("Characterselect_ForceRandom");
            ManaToggle_Disabled = LoadSprite("UI_ManaToggle_Disabled");
        }
    }
}
