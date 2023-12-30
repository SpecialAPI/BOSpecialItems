using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace BOSpecialItems
{
    [HarmonyPatch]
    public static class GlossaryStuffAdder
    {
        public static GlossaryDataBase glossaryDB;

        private readonly static List<StatusEffectInfoSO> queuedStatus = new();
        private readonly static List<SlotStatusEffectInfoSO> queuedField = new();
        private readonly static List<GlossaryPassives> queuedPassives = new();
        private readonly static List<GlossaryKeywords> queuedKeywords = new();

        [HarmonyPatch(typeof(GlossaryDataBase), nameof(GlossaryDataBase.Status), MethodType.Getter)]
        [HarmonyPatch(typeof(GlossaryDataBase), nameof(GlossaryDataBase.Fields), MethodType.Getter)]
        [HarmonyPatch(typeof(GlossaryDataBase), nameof(GlossaryDataBase.Passives), MethodType.Getter)]
        [HarmonyPatch(typeof(GlossaryDataBase), nameof(GlossaryDataBase.Keywords), MethodType.Getter)]
        [HarmonyPrefix]
        private static void CatchUp(GlossaryDataBase __instance)
        {
            if(glossaryDB == null)
            {
                glossaryDB = __instance;
                glossaryDB._status = glossaryDB._status.Concat(queuedStatus).ToArray();
                glossaryDB._fields = glossaryDB._fields.Concat(queuedField).ToArray();
                glossaryDB._passives = glossaryDB._passives.Concat(queuedPassives).ToArray();
                glossaryDB._keywords = glossaryDB._keywords.Concat(queuedKeywords).ToArray();
                queuedStatus.Clear();
                queuedField.Clear();
                queuedPassives.Clear();
                queuedKeywords.Clear();
            }
        }

        [HarmonyPatch(typeof(GlossaryListUIPanel), nameof(GlossaryListUIPanel.TryInitializeStatus))]
        [HarmonyPrefix]
        private static void AddExtraStatusIconsIfNeeded(GlossaryListUIPanel __instance, StatusEffectInfoSO[] status)
        {
            if (!__instance._initialized)
            {
                if (status.Length > __instance._icons.Length)
                {
                    var l = __instance._icons.ToList();
                    var toadd = status.Length - __instance._icons.Length;
                    for (int i = 0; i < toadd; i++)
                    {
                        l.Add(Object.Instantiate(l[0], l[0].transform.parent));
                    }
                    __instance._icons = l.ToArray();
                    (__instance.transform as RectTransform).sizeDelta = new(0f, 284f + 105f * Mathf.Max(Mathf.CeilToInt(l.Count / 8f) - 3, 0));
                }
                
                if(__instance.GetComponent<HorizontalLayoutGroup>() != null)
                {
                    Object.DestroyImmediate(__instance.GetComponent<HorizontalLayoutGroup>());
                    var grid = __instance.gameObject.AddComponent<GridLayoutGroup>();
                    grid.cellSize = new(118.5f, 105f);
                }

                if(__instance.transform.parent != null && (__instance.transform.parent.parent == null || __instance.transform.parent.parent.GetComponent<ScrollRect>() == null))
                {
                    var y = 410f;

                    __instance.transform.parent.Find("StatusTitleText (TMP)").localPosition = new(25f, y);

                    var scrollView = new GameObject("Scroll View");
                    scrollView.AddComponent<RectTransform>();
                    scrollView.transform.SetParent(__instance.transform.parent, false);

                    var viewport = new GameObject("Viewport");
                    var viewportTransform = viewport.AddComponent<RectTransform>();
                    viewport.transform.SetParent(scrollView.transform, false);
                    viewport.AddComponent<Image>();
                    viewport.AddComponent<Mask>().showMaskGraphic = false;
                    viewportTransform.sizeDelta = new(1000, 315);

                    __instance.transform.SetParent(viewport.transform, false);

                    var scrollimg = scrollView.AddComponent<Image>();
                    var tex = LoadTexture("UINineSliceTest_3");
                    scrollimg.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));
                    scrollimg.pixelsPerUnitMultiplier = 0.05f;
                    scrollimg.type = Image.Type.Sliced;
                    var scrollRect = scrollView.AddComponent<ScrollRect>();
                    scrollRect.content = __instance.transform as RectTransform;
                    scrollRect.viewport = viewport.transform as RectTransform;
                    scrollRect.horizontal = false;
                    scrollRect.vertical = true;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.transform.localPosition = new(-28f, y - 50f);
                    (scrollRect.transform as RectTransform).sizeDelta = new(1025f, 340f);
                    (scrollRect.transform as RectTransform).pivot = new(0.5f, 1f);
                    scrollRect.scrollSensitivity = 10f;
                    scrollRect.movementType = ScrollRect.MovementType.Clamped;

                    (__instance.transform as RectTransform).pivot = new(0.5f, 1f);

                    //var scrollbar = Object.Instantiate(Object.FindObjectOfType<Scrollbar>(true).gameObject, scrollView.transform);
                    //var bar = scrollbar.GetComponent<Scrollbar>();

                    //scrollRect.verticalScrollbar = bar;
                }
            }
        }

        [HarmonyPatch(typeof(GlossaryListUIPanel), nameof(GlossaryListUIPanel.TryInitializeField))]
        [HarmonyPrefix]
        private static void AddExtraFieldIconsIfNeeded(GlossaryListUIPanel __instance, SlotStatusEffectInfoSO[] fields)
        {
            if (!__instance._initialized)
            {
                if(fields.Length > __instance._icons.Length)
                {
                    var l = __instance._icons.ToList();
                    var toadd = fields.Length - __instance._icons.Length;
                    for (int i = 0; i < toadd; i++)
                    {
                        l.Add(Object.Instantiate(l[0], l[0].transform.parent));
                    }
                    __instance._icons = l.ToArray();
                    (__instance.transform as RectTransform).sizeDelta = new(0f, 284f + 105f * Mathf.Max(Mathf.CeilToInt(l.Count / 8f) - 3, 0));
                }

                if (__instance.GetComponent<HorizontalLayoutGroup>() != null)
                {
                    Object.DestroyImmediate(__instance.GetComponent<HorizontalLayoutGroup>());
                    var grid = __instance.gameObject.AddComponent<GridLayoutGroup>();
                    grid.cellSize = new(118.5f, 105f);
                }

                if (__instance.transform.parent != null && (__instance.transform.parent.parent == null || __instance.transform.parent.parent.GetComponent<ScrollRect>() == null))
                {
                    var y = -50f;

                    __instance.transform.parent.Find("FieldTitleText (TMP)").localPosition = new(25f, y);

                    var scrollView = new GameObject("Scroll View");
                    scrollView.AddComponent<RectTransform>();
                    scrollView.transform.SetParent(__instance.transform.parent, false);

                    var viewport = new GameObject("Viewport");
                    var viewportTransform = viewport.AddComponent<RectTransform>();
                    viewport.transform.SetParent(scrollView.transform, false);
                    viewport.AddComponent<Image>();
                    viewport.AddComponent<Mask>().showMaskGraphic = false;
                    viewportTransform.sizeDelta = new(1000, 315);

                    __instance.transform.SetParent(viewport.transform, false);

                    var scrollimg = scrollView.AddComponent<Image>();
                    var tex = LoadTexture("UINineSliceTest_3");
                    scrollimg.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));
                    scrollimg.pixelsPerUnitMultiplier = 0.05f;
                    scrollimg.type = Image.Type.Sliced;
                    var scrollRect = scrollView.AddComponent<ScrollRect>();
                    scrollRect.content = __instance.transform as RectTransform;
                    scrollRect.viewport = viewport.transform as RectTransform;
                    scrollRect.horizontal = false;
                    scrollRect.vertical = true;
                    scrollRect.movementType = ScrollRect.MovementType.Elastic;
                    scrollRect.transform.localPosition = new(-28f, y - 50f);
                    (scrollRect.transform as RectTransform).sizeDelta = new(1025f, 340f);
                    (scrollRect.transform as RectTransform).pivot = new(0.5f, 1f);
                    scrollRect.scrollSensitivity = 10f;
                    scrollRect.movementType = ScrollRect.MovementType.Clamped;

                    (__instance.transform as RectTransform).pivot = new(0.5f, 1f);

                    //var scrollbar = Object.Instantiate(Object.FindObjectOfType<Scrollbar>(true).gameObject, scrollView.transform);
                    //var bar = scrollbar.GetComponent<Scrollbar>();

                    //scrollRect.verticalScrollbar = bar;
                }
            }
        }

        [HarmonyPatch(typeof(GlossaryListUIPanel), nameof(GlossaryListUIPanel.TryInitializePassive))]
        [HarmonyPrefix]
        private static void AddExtraPassiveIconsIfNeeded(GlossaryListUIPanel __instance, GlossaryPassives[] passives)
        {
            if (!__instance._initialized && passives.Length > __instance._icons.Length)
            {
                var l = __instance._icons.ToList();
                var toadd = passives.Length - __instance._icons.Length;
                for (int i = 0; i < toadd; i++)
                {
                    l.Add(Object.Instantiate(l[l.Count - 1], l[l.Count - 1].transform.parent));
                }
                __instance._icons = l.ToArray();
                RectTransform iconzone;
                if (__instance.transform.Find("Scroll View") == null)
                {
                    iconzone = (__instance.transform.Find("IconZone") as RectTransform);
                }
                else
                {
                    iconzone = (__instance.transform.transform.Find("Scroll View").Find("Viewport").Find("IconZone") as RectTransform);
                }
                iconzone.sizeDelta = new(0, 515f + 81.5f * Mathf.Max(Mathf.CeilToInt(l.Count / 9f) - 10, 0));
            }

            if (__instance.transform.Find("Scroll View") == null)
            {
                var y = 410f;

                __instance.transform.Find("TitleText (TMP)").localPosition = new(25f, y);

                var iconzone = __instance.transform.Find("IconZone");

                var scrollView = new GameObject("Scroll View");
                scrollView.AddComponent<RectTransform>();
                scrollView.transform.SetParent(__instance.transform, false);

                var viewport = new GameObject("Viewport");
                var viewportTransform = viewport.AddComponent<RectTransform>();
                viewport.transform.SetParent(scrollView.transform, false);
                viewport.AddComponent<Image>();
                viewport.AddComponent<Mask>().showMaskGraphic = false;
                viewportTransform.sizeDelta = new(1000f, 750f);

                iconzone.SetParent(viewport.transform, false);

                var scrollimg = scrollView.AddComponent<Image>();
                var tex = LoadTexture("UINineSliceTest_3");
                scrollimg.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));
                scrollimg.pixelsPerUnitMultiplier = 0.05f;
                scrollimg.type = Image.Type.Sliced;
                var scrollRect = scrollView.AddComponent<ScrollRect>();
                scrollRect.content = iconzone as RectTransform;
                scrollRect.viewport = viewport.transform as RectTransform;
                scrollRect.horizontal = false;
                scrollRect.vertical = true;
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
                scrollRect.transform.localPosition = new(-28, y - 50f);
                (scrollRect.transform as RectTransform).sizeDelta = new(1025f, 800f);
                (scrollRect.transform as RectTransform).pivot = new(0.5f, 1f);
                scrollRect.scrollSensitivity = 10f;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;

                (iconzone as RectTransform).pivot = new(0.5f, 1f);

                Object.DestroyImmediate(iconzone.GetComponent<VerticalLayoutGroup>());
                var grid = iconzone.gameObject.AddComponent<GridLayoutGroup>();
                grid.cellSize = new(81.5f, 81.5f);
                grid.spacing = new(25.2f, 0f);

                List<Transform> children = new();
                List<Transform> iconGroups = new();

                for(int i = 0; i < iconzone.childCount; i++)
                {
                    for(int j = 0; j < iconzone.GetChild(i).childCount; j++)
                    {
                        children.Add(iconzone.GetChild(i).GetChild(j));
                    }
                    iconGroups.Add(iconzone.GetChild(i));
                }

                foreach(var child in children)
                {
                    child.transform.SetParent(iconzone, false);
                }

                foreach (var child in iconGroups)
                {
                    Object.Destroy(child.gameObject);
                }


                //var scrollbar = Object.Instantiate(Object.FindObjectOfType<Scrollbar>(true).gameObject, scrollView.transform);
                //var bar = scrollbar.GetComponent<Scrollbar>();

                //scrollRect.verticalScrollbar = bar;
            }
        }

        [HarmonyPatch(typeof(GlossaryListUIPanel), nameof(GlossaryListUIPanel.TryInitializeKeyword))]
        [HarmonyPrefix]
        private static void AddExtraKeywordIconsIfNeeded(GlossaryListUIPanel __instance, GlossaryKeywords[] keywords)
        {
            if (!__instance._initialized && keywords.Length > __instance._icons.Length)
            {
                var l = __instance._icons.ToList();
                var toadd = keywords.Length - __instance._icons.Length;
                for (int i = 0; i < toadd; i++)
                {
                    l.Add(Object.Instantiate(l[0], l[0].transform.parent));
                }
                __instance._icons = l.ToArray();
                RectTransform iconzone;
                if (__instance.transform.Find("Scroll View") == null)
                {
                    iconzone = (__instance.transform.Find("IconZone") as RectTransform);
                }
                else
                {
                    iconzone = (__instance.transform.transform.Find("Scroll View").Find("Viewport").Find("IconZone") as RectTransform);
                }
                iconzone.sizeDelta = new(0, 372 + 56.1f * Mathf.Max(l.Count - 14, 0));
            }

            if (__instance.transform.Find("Scroll View") == null)
            {
                var y = 410f;

                __instance.transform.Find("TitleText (TMP)").localPosition = new(25f, y);

                var iconzone = __instance.transform.Find("IconZone");

                var scrollView = new GameObject("Scroll View");
                scrollView.AddComponent<RectTransform>();
                scrollView.transform.SetParent(__instance.transform, false);

                var viewport = new GameObject("Viewport");
                var viewportTransform = viewport.AddComponent<RectTransform>();
                viewport.transform.SetParent(scrollView.transform, false);
                viewport.AddComponent<Image>();
                viewport.AddComponent<Mask>().showMaskGraphic = false;
                viewportTransform.sizeDelta = new(1000f, 750f);

                iconzone.SetParent(viewport.transform, false);

                var scrollimg = scrollView.AddComponent<Image>();
                var tex = LoadTexture("UINineSliceTest_3");
                scrollimg.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.Tight, new(9, 9, 9, 9));
                scrollimg.pixelsPerUnitMultiplier = 0.05f;
                scrollimg.type = Image.Type.Sliced;
                var scrollRect = scrollView.AddComponent<ScrollRect>();
                scrollRect.content = iconzone as RectTransform;
                scrollRect.viewport = viewport.transform as RectTransform;
                scrollRect.horizontal = false;
                scrollRect.vertical = true;
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
                scrollRect.transform.localPosition = new(-28, y - 50f);
                (scrollRect.transform as RectTransform).sizeDelta = new(1025f, 800f);
                (scrollRect.transform as RectTransform).pivot = new(0.5f, 1f);
                scrollRect.scrollSensitivity = 10f;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;

                (iconzone as RectTransform).pivot = new(0.5f, 1f);

                iconzone.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
                iconzone.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
                iconzone.GetComponent<VerticalLayoutGroup>().childScaleHeight = false;

                for (int i = 0; i < iconzone.childCount; i++)
                {
                    (iconzone.GetChild(i) as RectTransform).sizeDelta = new(0f, 56.1f);
                }


                //var scrollbar = Object.Instantiate(Object.FindObjectOfType<Scrollbar>(true).gameObject, scrollView.transform);
                //var bar = scrollbar.GetComponent<Scrollbar>();

                //scrollRect.verticalScrollbar = bar;
            }
        }

        public static void AddStatus(StatusEffectInfoSO info)
        {
            if(glossaryDB != null)
            {
                glossaryDB._status = glossaryDB._status.AddToArray(info);
            }
            else
            {
                queuedStatus.Add(info);
            }
        }

        public static void AddField(SlotStatusEffectInfoSO info)
        {
            if (glossaryDB != null)
            {
                glossaryDB._fields = glossaryDB._fields.AddToArray(info);
            }
            else
            {
                queuedField.Add(info);
            }
        }

        public static void AddKeyword(string kwName, string description)
        {
            var kw = new GlossaryKeywords()
            {
                _keyword = kwName,
                _description = description,
                glossaryID = (GlossaryLocID)(-1)
            };
            if (glossaryDB != null)
            {
                glossaryDB._keywords = glossaryDB._keywords.AddToArray(kw);
            }
            else
            {
                queuedKeywords.Add(kw);
            }
        }

        public static void AddPassive(string name, string description, string sprite)
        {
            var pas = new GlossaryPassives()
            {
                _name = name,
                _description = description,
                sprite = LoadSprite(sprite),
                glossaryID = (GlossaryLocID)(-1)
            };
            if (glossaryDB != null)
            {
                glossaryDB._passives = glossaryDB._passives.AddToArray(pas);
            }
            else
            {
                queuedPassives.Add(pas);
            }
        }
    }
}
