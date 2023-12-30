using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Status
{
    public static class GenericStatusEffectStaticMethods
    {
        public static Dictionary<Type, StatusEffectInfoSO> AddedGenericEffects = new();

        public static void AddNewGenericEffect<T>(string name, string description, string sprite, string applyEvent = "event:/UI/Combat/Status/UI_CBT_STS_Update", string updateEvent = "event:/UI/Combat/Status/UI_CBT_STS_Update", string removeEvent = "event:/UI/Combat/Status/UI_CBT_STS_Remove", bool addToGlossary = true) where T : GenericStatusEffect
        {
            var icon = LoadSprite(sprite);
            var codename = typeof(T).Name;
            var i = CreateScriptable<StatusEffectInfoSO>(x =>
            {
                x.statusEffectType = ExtendEnum<StatusEffectType>(codename);
                x._statusName = name;
                x._description = description;
                x.icon = icon;
                x._applied_SE_Event = applyEvent;
                x._updated_SE_Event = updateEvent;
                x._removed_SE_Event = removeEvent;
            });
            AddedGenericEffects[typeof(T)] = i;
            if (addToGlossary)
            {
                AddStatus(i);
            }
            AddIntent(codename, new IntentInfoBasic()
            {
                _color = new(1f, 1f, 1f, 1f),
                _sound = "",
                _sprite = icon
            });
            AddIntent($"Rem_{codename}", new IntentInfoBasic()
            {
                _color = new(0.3529f, 0.3529f, 0.3529f, 1f),
                _sound = "",
                _sprite = icon
            });
        }

        public static StatusEffectInfoSO GetInfo<T>() where T : GenericStatusEffect
        {
            if (AddedGenericEffects.TryGetValue(typeof(T), out var info))
            {
                return info;
            }
            return null;
        }

        public static StatusEffectType GetEffectType<T>() where T : GenericStatusEffect
        {
            return ExtendEnum<StatusEffectType>(typeof(T).Name);
        }

        public static IntentType GetEffectIntent<T>() where T : GenericStatusEffect
        {
            return Intent(typeof(T).Name);
        }

        public static IntentType GetEffectRemoveIntent<T>() where T : GenericStatusEffect
        {
            return Intent($"Rem_{typeof(T).Name}");
        }
    }

    public abstract class GenericStatusEffect : IStatusEffect, ITriggerEffect<IStatusEffector>
    {
        public StatusEffectInfoSO EffectInfo => AddedGenericEffects[GetType()];

        public StatusEffectType EffectType => EffectInfo.statusEffectType;

        public virtual int StatusContent { get; set; }

        public virtual int Restrictor { get; set; }

        public virtual bool CanBeRemoved => Restrictor <= 0;

        public abstract bool IsPositive { get; }

        public virtual string DisplayText => FormatEffectText(StatusContent, Restrictor);

        public void SetupStatus(int duration, int restrictors = 0)
        {
            StatusContent = duration;
            Restrictor = restrictors;
        }

        public virtual bool AddContent(IStatusEffect content)
        {
            StatusContent += content.StatusContent;
            Restrictor += content.Restrictor;
            return true;
        }

        public virtual void DettachRestrictor(IStatusEffector effector)
        {
            Restrictor--;
            if (!TryRemoveStatusEffect(effector))
            {
                effector.StatusEffectValuesChanged(EffectType, 0);
            }
        }

        public virtual int JustRemoveAllContent()
        {
            var duration = StatusContent;
            StatusContent = 0;
            return duration;
        }

        public virtual void OnSubActionTrigger(object sender, object args, bool stateCheck)
        {
        }

        public virtual void OnTriggerAttached(IStatusEffector caller)
        {
            CombatManager.Instance.AddObserver(EffectTick, TriggerCalls.OnTurnFinished.ToString(), caller);
        }

        public virtual void EffectTick(object sender, object args)
        {
            if (sender is IStatusEffector effector && StatusDurationCanBeReduced)
            {
                int duration = StatusContent;
                StatusContent = Mathf.Max(0, StatusContent - 1);
                if (!TryRemoveStatusEffect(effector) && duration != StatusContent)
                {
                    effector.StatusEffectValuesChanged(EffectType, StatusContent - duration);
                }
            }
        }

        public void ReduceContent(IStatusEffector effector, int reduceBy)
        {
            int duration = StatusContent;
            StatusContent = Mathf.Max(0, StatusContent - reduceBy);
            if (!TryRemoveStatusEffect(effector) && duration != StatusContent)
            {
                effector.StatusEffectValuesChanged(EffectType, StatusContent - duration);
            }
        }

        public virtual void OnTriggerDettached(IStatusEffector caller)
        {
            CombatManager.Instance.RemoveObserver(EffectTick, TriggerCalls.OnTurnFinished.ToString(), caller);
        }

        public void SetEffectInformation(StatusEffectInfoSO effectInfo)
        {
        }

        public virtual bool TryAddContent(int amount)
        {
            if (StatusContent <= 0)
            {
                return false;
            }
            StatusContent += amount;
            return true;
        }

        public virtual bool TryRemoveStatusEffect(IStatusEffector effector)
        {
            if (StatusContent <= 0 && CanBeRemoved)
            {
                effector.RemoveStatusEffect(EffectType);
                return true;
            }
            return false;
        }
    }
}
