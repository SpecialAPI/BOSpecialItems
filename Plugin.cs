global using BepInEx;
global using BrutalAPI;
global using UnityEngine;
global using System.Reflection;
global using HarmonyLib;
global using System.Collections.Generic;
global using System.Reflection.Emit;
global using System.Collections;
global using System.Linq;

global using BrutalAPIPlugin = BrutalAPI.BrutalAPI;
global using Object = UnityEngine.Object;
global using Random = UnityEngine.Random;
global using Debug = UnityEngine.Debug;
global using Passives = BrutalAPI.Passives;

global using BOSpecialItems.Content;
global using BOSpecialItems.Content.Additional;
global using BOSpecialItems.Content.Conditions;
global using BOSpecialItems.Content.Effects;
global using BOSpecialItems.Content.Items;
global using BOSpecialItems.Content.Items.PassiveFlags;
global using BOSpecialItems.Content.Items.Wearables;
global using BOSpecialItems.Content.Passive;
global using BOSpecialItems.Content.Status;
global using BOSpecialItems.Content.Extension;

global using static BOSpecialItems.Tools;
global using static BOSpecialItems.Content.Extension.EnumExtension;

using System;
using FMODUnity;

namespace BOSpecialItems
{
    [BepInPlugin(GUID, "SpecialAPI's Stuff Pack", "1.0.1")]
    [BepInDependency("Bones404.BrutalAPI", BepInDependency.DependencyFlags.HardDependency)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "SpecialAPI.BOSpecialItems";

        public static ZoneBGDataBaseSO z1;
        public static ZoneBGDataBaseSO z2;
        public static ZoneBGDataBaseSO z3;
        public static ZoneBGDataBaseSO z1_hard;
        public static ZoneBGDataBaseSO z2_hard;
        public static ZoneBGDataBaseSO z3_hard;

        public void Awake()
        {
            SpecialItemsAssembly = Assembly.GetExecutingAssembly();

            LoadFMODBankFromResource("BOSpecialItems");
            LoadFMODBankFromResource("BOSpecialItems.strings");

            z1 = LoadedAssetsHandler.GetZoneDB("ZoneDB_01") as ZoneBGDataBaseSO;
            z2 = LoadedAssetsHandler.GetZoneDB("ZoneDB_02") as ZoneBGDataBaseSO;
            z3 = LoadedAssetsHandler.GetZoneDB("ZoneDB_03") as ZoneBGDataBaseSO;
            z1_hard = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_01") as ZoneBGDataBaseSO;
            z2_hard = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_02") as ZoneBGDataBaseSO;
            z3_hard = LoadedAssetsHandler.GetZoneDB("ZoneDB_Hard_03") as ZoneBGDataBaseSO;

            /*testzone = new()
            {
                _baseCombatEnvironment = z2._baseCombatEnvironment,
                _baseOWEnvironment = z3._baseOWEnvironment,
                _bossBundleSelector = z1_hard._bossBundleSelector,
                _combatAmbience = AmbienceType.Secret,
                _deckInfo = z2_hard._deckInfo,
                _easyEnemyBundleSelector = z3_hard._easyEnemyBundleSelector,
                _encounterLevelRank = 3,
                _FlavourPool = z1_hard._FlavourPool,
                _foolsRank = 2,
                _foolsRoom = z2._foolsRoom,
                _foolsSignType = SignType.Admo,
                _FreeFoolsPool = z3_hard._FreeFoolsPool,
                _gameDB = z1_hard._gameDB,
                _hardEnemyBundleSelector = z2_hard._hardEnemyBundleSelector,
                _itemRoom = z2_hard._itemRoom,
                _maxLevelUpRank = 3,
                _itemSignType = SignType.Psaltery,
                _mediumEnemyBundleSelector = z1_hard._mediumEnemyBundleSelector,
                _maxMoneyChestAmount = 99,
                _minMoneyChestAmount = 1,
                _moneyChestRoom = z1._moneyChestRoom,
                _moneyChestSignType = SignType.PitifulCorpse_BOSS,
                _omittedCharacters = z1_hard._omittedCharacters,
                _OmittedCharacters = z1_hard._OmittedCharacters,
                _overworldMusicEvent = z2._overworldMusicEvent,
                _poolDB = z1_hard._poolDB,
                _PrizesInRun = z1_hard._PrizesInRun,
                _EntitiesInRun = z1_hard._EntitiesInRun,
                _QuestPool = z1_hard._QuestPool,
                _restAmbience = z3_hard._restAmbience,
                _ShopItemsInRun = z1_hard._ShopItemsInRun,
                _shopRoom = z1_hard._shopRoom,
                _shopSignType = SignType.CranesCorpse,
                _specialEnemyBundleSelector = z1_hard._specialEnemyBundleSelector,
                _SpecialQuestPool = z1_hard._SpecialQuestPool,
                _transitionAmbience = z2_hard._transitionAmbience,
                _uncheckedCharacters = z2_hard._uncheckedCharacters,
                _uncheckedFlavours = z1_hard._uncheckedFlavours,
                _uncheckedFreeFools = z3_hard._uncheckedFreeFools,
                _uncheckedQuests = z1_hard._uncheckedQuests,
                _zoneData = z1_hard._zoneData,
                _zoneLootCalculator = z3_hard._zoneLootCalculator,
                _zoneName = ZoneType.FarShore
            };*/

            ChangeHealthColorButtonHolder.disabledSprite = LoadSprite("UI_ManaToggle_Disabled", 32);

            new Harmony(GUID).PatchAll();

            GenericStatusEffect.AddNewGenericEffect<TargetSwapStatusEffect>("TargetSwap", "While TargetSwapped all abilities are performed as if the caster is on the space directly opposing them. Instant kills or fleeing effects are not affected by this.\n1 point of TargetSwap is lost at the end of each turn.", "TargetSwap", "event:/TargetSwapApply");
            GenericStatusEffect.AddNewGenericEffect<BerserkStatusEffect>("Berserk", "Deal double damage.\n1 point of Berserk is lost at the end of each turn.", "Berserk", "event:/FuryApply");
            GenericStatusEffect.AddNewGenericEffect<FuryStatusEffect>("Fury", "When performing an ability, perform it again and reduce Fury by 1 for each point of Fury.\n1 point of Fury is lost at the end of each turn.", "Fury", "event:/FuryApply");
            GenericStatusEffect.AddNewGenericEffect<WeakenedStatusEffect>("Weakened", "Weakened party members are 1 level lower than they would be otherwise for each point of Weakened.\nDamage dealt by Weakened enemies is multiplied by 0.85 for each point of Weakened.\n1 point of Weakened is lost at the end of each turn.", "Weaken", "event:/WeakenApply");
            GenericStatusEffect.AddNewGenericEffect<SurviveStatusEffect>("Survive", "Survive 1 fatal hit for each point of Survive.", "Survive", "event:/Combat/StatusEffects/SE_Divine_Apl");
            GenericStatusEffect.AddNewGenericEffect<PoweredUpStatusEffect>("Powered Up", "Powered Up party members are 1 level higher than they would be otherwise for each point of Powered Up.\nDamage dealt by Powered Up enemies is increased by 25% for each point of Powered Up.\n1 point of Powered Up is lost at the end of each turn.", "PoweredUp", "event:/Combat/StatusEffects/SE_Divine_Apl");

            GadgetDB.Init();

            MergeEnemiesEffect.SetupMergedPassive();

            Retargetter.Init();
            //Converter.Init(); //scrapped (for now at least)
            FailedRound.Init();
            JesterHat.Init();
            TheTiderunner.Init();
            Bleach.Init();
            CombatDice.Init();
            LoudPhone.Init();
            WorldShatter.Init();
            SilverMirror.Init();
            Potential.Init();
            Survivorship.Init();
            TheSquirrel.Init();
            BloodyHacksaw.Init();
            UntetheredHealthItem.Init();

            GlossaryStuffAdder.AddPassive("TargetShift", "All abilities performed by this party member/enemy are performed as if the caster is on the space to the right/left/far right/far left of them.", "TargetShift");
            GlossaryStuffAdder.AddPassive("Pigment Core", "Unlocks the ability to change the colour of this party member's/enemy's health color through a button to the right of it's health bar.", "UntetheredHealthColor");
            GlossaryStuffAdder.AddPassive("Merged", "This enemy will perform an additional ability for each enemy merged into it.", "Merged");

            GlossaryStuffAdder.AddKeyword("Dry Damage", "Dry damage is direct damage that doesn't generate pigment.");
            GlossaryStuffAdder.AddKeyword("Reliable Damage", "Reliable damage always deals the same amount of damage, regardless of any passives, items, status effects or field effects. It will still \"trigger\" effects that would normally modify damage dealt, such as reducing frail and shield or dealing damage to other enemies if the target has divine protection.");
        }
    }
}
