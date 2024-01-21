global using BepInEx;
global using UnityEngine;
global using System.Reflection;
global using HarmonyLib;
global using System.Collections.Generic;
global using System.Reflection.Emit;
global using System.Collections;
global using System.Linq;
global using MUtility;

global using Object = UnityEngine.Object;
global using Random = UnityEngine.Random;
global using Debug = UnityEngine.Debug;

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

global using BOSpecialItems.Patches;

global using static BOSpecialItems.Tools;
global using static BOSpecialItems.Content.Extension.EnumExtension;
global using static BOSpecialItems.StoredValueAdder;
global using static BOSpecialItems.GlossaryStuffAdder;
global using static BOSpecialItems.IntentAdder;
global using static BOSpecialItems.Content.Status.GenericStatusEffectStaticMethods;

using System;
using FMODUnity;

namespace BOSpecialItems
{
    [BepInPlugin(GUID, "SpecialAPI's Stuff Pack", "1.0.1")]
    //[BepInDependency("Bones404.BrutalAPI", BepInDependency.DependencyFlags.HardDependency)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "SpecialAPI.BOSpecialItems";

        public void Awake()
        {
            SpecialItemsAssembly = Assembly.GetExecutingAssembly();

            LoadFMODBankFromResource("BOSpecialItems");
            LoadFMODBankFromResource("BOSpecialItems.strings");

            ChangeHealthColorButtonHolder.disabledSprite = LoadSprite("UI_ManaToggle_Disabled", 32);

            glossaryDB = Resources.FindObjectsOfTypeAll<GlossaryDataBase>().FirstOrDefault();
            infoHolder = Resources.FindObjectsOfTypeAll<GameInformationHolder>().FirstOrDefault();
            if(infoHolder != null)
            {
                itemPool = infoHolder.ItemPoolDB;
            }
            if(itemPool == null)
            {
                itemPool = Resources.FindObjectsOfTypeAll<ItemPoolDataBaseSO>().FirstOrDefault();
            }

            Pigments.Init();
            Passives.Init();

            new Harmony(GUID).PatchAll();

            AddNewGenericEffect<TargetSwapStatusEffect>("TargetSwap", "While TargetSwapped all abilities are performed as if the caster is on the space directly opposing them. Instant kills or fleeing effects are not affected by this.\n1 point of TargetSwap is lost at the end of each turn.", "TargetSwap", "event:/TargetSwapApply");
            AddNewGenericEffect<BerserkStatusEffect>("Berserk", "Deal double damage.\n1 point of Berserk is lost at the end of each turn.", "Berserk", "event:/FuryApply");
            AddNewGenericEffect<FuryStatusEffect>("Fury", "When performing an ability, perform it again and reduce Fury by 1 for each point of Fury.\n1 point of Fury is lost at the end of each turn.", "Fury", "event:/FuryApply");
            AddNewGenericEffect<WeakenedStatusEffect>("Weakened", "Weakened party members are 1 level lower than they would be otherwise for each point of Weakened.\nDamage dealt by Weakened enemies is multiplied by 0.85 for each point of Weakened.\n1 point of Weakened is lost at the end of each turn.", "Weaken", "event:/WeakenApply");
            AddNewGenericEffect<SurviveStatusEffect>("Survive", "Survive 1 fatal hit for each point of Survive.", "Survive", "event:/Combat/StatusEffects/SE_Divine_Apl");
            AddNewGenericEffect<PoweredUpStatusEffect>("Powered Up", "Powered Up party members are 1 level higher than they would be otherwise for each point of Powered Up.\nDamage dealt by Powered Up enemies is increased by 25% for each point of Powered Up.\n1 point of Powered Up is lost at the end of each turn.", "PoweredUp", "event:/Combat/StatusEffects/SE_Divine_Apl");
            AddNewGenericEffect<MovementChargeStatusEffect>("Movement Charge", "When a party member with Movement Charge manually moves to a new position, attempt to refresh their movemnt and decrease Movement Charge by 1.\nAll Movement Charge is loast at the end of each turn.", "MovementCharge");

            AddNewGenericEffect<JumpChargeStatusEffect>("Jump Charge", "When a party member with Jump Charge manually moves to a new position, deal 10 damage to the opposing enemy, move the left and right party members away from their new position and reduce Jump Charge by 1.", "JumpCharge", "event:/FuryApply", addToGlossary: false);
            AddNewGenericEffect<RamChargeStatusEffect>("Ram Charge", "When a party member with Ram Charge manually moves to a new position, deal 10 damage to the opposing enemy, and move it as far away from the party member's old position as possible.", "RamCharge", "event:/FuryApply", addToGlossary: false);

            CustomStoredValues.Init();
            CustomPassives.Init();
            GadgetDB.Init();

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
            ArtistsPalette.Init();
            ArtOfViolence.Init();
            RipAndTear.Init();
            ConjoinedFungi.Init();
            PetrifiedMedicine.Init();
            AllSeeingEye.Init();
            InterdimensionalShapeshifter.Init();
            AllAbilitiesAbilityItem.Init();

            Widewak.Init();

            AddGlossaryPassive("TargetShift", "All abilities performed by this party member/enemy are performed as if the caster is on the space to the right/left/far right/far left of them.", "TargetShift");
            AddGlossaryPassive("Pigment Core", "Unlocks the ability to change the colour of this party member's/enemy's health color through a button to the right of it's health bar.", "UntetheredHealthColor");
            AddGlossaryPassive("Merged", "This enemy will perform an additional ability for each enemy merged into it.", "Merged");
            AddGlossaryPassive("Shape-Shifter", "At the start of each turn, unequip this party member's held item and equip a random treasure item. Attempt to trigger that item's on combat start effects. If this passive ability was granted by an item, it will not be removed when the item is unequipped in combat.", "Shapeshifter");

            AddGlossaryKeyword("Dry Damage", "Dry damage is direct damage that doesn't generate pigment.");
            AddGlossaryKeyword("Wet Damage", "Wet damage is indirect damage that generates pigment.");
            AddGlossaryKeyword("Reliable Damage", "Reliable damage always deals the same amount of damage, regardless of any passives, items, status effects or field effects. It will still \"trigger\" effects that would normally modify damage dealt, such as reducing frail and shield or dealing damage to other enemies if the target has divine protection.");
        }
    }
}
