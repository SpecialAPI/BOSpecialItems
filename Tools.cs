using BepInEx.Configuration;
using FMOD;
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
    [HarmonyPatch]
    public static class Tools
    {
        public static Assembly SpecialItemsAssembly;
        public static ConfigFile Config;
        public static AssetBundle Bundle;

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

        public static Sprite LoadSprite(string name, int pixelsperunit = 32, Vector2? pivot = null)
        {
            var tex = LoadTexture(name);
            if(tex != null)
            {
                return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), pivot ?? new Vector2(0.5f, 0.5f), pixelsperunit);
            }
            return null;
        }

        public static string ToCodeName(this string orig)
        {
            return orig.Replace("'", "").Replace("-", "").Replace(" ", "");
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

        public static T CreatePassive<T>(string name, string description, string sprite, Action<T> configure = null, string overrideEnum = null) where T : BasePassiveAbilitySO
        {
            var p = ScriptableObject.CreateInstance<T>();
            var codename = name.ToCodeName();

            p.name = $"{codename}_PA";
            p.type = ExtendEnum<PassiveAbilityTypes>(overrideEnum ?? codename);

            p._passiveName = name;
            p.SetDescriptions(description);

            p.passiveIcon = LoadSprite(sprite);

            configure?.Invoke(p);

            return p;
        }

        public static T CreateCharacter<T>(string name, string spriteBase, Action<T> configure = null, bool add = true) where T : CharacterSO
        {
            var c = ScriptableObject.CreateInstance<T>();
            var codename = name.ToCodeName();

            c.name = $"{codename}_CH";
            c.unitType = ExtendEnum<UnitType>(codename);

            c._characterName = name;

            configure?.Invoke(c);

            if (add)
                c.AddCharacter();

            return c;
        }

        public static TValue LookForOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> lookup, TKey k, Func<TKey, TValue> create)
        {
            if (lookup.TryGetValue(k, out var value))
            {
                value = create(k);
                lookup[k] = value;
            }

            return value;
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

        public static DamageInfo SilentDamage(this IUnit u, int amount, IUnit killer, DeathType deathType, int targetSlotOffset = -1, bool addHealthMana = true, bool directDamage = true, bool ignoresShield = false, DamageType specialDamage = DamageType.None)
        {
            if (u is not CharacterCombat and not EnemyCombat)
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
            var ex = new DamageReceivedValueChangeException(amount, specialDamage, directDamage, ignoresShield, firstSlot, lastSlot);
            CombatManager.Instance.PostNotification(TriggerCalls.OnBeingDamaged.ToString(), u, ex);
            var modifiedValue = ex.GetModifiedValue();
            if (killer != null && !killer.Equals(null))
            {
                CombatManager.Instance.ProcessImmediateAction(new UnitDamagedImmediateAction(modifiedValue, killer.IsUnitCharacter));
            }
            var newHealth = Mathf.Max(u.CurrentHealth - modifiedValue, 0);
            var damageDealt = u.CurrentHealth - newHealth;
            if (damageDealt != 0)
            {
                if (u is CharacterCombat cc)
                {
                    cc.GetHit();
                    cc.CurrentHealth = newHealth;
                }
                else if (u is EnemyCombat ec)
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

        public static int GetRandomCharacterSlotWithSize(this CombatStats stat, int size)
        {
            var slot = -1;
            var remainingSlots = new List<int>();
            for (int i = 0; i < stat.combatSlots.CharacterSlots.Length; i++)
            {
                remainingSlots.Add(i);
            }
            while (remainingSlots.Count > 0)
            {
                var idx = Random.Range(0, remainingSlots.Count);
                var slothere = remainingSlots[idx];
                remainingSlots.RemoveAt(idx);
                slot = stat.combatSlots.GetCharacterFitSlot(slothere, size);
                if (slot != -1)
                {
                    break;
                }
            }
            return slot;
        }

        public static IEnumerable MatchBefore(this ILCursor curs, Func<Instruction, bool> predicate)
        {
            for(; curs.JumpBeforeNext(predicate); curs.JumpToNext(predicate))
            {
                yield return null;
            }
        }

        public static VariableDefinition DeclareLocal<T>(this ILContext ctx)
        {
            var loc = new VariableDefinition(ctx.Import(typeof(T)));
            ctx.Body.Variables.Add(loc);

            return loc;
        }

        public static VariableDefinition DeclareLocal<T>(this ILCursor curs)
        {
            return curs.Context.DeclareLocal<T>();
        }

        public static void ForceChangeHealthColor(this IUnit u, ManaColorSO newColor)
        {
            if(u is CharacterCombat cc)
            {
                cc.HealthColor = newColor;
                CombatManager.Instance.AddUIAction(new CharacterHealthColorChangeUIAction(cc.ID, cc.HealthColor));
            }
            else if(u is EnemyCombat ec)
            {
                ec.HealthColor = newColor;
                CombatManager.Instance.AddUIAction(new EnemyHealthColorChangeUIAction(ec.ID, ec.HealthColor));
            }
        }

        public static bool Calls(this Instruction instr, MethodBase mthd)
        {
            return instr.MatchCallOrCallvirt(mthd);
        }

        public static bool JumpToNext(this ILCursor curs, Func<Instruction, bool> predicate, int times = 1)
        {
            for(int i = 0; i < times; i++)
            {
                if(!curs.TryGotoNext(MoveType.After, predicate))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool JumpBeforeNext(this ILCursor curs, Func<Instruction, bool> predicate, int times = 1)
        {
            //Debug.Log($"jump before next, curr idx {curs.Index}");
            for (int i = 0; i < times - 1; i++)
            {
                if (!curs.TryGotoNext(MoveType.After, predicate))
                {
                    return false;
                }
                //Debug.Log($"   curr idx {curs.Index}");
            }
            if(curs.TryGotoNext(MoveType.Before, predicate))
            {
                //Debug.Log($"   end {curs.Index}");
                return true;
            }
            return false;
        }

        public static void CharacterLevelSetup(this CharacterSO ch, Func<int, CharacterRankedData> levelsetup, int levels = 4)
        {
            ch.rankedData = new CharacterRankedData[levels];
            for(int i = 0; i < levels; i++)
            {
                ch.rankedData[i] = levelsetup(i);
            }
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

        public static string TrimGuid(this string str)
        {
            if (str.Contains("."))
            {
                return str.Substring(str.LastIndexOf(".") + 1);
            }
            else
            {
                return str;
            }
        }

        public static List<CombatAbility> GetAbilities(this IUnit unit)
        {
            if(unit is CharacterCombat cc)
            {
                return cc.CombatAbilities ?? new();
            }
            else if(unit is EnemyCombat ec)
            {
                return ec.Abilities ?? new();
            }
            return new();
        }
        
        public static void UpdateUIAbilities(this IUnit unit)
        {
            if(unit is CharacterCombat cc)
            {
                CombatManager.Instance.AddUIAction(new CharacterUpdateAllAttacksUIAction(cc.ID, cc.CombatAbilities.ToArray()));
            }
            else if(unit is EnemyCombat ec)
            {
                CombatManager.Instance.AddUIAction(new EnemyUpdateAllAttacksUIAction(ec.ID, ec.Abilities.ToArray()));
            }
        }

        public static Vector3 Vector3Divide(Vector3 left, Vector3 right)
        {
            return new(left.x / right.x, left.y / right.y, left.z / right.z);
        }

        public static T AddComponent<T>(this Component comp) where T : Component
        {
            if(comp == null || comp.gameObject == null)
            {
                return null;
            }
            return comp.gameObject.AddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this Component comp) where T : Component
        {
            if(comp == null || comp.gameObject == null)
            {
                return null;
            }
            return comp.gameObject.GetOrAddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if(go == null)
            {
                return null;
            }
            if(go.GetComponent<T>() != null)
            {
                return go.GetComponent<T>();
            }
            return go.AddComponent<T>();
        }

        public static bool HasAnyNotifications(string notifName, object sender)
        {
            return NtfUtils.notifications != null && NtfUtils.notifications._table != null && NtfUtils.notifications._table.ContainsKey(notifName) && NtfUtils.notifications._table[notifName] != null && NtfUtils.notifications._table[notifName].ContainsKey(sender) && NtfUtils.notifications._table[notifName][sender] != null && NtfUtils.notifications._table[notifName][sender].Count > 0;
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

        public static void SetDescriptions(this BasePassiveAbilitySO passive, string descriptionFormat)
        {
            passive._characterDescription = string.Format(descriptionFormat,
                "party member",
                "enemy",
                "party members",
                "enemies",
                "Party member",
                "Enemy",
                "Party members",
                "Enemies");
            passive._enemyDescription = string.Format(descriptionFormat,
                "enemy",
                "party member",
                "enemies",
                "party members",
                "Enemy",
                "Party member",
                "Enemies",
                "Party members");
        }

        public static void DoForEachUnit(this CombatStats stats, Action<IUnit> dowhat)
        {
            foreach(var kvp in stats.CharactersOnField)
            {
                dowhat?.Invoke(kvp.Value);
            }
            foreach(var kvp in stats.EnemiesOnField)
            {
                dowhat?.Invoke(kvp.Value);
            }
        }

        public static void DoForEachUnit(Action<IUnit> dowhat)
        {
            CombatManager.Instance._stats.DoForEachUnit(dowhat);
        }

        public static T Choose<T>(int level, params T[] values)
        {
            return values[level];
        }

        public static IntentType IntentForDamage(int damage)
        {
            if(damage <= 2)
            {
                return IntentType.Damage_1_2;
            }
            else if(damage <= 6)
            {
                return IntentType.Damage_3_6;
            }
            else if(damage <= 10)
            {
                return IntentType.Damage_7_10;
            }
            else if(damage <= 15)
            {
                return IntentType.Damage_11_15;
            }
            else if (damage <= 20)
            {
                return IntentType.Damage_16_20;
            }
            return IntentType.Damage_21;
        }

        public static bool IsOpposing(IUnit first, IUnit second)
        {
            return
                first != null &&
                second != null &&
                first.IsUnitCharacter != second.IsUnitCharacter &&
                    ((second.SlotID >= first.SlotID && second.SlotID <= first.LastSlotId()) ||
                    (second.LastSlotId() >= first.SlotID && second.LastSlotId() <= first.LastSlotId()) ||
                    (first.SlotID >= second.SlotID && first.SlotID <= second.LastSlotId()) ||
                    (first.LastSlotId() >= second.SlotID && first.LastSlotId() <= second.LastSlotId()));
        }

        public static CharacterCombat RealCharacter(this CharacterCombatUIInfo self)
        {
            return CombatManager.Instance._stats.Characters[self.ID];
        }

        public static int LastSlotId(this IUnit u)
        {
            return u.SlotID + u.Size - 1;
        }

        public static void AddCharacter(this CharacterSO ch)
        {
            LoadedAssetsHandler.LoadedCharacters[ch.name] = ch;
        }

        public static T NewItem<T>(string name, string flavor, string description, string sprite, ItemPools pools, int price = 0, bool silent = false) where T : BaseWearableSO
        {
            return CreateScriptable<T>(x =>
            {
                x._itemName = name;
                x._flavourText = flavor;
                x._description = description;
                x.wearableImage = LoadSprite(sprite);
                x.isShopItem = pools.HasFlag(ItemPools.Shop);
                x.shopPrice = price;
                x.staticModifiers = new WearableStaticModifierSetterSO[0];

                x.startsLocked = false;
                x.hasSpecialUnlock = false;
                x.usesTheOnUnlockText = false;

                var codename = name.ToCodeName();
                x.name = codename;

                if (pools.HasFlag(ItemPools.Shop))
                {
                    x.name += "_SW";

                    if(!Config.Bind("Items.Shop", codename, true, $"Whether or not the item {name} appears in shops.").Value)
                    {
                        pools &= ~ItemPools.Shop;
                    }
                }
                if (pools.HasFlag(ItemPools.Treasure))
                {
                    x.name += "_TW";

                    if (!Config.Bind("Items.Treasure", codename, true, $"Whether or not the item {name} appears in treasure chests.").Value)
                    {
                        pools &= ~ItemPools.Treasure;
                    }
                }
                if (pools.HasFlag(ItemPools.Fish))
                {
                    x.name += "_FW";

                    if (!Config.Bind("Items.Fish", codename, true, $"Whether or not the item {name} appears in the fish pool.").Value)
                    {
                        pools &= ~ItemPools.Fish;
                    }
                }
                if (pools.HasFlag(ItemPools.Extra))
                {
                    x.name += "_ExtraW";
                }

                if (pools.HasFlag(ItemPools.Shop))
                {
                    if(itemPool != null)
                    {
                        itemPool._ShopPool = itemPool._ShopPool.AddToArray(x.name);
                    }
                    else
                    {
                        shopItemsToAdd.Add(x.name);
                    }
                }
                if (pools.HasFlag(ItemPools.Treasure))
                {
                    if (itemPool != null)
                    {
                        itemPool._TreasurePool = itemPool._TreasurePool.AddToArray(x.name);
                    }
                    else
                    {
                        treasuresToAdd.Add(x.name);
                    }
                }

                if (!silent)
                {
                    Debug.Log($"Added item {x.name}");
                }

                LoadedAssetsHandler.LoadedWearables[x.name] = x;
            });
        }

        [HarmonyPatch(typeof(ItemPoolDataBaseSO), nameof(ItemPoolDataBaseSO.ShopPool), MethodType.Getter)]
        [HarmonyPatch(typeof(ItemPoolDataBaseSO), nameof(ItemPoolDataBaseSO.TreasurePool), MethodType.Getter)]
        [HarmonyPrefix]
        public static void AddItemsToPool(ItemPoolDataBaseSO __instance)
        {
            if(itemPool == null)
            {
                itemPool = __instance;

                itemPool._TreasurePool = itemPool._TreasurePool.Concat(treasuresToAdd).ToArray();
                itemPool._ShopPool = itemPool._ShopPool.Concat(shopItemsToAdd).ToArray();

                treasuresToAdd.Clear();
                shopItemsToAdd.Clear();
            }
        }

        public static BaseWearableSO GetTotallyRandomTreasure()
        {
            if (itemPool == null)
            {
                Debug.Log("itempool is null... somehow?");
                return null;
            }
            else
            {
                var items = new List<string>(itemPool.TreasurePool);
                while (items.Count > 0)
                {
                    var idx = Random.Range(0, items.Count);
                    var stuff = LoadedAssetsHandler.GetWearable(items[idx]);
                    items.RemoveAt(idx);
                    if(stuff != null && (!stuff.startsLocked || infoHolder == null || infoHolder.Game == null || infoHolder.Game.IsItemUnlocked(stuff.name)))
                    {
                        return stuff;
                    }
                }
                return null;
            }
        }

        public static ItemPoolDataBaseSO itemPool;
        public static GameInformationHolder infoHolder;

        private static readonly List<string> shopItemsToAdd = new();
        private static readonly List<string> treasuresToAdd = new();
        public static readonly Dictionary<string, AbilitySO> LoadedBossAbilities = new();
    }

    [Flags]
    public enum ItemPools
    {
        Shop = 1,
        Treasure = 2,
        Fish = 4,
        Extra = 8
    }
}
