using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine.UI;

namespace BOSpecialItems.Content.Challenges.Setup
{
    [HarmonyPatch]
    public static class ChallengePatches
    {
        [HarmonyPatch(typeof(MainMenuController), nameof(MainMenuController.Start))]
        [HarmonyPostfix]
        public static void MenuSetup(MainMenuController __instance)
        {
            if(__instance._charSelection != null)
            {
                /*var challengeRunMenu = Object.Instantiate(__instance._charSelection.gameObject, __instance._charSelection.transform.parent);
                challengeRunMenu.SetActive(false);
                var menuController = challengeRunMenu.AddComponent<ChallengeMenuController>();
                menuController.chars = __instance._charSelectionDB;

                var challengeList = new GameObject("ChallengeList");
                challengeList.AddComponent<VerticalLayoutGroup>();
                var iconzone = challengeList.GetComponent<RectTransform>();

                var scrollView = new GameObject("Scroll View");
                scrollView.AddComponent<RectTransform>();
                scrollView.transform.SetParent(challengeRunMenu.transform, false);

                var viewport = new GameObject("Viewport");
                var viewportTransform = viewport.AddComponent<RectTransform>();
                viewport.transform.SetParent(scrollView.transform, false);
                viewport.AddComponent<Image>();
                viewport.AddComponent<Mask>().showMaskGraphic = false;
                viewportTransform.sizeDelta = new(1000f, 750f);

                iconzone.SetParent(viewport.transform, false);

                var scrollimg = scrollView.AddComponent<Image>();
                scrollimg.sprite = UISprites.NineSlice_Purple;
                scrollimg.pixelsPerUnitMultiplier = 0.05f;
                scrollimg.type = Image.Type.Sliced;
                var scrollRect = scrollView.AddComponent<ScrollRect>();
                scrollRect.content = iconzone;
                scrollRect.viewport = viewport.transform as RectTransform;
                scrollRect.horizontal = false;
                scrollRect.vertical = true;
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
                scrollRect.transform.localPosition = new(-525f, 400f);
                (scrollRect.transform as RectTransform).sizeDelta = new(500f, 800f);
                (scrollRect.transform as RectTransform).pivot = new(0.5f, 1f);
                scrollRect.scrollSensitivity = 10f;
                scrollRect.movementType = ScrollRect.MovementType.Clamped;

                iconzone.pivot = new(0.5f, 1f);
                iconzone.sizeDelta = new(450f, 100f);

                iconzone.GetComponent<VerticalLayoutGroup>().childControlHeight = false;
                iconzone.GetComponent<VerticalLayoutGroup>().childForceExpandHeight = false;
                iconzone.GetComponent<VerticalLayoutGroup>().childScaleHeight = false;

                foreach(var ch in ChallengeDB.Challenges.Values)
                {
                    var img = new GameObject("ChallengeButton");
                    img.transform.SetParent(iconzone, false);

                    var im = img.AddComponent<Image>();
                    im.sprite = UISprites.NineSlice_Purple;
                    im.pixelsPerUnitMultiplier = 0.05f;
                    im.type = Image.Type.Sliced;

                    var btn = img.AddComponent<Button>();
                    btn.onClick.AddListener(() => menuController.OnChallengeSelected(ch));

                    var txt = new GameObject("Text");
                    txt.transform.SetParent(img.transform, false);

                    var tx = txt.AddComponent<TextMeshProUGUI>();
                    tx.text = ch.Name;
                    tx.font = fnt;
                    tx.alignment = TextAlignmentOptions.Center;
                    tx.fontSize = 36f;
                    (txt.transform as RectTransform).sizeDelta = new(1000f, 100f);
                }

                var rulesWindow = new GameObject("RuleWindow");
                var ruleimg = rulesWindow.AddComponent<Image>();
                ruleimg.sprite = UISprites.NineSlice_Purple;
                ruleimg.pixelsPerUnitMultiplier = 0.05f;
                ruleimg.type = Image.Type.Sliced;
                var ruletransform = rulesWindow.GetComponent<RectTransform>();
                ruletransform.SetParent(challengeRunMenu.transform, false);
                ruletransform.localPosition = new(300f, -50f);
                ruletransform.sizeDelta = new(1000f, 500f);

                var rulesText = new GameObject("Text");
                rulesText.transform.SetParent(rulesWindow.transform, false);
                var rltxt = rulesText.AddComponent<TextMeshProUGUI>();
                rltxt.text = "";
                rltxt.font = fnt;
                rltxt.alignment = TextAlignmentOptions.Center;
                (rulesText.transform as RectTransform).sizeDelta = new(1000f, 500f);

                menuController.rulesText = rltxt;

                var charactersWindow = new GameObject("CharactersWindow");
                var charactersimg = charactersWindow.AddComponent<Image>();
                charactersimg.sprite = UISprites.NineSlice_Purple;
                charactersimg.pixelsPerUnitMultiplier = 0.05f;
                charactersimg.type = Image.Type.Sliced;
                var characterstransform = charactersWindow.GetComponent<RectTransform>();
                characterstransform.SetParent(challengeRunMenu.transform, false);
                characterstransform.localPosition = new(50f, 300f);
                characterstransform.sizeDelta = new(500f, 200f);

                var charactersGroup = new GameObject("CharactersGroup");
                var cgroupTransform = charactersGroup.AddComponent<RectTransform>();
                cgroupTransform.SetParent(charactersWindow.transform, false);
                menuController.charactersTransform = cgroupTransform;
                //cgroupTransform.sizeDelta = new(500f, 200f);
                var clayout = charactersGroup.AddComponent<HorizontalLayoutGroup>();
                clayout.childControlHeight = true;
                clayout.childControlWidth = true;
                clayout.childForceExpandHeight = true;
                clayout.childForceExpandWidth = true;
                clayout.childScaleHeight = false;
                clayout.childScaleWidth = false;
                clayout.childAlignment = TextAnchor.MiddleCenter;

                var itemsWindow = new GameObject("ItemsWindow");
                var itemsimg = itemsWindow.AddComponent<Image>();
                itemsimg.sprite = UISprites.NineSlice_Purple;
                itemsimg.pixelsPerUnitMultiplier = 0.05f;
                itemsimg.type = Image.Type.Sliced;
                var itemstransform = itemsWindow.GetComponent<RectTransform>();
                itemstransform.SetParent(challengeRunMenu.transform, false);
                itemstransform.localPosition = new(550f, 300f);
                itemstransform.sizeDelta = new(500f, 200f);
                var charactersLayout = charactersWindow.AddComponent<HorizontalLayoutGroup>();

                var itemsGroup = new GameObject("ItemsGroup");
                var igroupTransform = itemsGroup.AddComponent<RectTransform>();
                igroupTransform.SetParent(itemsWindow.transform, false);
                menuController.itemsTransform = igroupTransform;
                //igroupTransform.sizeDelta = new(500f, 200f);
                var ilayout = itemsGroup.AddComponent<HorizontalLayoutGroup>();
                ilayout.childControlHeight = true;
                ilayout.childControlWidth = true;
                ilayout.childForceExpandHeight = true;
                ilayout.childForceExpandWidth = true;
                ilayout.childScaleHeight = false;
                ilayout.childScaleWidth = false;
                ilayout.childAlignment = TextAnchor.MiddleCenter;

                var startChallengeButton = new GameObject("StartChallengeButton");
                var btimg = startChallengeButton.AddComponent<Image>();
                btimg.sprite = UISprites.NineSlice_Colored;
                btimg.pixelsPerUnitMultiplier = 0.05f;
                btimg.type = Image.Type.Sliced;
                btimg.color = new(1f, 0.5f, 0f);
                var bttransform = startChallengeButton.GetComponent<RectTransform>();
                bttransform.SetParent(challengeRunMenu.transform, false);
                bttransform.localPosition = new(300f, -350f);
                bttransform.sizeDelta = new(1000f, 100f);

                var buttonText = new GameObject("Text");
                buttonText.transform.SetParent(startChallengeButton.transform, false);
                var btntxt = buttonText.AddComponent<TextMeshProUGUI>();
                btntxt.text = "Start Challenge";
                btntxt.color = new(1f, 1f, 0f);
                btntxt.font = fnt;
                btntxt.alignment = TextAlignmentOptions.Center;
                btntxt.fontSize = 72f;
                (buttonText.transform as RectTransform).sizeDelta = new(1000f, 100f);*/

                var obj = Bundle.LoadAsset<GameObject>("assets/challengeselectionhandler.prefab");
                var challengeRunMenu = Object.Instantiate(obj, __instance._charSelection.transform.parent);

                challengeRunMenu.SetActive(false);

                challengeRunMenu.transform.SetSiblingIndex(__instance._charSelection.transform.GetSiblingIndex() + 1);

                var fnt = __instance._charSelection.GetComponentInChildren<TMP_Text>().font;
                foreach(var txt in challengeRunMenu.GetComponentsInChildren<TMP_Text>(true))
                {
                    txt.font = fnt;
                }

                var menuController = challengeRunMenu.AddComponent<ChallengeMenuController>();
                menuController.chars = __instance._charSelectionDB;

                var panels = challengeRunMenu.transform.Find("PanelHolder").Find("ChallengesTab");
                menuController.rulesText = panels.Find("RulesContainer").Find("Text (TMP)").GetComponent<TMP_Text>();
                menuController.charactersTransform = panels.Find("CharactersContainer").Find("ScrollView").Find("Viewport").Find("Content");
                menuController.itemsTransform = panels.Find("ItemsContainer").Find("ScrollView").Find("Viewport").Find("Content");
                menuController.charactersScroll = panels.Find("CharactersContainer").Find("ScrollView").GetComponent<ScrollRect>();
                menuController.itemsScroll = panels.Find("ItemsContainer").Find("ScrollView").GetComponent<ScrollRect>();

                menuController.openCloseEvent = __instance._charSelection._openCloseEvent;
                menuController.selectedEvent = __instance._charSelection._characterClickEvent;
                menuController.startRunEvent = __instance._startRunEvent;
                
                menuController.saveDataHandler = __instance._saveDataHandler;
                menuController.informationHolder = __instance._informationHolder;
                menuController.menu = __instance;

                panels.gameObject.SetActive(true);

                var charSelectPanels = challengeRunMenu.transform.Find("PanelHolder").Find("CharacterSelection");

                charSelectPanels.gameObject.SetActive(false);

                menuController.challengesMenu = panels.gameObject;
                menuController.charSelectMenu = charSelectPanels.gameObject;

                var challengesTransform = panels.Find("ChallengeList").Find("ScrollView").Find("Viewport").Find("Content");
                for(int i = 0; i < challengesTransform.childCount; i++)
                {
                    challengesTransform.GetChild(i).gameObject.SetActive(false);
                }

                var challenges = ChallengeDB.Challenges.Values.ToList();
                for(int i = 0; i < challenges.Count; i++)
                {
                    Transform c = null;
                    var challenge = challenges[i];

                    if (i < challengesTransform.childCount)
                    {
                        c = challengesTransform.GetChild(i);
                    }

                    if (c == null)
                    {
                        c = Object.Instantiate(challengesTransform.GetChild(0).gameObject, challengesTransform).transform;
                    }

                    c.gameObject.SetActive(true);
                    c.GetComponentInChildren<TMP_Text>().text = challenge.Name;

                    c.GetComponentInChildren<Button>().onClick.AddListener(() => menuController.OnChallengeSelected(challenge));
                }

                var returnButton = panels.Find("ReturnButton").GetComponent<Button>();
                returnButton.onClick.AddListener(() => menuController.Activation(false));

                var startButton = panels.Find("StartChallengeButton").GetComponent<Button>();
                startButton.onClick.AddListener(menuController.OnStartChallengePressed);

                menuController.startChallengeButton = startButton;
                menuController.buttonTextActive = startButton.transform.Find("ActiveText").GetComponent<TMP_Text>();
                menuController.buttonTextInactive = startButton.transform.Find("InactiveText").GetComponent<TMP_Text>();

                var charPanels = __instance._charSelection.transform.Find("PanelHolder");

                var selectionPart = charSelectPanels.Find("CharactersContainer").GetComponent<RectTransform>();
                selectionPart.localPosition = new(-390f, 300f, 0f);

                var characters = Object.Instantiate(charPanels.Find("Characters"), charSelectPanels).GetComponent<RectTransform>();
                characters.localPosition = new(275f, 280f, 0f);
                characters.sizeDelta = new(650f, 235f);
                characters.GetComponentInChildren<TMP_Text>().text = "Choose a Companion";

                var information = Object.Instantiate(charPanels.Find("CharacterInformation"), charSelectPanels).GetComponent<RectTransform>();
                information.localPosition = new(0f, -100f, 0f);
                information.sizeDelta = new(1250f, 350f);


                var noInformation = Object.Instantiate(charPanels.Find("CharacterNoInformation"), charSelectPanels).GetComponent<RectTransform>();
                noInformation.localPosition = new(0f, -100f, 0f);
                noInformation.sizeDelta = new(1250f, 350f);

                var backButton = charSelectPanels.Find("ReturnButton").GetComponent<Button>();
                backButton.onClick.AddListener(menuController.CloseCharSelect);

                if (__instance._basicMenuUI.activeSelf)
                {
                    var highestY = float.MinValue;
                    var buttons = __instance._basicMenuUI.transform;
                    var runStart = buttons.Find("StartButton");

                    if (runStart != null)
                    {
                        for (int i = 0; i < buttons.childCount; i++)
                        {
                            var c = buttons.GetChild(i);

                            if (c == null || !c.gameObject.activeSelf)
                                continue;

                            highestY = Mathf.Max(highestY, c.localPosition.y);
                        }

                        var challengeButton = Object.Instantiate(runStart.gameObject, buttons);
                        challengeButton.transform.localPosition = new Vector3(runStart.localPosition.x, highestY > float.MinValue ? highestY + 60f : 0f, runStart.localPosition.z);
                        challengeButton.GetComponentInChildren<TMP_Text>().text = "Challenge Run";

                        var btn = challengeButton.GetComponentInChildren<Button>();
                        btn.onClick.m_PersistentCalls = new();
                        btn.onClick.AddListener(() => menuController.Activation(true));
                    }
                }
            }
        }
    }
}
