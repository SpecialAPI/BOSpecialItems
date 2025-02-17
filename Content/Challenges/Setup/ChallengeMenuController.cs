using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Tools;
using UnityEngine.UI;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public class ChallengeMenuController : MonoBehaviour, IMenuControlable
    {
        public void OnChallengeSelected(ChallengeBase challenge)
        {
            activeChallenge = challenge;

            startChallengeButton.interactable = challenge != null;
            buttonTextActive.gameObject.SetActive(challenge != null);
            buttonTextInactive.gameObject.SetActive(challenge == null);

            RuntimeManager.PlayOneShot(selectedEvent);

            if(challenge != null )
            {
                rulesText.text = challenge.RulesText;
            }
            else
            {
                rulesText.text = "";
            }

            for(int i = 0; i < itemsTransform.childCount; i++)
            {
                var c = itemsTransform.GetChild(i);
                c.gameObject.SetActive(false);
            }

            for (int i = 0; i < charactersTransform.childCount; i++)
            {
                var c = charactersTransform.GetChild(i);
                c.gameObject.SetActive(false);
            }

            if(challenge != null)
            {
                var itm = challenge.StartingItems;
                if (itm != null)
                {
                    for (int i = 0; i < itm.Length; i++)
                    {
                        Transform c = null;

                        if (i < itemsTransform.childCount)
                        {
                            c = itemsTransform.GetChild(i);
                        }

                        if (c == null)
                        {
                            c = Instantiate(itemsTransform.GetChild(0).gameObject, itemsTransform).transform;
                        }

                        c.gameObject.SetActive(true);
                        c.GetComponent<Image>().sprite = LoadedAssetsHandler.GetWearable(itm[i]).wearableImage;
                    }
                }

                var chr = challenge.StartingCharacters;
                if (chr != null)
                {
                    for (int i = 0; i < chr.Length; i++)
                    {
                        Transform c = null;

                        if (i < charactersTransform.childCount)
                        {
                            c = charactersTransform.GetChild(i);
                        }

                        if (c == null)
                        {
                            c = Instantiate(charactersTransform.GetChild(0).gameObject, charactersTransform).transform;
                        }

                        c.gameObject.SetActive(true);
                        c.GetComponent<Image>().sprite = chr[i].Selector.GetImage();
                    }
                }
            }

            StartCoroutine(DelayedPositionFix());
        }

        public IEnumerator DelayedPositionFix()
        {
            yield return null;

            itemsScroll.horizontalNormalizedPosition = 0f;
            charactersScroll.horizontalNormalizedPosition = 0f;
        }

        public void Activation(bool enabled)
        {
            RuntimeManager.PlayOneShot(openCloseEvent);
            gameObject.SetActive(enabled);
            SetMenuControl(enabled);

            if (enabled)
            {
                charSelectMenu.SetActive(false);
                challengesMenu.SetActive(true);
            }
        }

        public void OnEscapePressed(bool close = true)
        {
            if (gameObject.activeInHierarchy)
            {
                if (charSelectMenu.activeSelf)
                {
                    RuntimeManager.PlayOneShot(openCloseEvent);

                    charSelectMenu.SetActive(false);
                    challengesMenu.SetActive(true);
                }
                else
                {
                    Activation(false);
                }
            }
        }

        public void OnStartChallengePressed()
        {
            if(!occupied && activeChallenge != null)
            {
                if(activeChallenge.StartingCharacters != null && activeChallenge.StartingCharacters.Any(x => x != null && x.Selector != null && x.Selector.IsSelectable))
                {
                    RuntimeManager.PlayOneShot(openCloseEvent);

                    charSelectMenu.SetActive(true);
                    challengesMenu.SetActive(false);

                    selectedCharacters = new int[activeChallenge.StartingCharacters.Length];
                    selectedIgnoredAbilities = new int[activeChallenge.StartingCharacters.Length];
                }
                else
                {
                    occupied = true;
                    selectedCharacters = new int[0];
                    selectedIgnoredAbilities = new int[0];

                    if (saveDataHandler.HasSavedRun && !saveDataHandler.RunCorrupted)
                    {
                        informationHolder.Game.SetIntData(DataUtils.winStreakVar, 0);
                    }
                    RuntimeManager.PlayOneShot(startRunEvent);
                    StartCoroutine(PrepareNewRunData(activeChallenge));
                }
            }
        }

        public void CloseCharSelect()
        {
            RuntimeManager.PlayOneShot(openCloseEvent);

            charSelectMenu.SetActive(false);
            challengesMenu.SetActive(true);
        }

        public void OnStartChallengeCharSelectPressed()
        {
            if (!occupied)
            {
                occupied = true;

                if (saveDataHandler.HasSavedRun && !saveDataHandler.RunCorrupted)
                {
                    informationHolder.Game.SetIntData(DataUtils.winStreakVar, 0);
                }

                RuntimeManager.PlayOneShot(startRunEvent);
                StartCoroutine(PrepareNewRunData(activeChallenge));
            }
        }

        public IEnumerator PrepareNewRunData(ChallengeBase challenge)
        {
            var run = ScriptableObject.CreateInstance<RunDataSO>();
            var startingChars = new List<InitialCharacter>();
            var currentChars = new List<CharacterSO>();

            var chs = challenge.StartingCharacters;
            if (chs != null)
            {

                var hasDPS = false;
                var hasSupport = false;

                for(int j = 0; j < 2; j++)
                {
                    for (int i = 0; i < chs.Length; i++)
                    {
                        var ch = chs[i];
                        var selectedIdx = selectedCharacters != null && i < selectedCharacters.Length ? selectedCharacters[i] : -1;

                        if (ch != null && ch.Selector != null && (ch.Selector.Deterministic(chars, selectedIdx) == (j == 0)))
                        {
                            var countAsDpsOrSupport = false;
                            var ignoredAbility = selectedIgnoredAbilities != null && i < selectedIgnoredAbilities.Length ? selectedIgnoredAbilities[i] : -1;

                            var resultChar = ch.Selector.GetCharacter(chars, selectedIdx, currentChars, hasDPS, hasSupport, ref countAsDpsOrSupport, ref ignoredAbility);

                            if (countAsDpsOrSupport)
                            {
                                hasDPS |= chars.IsCharacterDPS(resultChar, ignoredAbility);
                                hasSupport |= chars.IsCharacterSupport(resultChar, ignoredAbility);
                            }

                            currentChars.Add(resultChar);
                            startingChars.Add(new(resultChar, ch.Rank, ignoredAbility, ch.MainCharacter));
                        }
                    }
                }
            }

            if(startingChars.Count == 0)
            {
                startingChars.Add(new(LoadedAssetsHandler.GetCharcater("Nowak_CH"), 0, 0, true));
            }

            run.InitializeRun(informationHolder.Game, startingChars.ToArray(), informationHolder.GetZoneDBs());
            run.zoneLoadingType = ZoneLoadingType.ZoneStart;
            var tutorial = saveDataHandler.TutorialData;
            var tutorialOW = saveDataHandler.OWTutorialData;
            informationHolder.PrepareGameRun(run, tutorialOW, tutorial);

            var itm = challenge.StartingItems;
            if(itm != null)
            {
                foreach(var it in itm)
                {
                    if(it != null)
                    {
                        run.playerData.AddNewItem(LoadedAssetsHandler.GetWearable(it));
                    }
                }
            }

            run.playerData.AddCurrency(challenge.StartingMoney);

            yield return run.InitializeDataBase(informationHolder.Game, informationHolder.ItemPoolDB);

            menu.FinalizeMainMenuSounds();
            yield return menu.LoadNextScene(menu._owSceneToLoad);
        }

        public void SetMenuControl(bool enabled)
        {
            if (enabled)
            {
                MiscUtils.menuController?.AddOpenedMenu(this);
            }
            else
            {
                MiscUtils.menuController?.RemoveOpenedMenu(this);
            }
        }

        public TMP_Text rulesText;

        public Transform itemsTransform;
        public Transform charactersTransform;

        public ScrollRect itemsScroll;
        public ScrollRect charactersScroll;

        public SelectableCharactersSO chars;

        public ChallengeBase activeChallenge;

        public string openCloseEvent;
        public string selectedEvent;
        public string startRunEvent;

        public SaveDataHandler saveDataHandler;
        public GameInformationHolder informationHolder;
        public MainMenuController menu;

        public int[] selectedCharacters;
        public int[] selectedIgnoredAbilities;

        public GameObject challengesMenu;
        public GameObject charSelectMenu;

        public Button startChallengeButton;
        public TMP_Text buttonTextActive;
        public TMP_Text buttonTextInactive;

        public bool occupied;
    }
}
