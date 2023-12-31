﻿using FMOD;
using FMODUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tools;
using UnityEngine;

namespace BOSpecialItems
{
    public static class Tools
    {
        public static Assembly SpecialItemsAssembly;

        public static Texture2D LoadTexture(string name)
        {
            if(TryReadFromResource(name.TryAddExtension("png"), out var ba))
            {
                var tex = new Texture2D(1, 1);
                tex.LoadImage(ba);
                tex.filterMode = FilterMode.Point;
                return tex;
            }
            return null;
        }

        public static bool TryReadFromResource(string resname, out byte[] ba)
        {
            var names = SpecialItemsAssembly.GetManifestResourceNames().Where(x => x.EndsWith($".{resname}"));
            if(names.Count() > 0)
            {
                using var strem = SpecialItemsAssembly.GetManifestResourceStream(names.First());
                ba = new byte[strem.Length];
                strem.Read(ba, 0, ba.Length);
                return true;
            }
            Debug.LogError($"Couldn't load from resource name {resname}, returning an empty byte array.");
            ba = new byte[0];
            return false;
        }

        public static Sprite LoadSprite(string name, int pixelsperunit = 1, Vector2? pivot = null)
        {
            var tex = LoadTexture(name);
            if(tex != null)
            {
                return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), pivot ?? new Vector2(0.5f, 0.5f), pixelsperunit);
            }
            return null;
        }

        public static string FormatEffectText(int dura, int restrict)
        {
            string text = "";
            if (dura > 0)
            {
                text += dura;
            }
            if (restrict > 0)
            {
                text = text + "(" + restrict + ")";
            }
            return text;
        }

        public static bool StatusDurationCanBeReduced
        {
            get
            {
                BooleanReference booleanReference = new BooleanReference(entryValue: true);
                CombatManager.Instance.ProcessImmediateAction(new CheckHasStatusFieldReductionBlockIAction(booleanReference));
                return !booleanReference.value;
            }
        }

        public static string TryAddExtension(this string n, string e)
        {
            if (n.EndsWith($".{e}"))
            {
                return n;
            }
            return n + $".{e}";
        }

        public static void LoadFMODBankFromResource(string resname, bool loadSamples = false)
        {
            if(TryReadFromResource(resname.TryAddExtension("bank"), out var ba))
            {
                LoadFMODBankFromBytes(ba, resname, loadSamples);
            }
        }

        public static void LoadFMODBankFromBytes(byte[] ba, string bankName, bool loadSamples = false)
        {
            if (RuntimeManager.Instance.loadedBanks.ContainsKey(bankName))
            {
                var loadedBank = RuntimeManager.Instance.loadedBanks[bankName];
                loadedBank.RefCount++;
                if (loadSamples)
                {
                    loadedBank.Bank.loadSampleData();
                }
                return;
            }
            RuntimeManager.LoadedBank value = default;
            var res = RuntimeManager.Instance.studioSystem.loadBankMemory(ba, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out value.Bank);
            switch (res)
            {
                case RESULT.OK:
                    value.RefCount = 1;
                    RuntimeManager.Instance.loadedBanks.Add(bankName, value);
                    if (loadSamples)
                    {
                        value.Bank.loadSampleData();
                    }
                    break;
                case RESULT.ERR_EVENT_ALREADY_LOADED:
                    value.RefCount = 2;
                    RuntimeManager.Instance.loadedBanks.Add(bankName, value);
                    break;
                default:
                    throw new BankLoadException(bankName, res);
            }
        }

        public static bool HasFlag<T>(this WearableStaticModifiers mods) where T : WearableStaticFlagPassive
        {
            return mods.ExtraPassiveAbilities.Exists(x => x is T);
        }

        public static T CreateScriptable<T>(Action<T> configure = null) where T : ScriptableObject
        {
            var s = ScriptableObject.CreateInstance<T>();
            configure?.Invoke(s);
            return s;
        }

        public static ManaColorSO[] Cost(params ManaColorSO[] colors)
        {
            return colors;
        }

        public static AbilitySO GetAnyAbility(string name)
        {
            return LoadedAssetsHandler.GetCharacterAbility(name) ?? LoadedAssetsHandler.GetEnemyAbility(name) ?? GetBossAbility(name);
        }

        public static AbilitySO GetBossAbility(string abilityName)
        {
            if (!LoadedBossAbilities.TryGetValue(abilityName, out var value))
            {
                value = LoadBossAbility(abilityName);
                LoadedBossAbilities.Add(abilityName, value);
            }
            return value;
        }

        public static AbilitySO LoadBossAbility(string abilityName)
        {
            return Resources.Load("abilities/boss/" + abilityName) as AbilitySO;
        }

        public static T[] Copy<T>(this T[] array)
        {
            return array.ToArray();
        }

        public static List<T> Copy<T>(this List<T> list)
        {
            return new(list);
        }

        public static DamageInfo ReliableDamage(this IUnit u, int amount, IUnit killer, DeathType deathType, int targetSlotOffset = -1, bool addHealthMana = true, bool directDamage = true, bool ignoresShield = false, DamageType specialDamage = DamageType.None, bool doOnBeingDamagedCall = true)
        {
            if(u is not CharacterCombat and not EnemyCombat)
            {
                return new(0, false);
            }
            var firstSlot = u.SlotID;
            var lastSlot = u.SlotID + u.Size - 1;
            if (targetSlotOffset >= 0)
            {
                targetSlotOffset = Mathf.Clamp(u.SlotID + targetSlotOffset, firstSlot, lastSlot);
                firstSlot = targetSlotOffset;
                lastSlot = targetSlotOffset;
            }
            DamageReceivedValueChangeException ex = null;
            if (doOnBeingDamagedCall)
            {
                ex = new DamageReceivedValueChangeException(amount, specialDamage, directDamage, ignoresShield, firstSlot, lastSlot);
                CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), u, ex);
                ex.GetModifiedValue();
            }
            var modifiedValue = amount;
            if (killer != null && !killer.Equals(null))
            {
                CombatManager.Instance.ProcessImmediateAction(new UnitDamagedImmediateAction(modifiedValue, killer.IsUnitCharacter));
            }
            var newHealth = Mathf.Max(u.CurrentHealth - modifiedValue, 0);
            var damageDealt = u.CurrentHealth - newHealth;
            if (damageDealt != 0)
            {
                if(u is CharacterCombat cc)
                {
                    cc.GetHit();
                    cc.CurrentHealth = newHealth;
                }
                else if(u is EnemyCombat ec)
                {
                    ec.CurrentHealth = newHealth;
                }
                if (specialDamage == DamageType.None)
                {
                    specialDamage = Utils.GetBasicDamageTypeFromAmount(modifiedValue);
                }
                if (u is CharacterCombat)
                {
                    CombatManager.Instance.AddUIAction(new CharacterDamagedUIAction(u.ID, u.CurrentHealth, u.MaximumHealth, modifiedValue, specialDamage));
                }
                else if (u is EnemyCombat)
                {
                    CombatManager.Instance.AddUIAction(new EnemyDamagedUIAction(u.ID, u.CurrentHealth, u.MaximumHealth, modifiedValue, specialDamage));
                }
                if (addHealthMana)
                {
                    CombatManager.Instance.ProcessImmediateAction(new AddManaToManaBarAction(u.HealthColor, Utils.enemyManaAmount, u.IsUnitCharacter, u.ID));
                }
                CombatManager.Instance.PostNotification(TriggerCalls.OnDamaged.ToString(), u, new IntegerReference(damageDealt));
                if (directDamage)
                {
                    CombatManager.Instance.PostNotification(TriggerCalls.OnDirectDamaged.ToString(), u, new IntegerReference(damageDealt));
                }
            }
            else if (ex == null || !ex.ShouldIgnoreUI)
            {
                if (u is CharacterCombat)
                {
                    CombatManager.Instance.AddUIAction(new CharacterNotDamagedUIAction(u.ID, DamageType.Weak));
                }
                else if (u is EnemyCombat)
                {
                    CombatManager.Instance.AddUIAction(new EnemyNotDamagedUIAction(u.ID));
                }
            }
            var killed = u.CurrentHealth == 0 && damageDealt != 0;
            if (killed)
            {
                if (u is CharacterCombat)
                {
                    CombatManager.Instance.AddSubAction(new CharacterDeathAction(u.ID, killer, deathType));
                }
                else if (u is EnemyCombat)
                {
                    CombatManager.Instance.AddSubAction(new EnemyDeathAction(u.ID, killer, deathType));
                }
            }
            return new(damageDealt, killed);
        }

        public static int RandomizeAllButColor(this ManaBar bar, ManaColorSO excludecolor, ManaColorSO[] options)
        {
            var idxes = new List<int>();
            var colors = new List<ManaColorSO>();
            var slots = bar.ManaBarSlots;
            foreach (ManaBarSlot manaBarSlot in slots)
            {
                if (!manaBarSlot.IsEmpty && manaBarSlot.ManaColor != excludecolor)
                {
                    var rng = Random.Range(0, options.Length);
                    manaBarSlot.SetMana(options[rng]);
                    idxes.Add(manaBarSlot.SlotIndex);
                    colors.Add(options[rng]);
                }
            }
            if (idxes.Count > 0)
            {
                CombatManager.Instance.AddUIAction(new ModifyManaSlotsUIAction(bar.ID, idxes.ToArray(), colors.ToArray()));
            }
            return idxes.Count;
        }

        public static CombatManagerExt Ext(this CombatManager cm)
        {
            if (cm.GetComponent<CombatManagerExt>() != null)
            {
                return cm.GetComponent<CombatManagerExt>();
            }
            return cm.gameObject.AddComponent<CombatManagerExt>();
        }

        public static IUnitExt UnitExt(this IUnit u)
        {
            if (u is CharacterCombat cc)
            {
                return cc.Ext();
            }
            else if(u is EnemyCombat ec)
            {
                return ec.Ext();
            }
            Debug.Log("What the fuck. " + u.GetType());
            return null;
        }

        public static CharacterCombatExt Ext(this CharacterCombat cc)
        {
            var ext = CombatManager.Instance.Ext();
            if (!ext.Characters.ContainsKey(cc.ID))
            {
                ext.Characters.Add(cc.ID, new(cc));
            }
            //i stg if this somehow errors
            return ext.Characters[cc.ID];
        }

        public static EnemyCombatExt Ext(this EnemyCombat cc)
        {
            var ext = CombatManager.Instance.Ext();
            if (!ext.Enemies.ContainsKey(cc.ID))
            {
                ext.Enemies.Add(cc.ID, new(cc));
            }
            return ext.Enemies[cc.ID];
        }

        public static readonly Dictionary<string, AbilitySO> LoadedBossAbilities = new();
    }
}
