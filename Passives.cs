using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems
{
    public static class Passives
    {
        public static BasePassiveAbilitySO Focus;
        public static BasePassiveAbilitySO Unstable;
        public static BasePassiveAbilitySO Constricting;
        public static BasePassiveAbilitySO Formless;
        public static BasePassiveAbilitySO Pure;
        public static BasePassiveAbilitySO Absorb;
        public static BasePassiveAbilitySO Forgetful;
        public static BasePassiveAbilitySO Withering;
        public static BasePassiveAbilitySO Obscured;
        public static BasePassiveAbilitySO Confusion;
        public static BasePassiveAbilitySO Dying;
        public static BasePassiveAbilitySO Inanimate;
        public static BasePassiveAbilitySO Inferno;
        public static BasePassiveAbilitySO Enfeebled;
        public static BasePassiveAbilitySO Immortal;
        public static BasePassiveAbilitySO TwoFaced;
        public static BasePassiveAbilitySO RedEssence;
        public static BasePassiveAbilitySO YellowEssence;
        public static BasePassiveAbilitySO PurpleEssence;
        public static BasePassiveAbilitySO UntetheredEssence;
        public static BasePassiveAbilitySO Catalyst;
        public static BasePassiveAbilitySO Anchored;
        public static BasePassiveAbilitySO Delicate;
        public static BasePassiveAbilitySO Transfusion;
        public static BasePassiveAbilitySO Abomination;
        public static BasePassiveAbilitySO BronzosBlessing;
        public static BasePassiveAbilitySO Cashout;
        public static BasePassiveAbilitySO Infantile;

        private static Sprite ParentalIcon;
        private static Sprite DecayIcon;
        private static Sprite BonusAttackIcon;

        private static Dictionary<int, BasePassiveAbilitySO> Skittishes;
        private static Dictionary<int, BasePassiveAbilitySO> Slipperies;
        private static Dictionary<int, BasePassiveAbilitySO> Overexerts;
        private static Dictionary<int, BasePassiveAbilitySO> MultiAttacks;
        private static Dictionary<int, BasePassiveAbilitySO> Fleetings;
        private static Dictionary<int, BasePassiveAbilitySO> Leakies;
        private static Dictionary<int, BasePassiveAbilitySO> BoneSpurses;
        private static Dictionary<int, BasePassiveAbilitySO> Infestations;
        private static Dictionary<int, BasePassiveAbilitySO> Constructs;
        private static Dictionary<int, BasePassiveAbilitySO> Masochisms;

        private static Dictionary<int, string> XTimes;

        public static void Init()
        {
            XTimes = new()
            {
                { 1, "once" },
                { 2, "twice" },
                { 3, "thrice" },
                { 4, "four times" },
                { 5, "five times" },
                { 6, "six times" },
                { 7, "seven times" },
                { 8, "eight times" },
                { 9, "nine times" },
                { 10, "ten times" },
                { 11, "eleven times" },
                { 12, "twelve times" },
                { 13, "thirteen times" },
                { 14, "fourteen times" },
                { 15, "fifteen times" },
                { 16, "sixteen times" },
                { 17, "seventeen times" },
                { 18, "eighteen times" },
                { 19, "nineteen times" },
                { 20, "twenty times" },
            };

            Focus               = LoadedAssetsHandler.GetCharcater("Nowak_CH").passiveAbilities[0];
            Unstable            = LoadedAssetsHandler.GetEnemy("UnfinishedHeir_BOSS").passiveAbilities[3];
            Constricting        = LoadedAssetsHandler.GetCharcater("LongLiver_CH").passiveAbilities[0];
            Formless            = LoadedAssetsHandler.GetEnemy("TriggerFingers_BOSS").passiveAbilities[0];
            Pure                = LoadedAssetsHandler.GetCharcater("Cranes_CH").passiveAbilities[0];
            Absorb              = LoadedAssetsHandler.GetEnemy("Spoggle_Resonant_EN").passiveAbilities[0];
            Forgetful           = LoadedAssetsHandler.GetEnemy("Ouroboros_Head_BOSS").passiveAbilities[1];
            Withering           = LoadedAssetsHandler.GetEnemy("SilverSuckle_EN").passiveAbilities[0];
            Obscured            = LoadedAssetsHandler.GetEnemy("Ouroboros_Head_BOSS").passiveAbilities[0];
            Confusion           = LoadedAssetsHandler.GetEnemy("ManicHips_EN").passiveAbilities[0];
            Dying               = LoadedAssetsHandler.GetCharcater("Agon_CH").passiveAbilities[0];
            Inanimate           = LoadedAssetsHandler.GetCharcater("Gospel_CH").passiveAbilities[0];
            Inferno             = LoadedAssetsHandler.GetCharcater("Dimitri_CH").passiveAbilities[0];
            Enfeebled           = LoadedAssetsHandler.GetEnemy("PitifulCorpse_BOSS").passiveAbilities[1];
            Immortal            = LoadedAssetsHandler.GetCharcater("Bimini_CH").passiveAbilities[0];
            TwoFaced            = LoadedAssetsHandler.GetCharcater("Splig_CH").passiveAbilities[0];
            RedEssence          = (LoadedAssetsHandler.LoadWearable("SpringTrap_SW").staticModifiers[0] as ExtraPassiveAbility_Wearable_SMS)._extraPassiveAbility;
            YellowEssence       = (LoadedAssetsHandler.LoadWearable("FlyPaper_SW").staticModifiers[0] as ExtraPassiveAbility_Wearable_SMS)._extraPassiveAbility;
            PurpleEssence       = (LoadedAssetsHandler.LoadWearable("GlueTrap_SW").staticModifiers[0] as ExtraPassiveAbility_Wearable_SMS)._extraPassiveAbility;
            UntetheredEssence   = LoadedAssetsHandler.GetCharcater("Splig_CH").passiveAbilities[1];
            Catalyst            = LoadedAssetsHandler.GetCharcater("Rags_CH").passiveAbilities[0];
            Anchored            = LoadedAssetsHandler.GetEnemy("Mobius_BOSS").passiveAbilities[0];
            Delicate            = LoadedAssetsHandler.GetCharcater("Hans_CH").passiveAbilities[0];
            Transfusion         = LoadedAssetsHandler.GetEnemy("JumbleGuts_Clotted_EN").passiveAbilities[1];
            Abomination         = LoadedAssetsHandler.GetEnemy("UnfinishedHeir_BOSS").passiveAbilities[0];
            BronzosBlessing     = (LoadedAssetsHandler.GetEnemy("Bronzo1_EN").enterEffects[3].effect as AddPassiveEffect)._passiveToAdd;
            Cashout             = LoadedAssetsHandler.GetEnemy("Bronzo_MoneyPile_EN").passiveAbilities[0];
            Infantile           = LoadedAssetsHandler.GetEnemy("Flarblet_EN").passiveAbilities[0];

            Skittishes = new()
            {
                { 1, LoadedAssetsHandler.GetCharcater("Anton_CH").passiveAbilities[0] },
                { 3, LoadedAssetsHandler.GetEnemy("ScatteringHomunculus_EN").passiveAbilities[0] },
                { 10, LoadedAssetsHandler.GetEnemy("Bronzo5_EN").passiveAbilities[0] }
            };
            Slipperies = new()
            {
                { 1, LoadedAssetsHandler.GetEnemy("JumbleGuts_Clotted_EN").passiveAbilities[2] }
            };
            Overexerts = new()
            {
                { 1, LoadedAssetsHandler.GetEnemy("Chordophone_EN").passiveAbilities[1] },
                { 6, LoadedAssetsHandler.GetEnemy("Scrungie_EN").passiveAbilities[2] },
                { 10, LoadedAssetsHandler.GetEnemy("Bronzo3_EN").passiveAbilities[1] }
            };
            MultiAttacks = new()
            {
                { 2, LoadedAssetsHandler.GetEnemy("MunglingMudLung_EN").passiveAbilities[1] },
                { 3, LoadedAssetsHandler.GetEnemy("Chordophone_EN").passiveAbilities[0] },
                { 4, LoadedAssetsHandler.GetEnemy("Charcarrion_Alive_BOSS").passiveAbilities[1] }
            };
            Fleetings = new()
            {
                { 1, (LoadedAssetsHandler.GetCharacterAbility("Exit_1_A").effects[1].effect as AddPassiveEffect)._passiveToAdd },
                { 3, LoadedAssetsHandler.GetEnemy("Keko_EN").passiveAbilities[0] },
                { 4, LoadedAssetsHandler.GetEnemy("GigglingMinister_EN").passiveAbilities[1] }
            };
            Leakies = new()
            {
                { 1, LoadedAssetsHandler.GetCharcater("SmokeStacks_CH").passiveAbilities[0] }
            };
            BoneSpurses = new()
            {
                { 2, LoadedAssetsHandler.GetCharcater("Fennec_CH").passiveAbilities[0] },
                { 3, ((LoadedAssetsHandler.LoadWearable("GumpMingGoa_TW") as PerformEffectWearable).effects[0].effect as AddPassiveEffect)._passiveToAdd },
            };
            Infestations = new()
            {
                { 1, LoadedAssetsHandler.GetEnemy("Keko_EN").passiveAbilities[1] },
                { 2, LoadedAssetsHandler.GetEnemy("Kekastle_EN").passiveAbilities[2] }
            };
            Constructs = new()
            {
                { 1, LoadedAssetsHandler.GetCharcater("Doll_CH").passiveAbilities[0] }
            };
            Masochisms = new()
            {
                { 1, LoadedAssetsHandler.GetEnemy("ChoirBoy_EN").passiveAbilities[0] }
            };

            ParentalIcon = LoadedAssetsHandler.GetEnemy("Flarb_EN").passiveAbilities[1].passiveIcon;
            DecayIcon = LoadedAssetsHandler.GetEnemy("MudLung_EN").passiveAbilities[0].passiveIcon;
            BonusAttackIcon = LoadedAssetsHandler.GetEnemy("Xiphactinus_EN").passiveAbilities[1].passiveIcon;
        }

        private static string GetTimes(int count)
        {
            if(XTimes.TryGetValue(count, out var str))
            {
                return str;
            }
            return $"{count} times";
        }

        private static string Owner(string def, string custom)
        {
            return custom ?? def;
        }

        public static BasePassiveAbilitySO Skittish(int count = 1, string customOwner = null)
        {
            return LookForOrCreatePassive<PerformEffectPassiveAbility>(Skittishes, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Skittish ({count})";
                x.passiveIcon = Skittishes[1].passiveIcon;
                x.type = PassiveAbilityTypes.Skittish;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnAbilityUsed };
                x._characterDescription = $"Upon performing an attack {Owner("this party member", customOwner)} will attempt to move to the Left or Right {GetTimes(count)}.";
                x._enemyDescription = $"Upon performing an attack {Owner("this enemy", customOwner)} will attempt to move to the Left or Right {GetTimes(count)}.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<IsAliveEffectorCondition>(x => x.checkByCurrentHealth = true) };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = new EffectInfo[count];
                for(int i = 0; i < count; i++)
                {
                    x.effects[i] = new()
                    {
                        condition = null,
                        effect = CreateScriptable<SwapToSidesEffect>(),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    };
                }

                x.name = $"Skittish_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO Slippery(int count = 1, string customOwner = null)
        {
            return LookForOrCreatePassive<PerformEffectPassiveAbility>(Slipperies, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Slippery ({count})";
                x.passiveIcon = Slipperies[1].passiveIcon;
                x.type = PassiveAbilityTypes.Slippery;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectDamaged };
                x._characterDescription = $"Upon taking direct damage {Owner("this party member", customOwner)} will attempt to move to the Left or Right {GetTimes(count)}.";
                x._enemyDescription = $"Upon taking direct damage {Owner("this enemy", customOwner)} will attempt to move to the Left or Right {GetTimes(count)}.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<IsAliveEffectorCondition>(x => x.checkByCurrentHealth = true) };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = new EffectInfo[count];
                for (int i = 0; i < count; i++)
                {
                    x.effects[i] = new()
                    {
                        condition = null,
                        effect = CreateScriptable<SwapToSidesEffect>(),
                        entryVariable = 0,
                        targets = TargettingLibrary.ThisSlot
                    };
                }

                x.name = $"Slippery_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO Overexert(int count, string customOwner = null)
        {
            return LookForOrCreatePassive<OverexertPassiveAbility>(Overexerts, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Overexert ({count})";
                x.passiveIcon = Overexerts[1].passiveIcon;
                x.type = PassiveAbilityTypes.Overexert;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectDamaged };
                x._characterDescription = $"This passive is not meant for party members.";
                x._enemyDescription = $"Upon receiving {count} or more direct damage, cancel 1 of {Owner("this enemy's", customOwner)} actions.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<IntegerReferenceOverEqualValueEffectorCondition>(x => x.compareValue = count), CreateScriptable<IsAliveEffectorCondition>(x => x.checkByCurrentHealth = true) };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.name = $"Overexert_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO MultiAttack(int count, string customOwner = null)
        {
            return LookForOrCreatePassive<IntegerSetterPassiveAbility>(MultiAttacks, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"MultiAttack ({count})";
                x.passiveIcon = MultiAttacks[2].passiveIcon;
                x.type = PassiveAbilityTypes.MultiAttack;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.AttacksPerTurn };
                x._characterDescription = $"This passive is not meant for party members.";
                x._enemyDescription = $"{Owner("This enemy", customOwner)} will perform {count} actions each turn.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<IntegerReferenceDetectionEffectorCondition>() };
                x.doesPassiveTriggerInformationPanel = false;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.integerValue = count - 1;
                x._isItAdditive = true;

                x.name = $"MultiAttack_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO Fleeting(int count, string customOwner = null)
        {
            return LookForOrCreatePassive<FleetingPassiveAbility>(Fleetings, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Fleeting ({count})";
                x.passiveIcon = Fleetings[1].passiveIcon;
                x.type = PassiveAbilityTypes.Fleeting;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnRoundFinished };
                x._characterDescription = $"After {count} rounds {Owner("this party member", customOwner)} will flee… Coward.";
                x._enemyDescription = $"After {count} rounds {Owner("this enemy", customOwner)} will flee.";
                x.conditions = new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = false;
                x.specialStoredValue = UnitStoredValueNames.None;

                x._ignoreFirstTurn = false;
                x._turnsBeforeFleeting = count;

                x.name = $"Fleeting_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO Leaky(int count = 1, string customOwner = null)
        {
            return LookForOrCreatePassive<PerformEffectPassiveAbility>(Leakies, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Leaky ({count})";
                x.passiveIcon = Leakies[1].passiveIcon;
                x.type = PassiveAbilityTypes.Leaky;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectDamaged };
                x._characterDescription = $"Upon receiving direct damage, {Owner("this character", customOwner)} generates {count} extra pigment of its health colour.";
                x._enemyDescription = $"Upon receiving direct damage, {Owner("this enemy", customOwner)} generates {count} extra pigment of its health colour.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<HealthColorDetectionEffectorCondition>(x => { x.canGenerateMana = true; x.checkManaGeneration = true; }) };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<GenerateCasterHealthManaEffect>(),
                        entryVariable = count,
                        targets = null
                    }
                };

                x.name = $"Leaky_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO BoneSpurs(int count)
        {
            return LookForOrCreatePassive<PerformEffectPassiveAbility>(BoneSpurses, count, x =>
            {
                x._passiveName = $"Bone Spurs ({count})";
                x.passiveIcon = BoneSpurses[2].passiveIcon;
                x.type = PassiveAbilityTypes.BoneSpurs;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDirectDamaged };
                x._characterDescription = $"Deal {count} indirect damage to the Opposing enemy upon receiving direct damage.";
                x._enemyDescription = $"Deal {count} indirect damage to the Opposing party member upon receiving direct damage.";
                x.conditions = new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.BoneSpursPA;

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<DamageByStoredValueEffect>(x => { x._deathType = DeathType.Basic; x._increaseDamage = true; x._indirect = true; x._valueName = UnitStoredValueNames.BoneSpursPA; }),
                        entryVariable = count,
                        targets = TargettingLibrary.OpposingSlot
                    }
                };

                x.name = $"BoneSpurs_{count}_PA";
            }, null);
        }

        public static BasePassiveAbilitySO Infestation(int count, string customOwner = null)
        {
            return LookForOrCreatePassive<InfestationPassiveAbility>(Infestations, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Infestation ({count})";
                x.passiveIcon = Infestations[1].passiveIcon;
                x.type = PassiveAbilityTypes.Infestation;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnWillApplyDamage };
                x._characterDescription = $"{Owner("This party member", customOwner)} boosts the damage dealt by all other party members and enemies with Infestation by {count}.";
                x._enemyDescription = $"{Owner("This enemy", customOwner)} boosts the damage dealt by all other enemies and party members with Infestation by {count}.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<DamageDealtValueChangeDetectionEffectorCondition>() };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.InfestationPA;

                x.name = $"Infestation_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";

                x._addsXInfestationEnemies = count;
                x._selfDamageMultiplierPerEnemy = 1;
                x._useDealt = true;
            }, customOwner);
        }

        public static BasePassiveAbilitySO Construct(int count = 1)
        {
            return LookForOrCreatePassive<Connection_PerformEffectPassiveAbility>(Constructs, count, x =>
            {
                x._passiveName = $"Construct ({count})";
                x.passiveIcon = Constructs[1].passiveIcon;
                x.type = PassiveAbilityTypes.Construct;
                x._triggerOn = new TriggerCalls[0];
                x._characterDescription = $"Add {count} random item abilities at the beginning of combat.";
                x._enemyDescription = $"Add {count} random item abilities at the beginning of combat.";
                x.conditions = new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = false;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.immediateEffect = true;
                x.disconnectionEffects = new EffectInfo[0];
                x.connectionEffects = new EffectInfo[count];

                var extra = ((Constructs[1] as Connection_PerformEffectPassiveAbility).connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect)._extraData;
                var slap = ((Constructs[1] as Connection_PerformEffectPassiveAbility).connectionEffects[1].effect as CasterAddRandomExtraAbilityEffect)._slapData;

                for (int i = 0; i < count; i++)
                {
                    x.connectionEffects[i] = new()
                    {
                        condition = null,
                        effect = CreateScriptable<CasterAddRandomExtraAbilityEffect>(x => { x._extraData = extra; x._slapData = slap; }),
                        entryVariable = 1,
                        targets = null
                    };
                }

                x.name = $"Construct_{count}_PA";
            }, null);
        }

        public static BasePassiveAbilitySO Masochism(int count = 1, string customOwner = null)
        {
            return LookForOrCreatePassive<PerformEffectPassiveAbility>(Masochisms, count, x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = $"Masochism ({count})";
                x.passiveIcon = Masochisms[1].passiveIcon;
                x.type = PassiveAbilityTypes.Masochism;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDamaged };
                x._characterDescription = $"This Passive Ability is not meant for characters.";
                x._enemyDescription = $"Upon receiving {count} or more damage, {Owner("this enemy", customOwner)} will queue an additional ability.";
                x.conditions = new EffectorConditionSO[] { CreateScriptable<IsAliveEffectorCondition>(x => x.checkByCurrentHealth = true) };
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<AddTurnCasterToTimelineEffect>(),
                        entryVariable = 1,
                        targets = null
                    }
                };

                x.name = $"Masochism_{(hasCustomOwner ? $"Custom_{customOwner.ToCodeName()}" : count.ToString())}_PA";
            }, customOwner);
        }

        public static BasePassiveAbilitySO Parental(AbilitySO parentalAbility, string customOwner = null)
        {
            return CreateScriptable<ParentalPassiveAbility>(x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = "Parental";
                x.passiveIcon = ParentalIcon;
                x.type = PassiveAbilityTypes.Parental;
                x._triggerOn = new TriggerCalls[0];
                x._characterDescription = "This passive is not meant for party members.";
                x._enemyDescription = $"If an infantile enemy receives direct damage, {Owner("this enemy", customOwner)} will perform \"{parentalAbility._abilityName}\" in retribution.";
                x.conditions = new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = false;
                x.specialStoredValue = UnitStoredValueNames.None;

                x._parentalAbility = new ExtraAbilityInfo()
                {
                    ability = parentalAbility,
                    cost = new ManaColorSO[0],
                    rarity = CreateScriptable<RaritySO>(x =>
                    {
                        x.rarityValue = 0;
                        x.canBeRerolled = true;
                    })
                };

                x.name = $"Parental_{parentalAbility._abilityName.ToCodeName()}{(hasCustomOwner ? $"_Custom_{customOwner.ToCodeName()}" : "")}_PA";
            });
        }

        public static BasePassiveAbilitySO BonusAttack(AbilitySO attack, string customOwner = null)
        {
            return CreateScriptable<ExtraAttackPassiveAbility>(x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = attack._abilityName;
                x.passiveIcon = BonusAttackIcon;
                x.type = ExtendEnum<PassiveAbilityTypes>($"BonusAttack_{attack._abilityName.ToCodeName()}");
                x._triggerOn = new TriggerCalls[] { TriggerCalls.ExtraAdditionalAttacks };
                x._characterDescription = "This passive is not meant for party members.";
                x._enemyDescription = $"{Owner("This enemy", customOwner)} will perform \"{attack._abilityName}\" at the end of each turn as an additional attack.";
                x.conditions = new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = false;
                x.specialStoredValue = UnitStoredValueNames.None;

                x._extraAbility = new ExtraAbilityInfo()
                {
                    ability = attack,
                    cost = new ManaColorSO[0],
                    rarity = CreateScriptable<RaritySO>(x =>
                    {
                        x.rarityValue = 0;
                        x.canBeRerolled = true;
                    })
                };

                x.name = $"BonusAttack_{attack._abilityName.ToCodeName()}{(hasCustomOwner ? $"_Custom_{customOwner.ToCodeName()}" : "")}_PA";
            });
        }

        public static BasePassiveAbilitySO Decay(EnemySO enemyToSpawn, int chance, bool givesExperience = true, string customOwner = null)
        {
            return CreateScriptable<PerformEffectPassiveAbility>(x =>
            {
                var hasCustomOwner = !string.IsNullOrEmpty(customOwner);

                x._passiveName = "Decay";
                x.passiveIcon = DecayIcon;
                x.type = PassiveAbilityTypes.Decay;
                x._triggerOn = new TriggerCalls[] { TriggerCalls.OnDeath };
                x._characterDescription = $"Upon death this party member has a 34% chance of spawning a Mung.";
                x._enemyDescription = $"Upon death this enemy has a 34% chance of spawning a Mung.";
                x.conditions = chance < 100 ? new EffectorConditionSO[] { CreateScriptable<PercentageEffectorCondition>(x => x.triggerPercentage = chance) } : new EffectorConditionSO[0];
                x.doesPassiveTriggerInformationPanel = true;
                x.specialStoredValue = UnitStoredValueNames.None;

                x.effects = new EffectInfo[]
                {
                    new()
                    {
                        condition = null,
                        effect = CreateScriptable<SpawnEnemyInSlotFromEntryEffect>(x => { x.enemy = enemyToSpawn; x.givesExperience = givesExperience; })
                    }
                };

                x.name = $"Decay_{enemyToSpawn.name}{(hasCustomOwner ? $"_Custom_{customOwner.ToCodeName()}" : "")}_{chance}Percent{(givesExperience ? "" : "_NoExperience")}_PA";
            });
        }

        private static BasePassiveAbilitySO LookForOrCreatePassive<T>(Dictionary<int, BasePassiveAbilitySO> passives, int count, Action<T> configurePassive, string customOwner) where T : BasePassiveAbilitySO
        {
            var hasCustomOwner = !string.IsNullOrEmpty(customOwner);
            if(hasCustomOwner || !passives.ContainsKey(count))
            {
                var passive = CreateScriptable(configurePassive);
                if (!hasCustomOwner)
                {
                    passives[count] = passive;
                }
                return passive;
            }
            return passives[count];
        }
    }
}
