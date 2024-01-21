using FMODUnity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Effects
{
    public class AdvancedAnimationVisualsEffect : EffectSO
    {
        public List<AdvancedAnimationData> animations;

        public override bool PerformEffect(CombatStats stats, IUnit caster, TargetSlotInfo[] targets, bool areTargetSlots, int entryVariable, out int exitAmount)
        {
            CombatManager.Instance.AddUIAction(new AdvancedAnimationVisualsAction(animations, caster));
            exitAmount = 0;
            return true;
        }
    }

    public class AdvancedAnimationVisualsAction : CombatAction
    {
        public List<AdvancedAnimationData> animations;
        public IUnit caster;

        public AdvancedAnimationVisualsAction(List<AdvancedAnimationData> anims, IUnit cast)
        {
            animations = anims.Copy();
            caster = cast;
        }

        public override IEnumerator Execute(CombatStats stats)
        {
            var animationUI = stats.combatUI._animations;
            if (!animationUI.CanTriggerAnimations)
            {
                yield break;
            }
            while (animationUI._playingAnimation)
            {
                yield return null;
            }
            animationUI._playingAnimation = true;
            var targetsAndAnimations = animations
                .FindAll(x => x != null && x.visuals != null)
                .ConvertAll(x => (data: x, targettedSlots: x.targets?.GetTargets(stats.combatSlots, EventPatches.ChangeCasterSlotId(caster.SlotID + x.targettingOffset, caster, null, 0), EventPatches.ChangeCasterCharacter(caster.IsUnitCharacter, caster, null, 0))))
                .FindAll(x => x.data.visuals.isAnimationFullScreen || (x.targettedSlots != null && x.targettedSlots.Length > 0));
            var maxDelay = animations.Max(x => x.visuals.animation.length + x.timeDelay);
            animationUI._interruptAnimation = false;
            for(var ela = 0f; ela < maxDelay; ela += Time.deltaTime)
            {
                if (animationUI._interruptAnimation)
                {
                    animationUI.StartCoroutine(animationUI.FinalizeInterruptAnimations(maxDelay - ela));
                    yield break;
                }
                var ready = targetsAndAnimations.FindAll(x => x.data.timeDelay <= ela);
                foreach(var (r, targets) in ready)
                {
                    if (r.visuals.isAnimationFullScreen)
                    {
                        var visuals = r.visuals;
                        var aoc = MakeNewAOC(animationUI, visuals.animation);

                        AbilityActionHandler abilityActionHandler;
                        if (animationUI._idleCharacterActionHandlers.Count > 0)
                        {
                            abilityActionHandler = animationUI._idleCharacterActionHandlers[0];
                            animationUI._idleCharacterActionHandlers.RemoveAt(0);
                        }
                        else
                        {
                            abilityActionHandler = animationUI.SpawnNewActionHandler(animationUI._characterActionHandlerTemplate);
                        }
                        var animPos = animationUI._bigAnimationLocation.position;
                        var t = abilityActionHandler.transform;
                        t.position = animPos;
                        t.rotation = animationUI._bigAnimationLocation.rotation;
                        abilityActionHandler.Animator.runtimeAnimatorController = aoc;
                        abilityActionHandler.gameObject.SetActive(value: true);
                        animationUI._inUseCharacterActionHandlers.Add(abilityActionHandler);
                        if (visuals.audioReference != "" && r.playAudio)
                        {
                            RuntimeManager.PlayOneShot(visuals.audioReference, animPos);
                        }
                    }
                    else
                    {
                        var visuals = r.visuals;
                        var areTargetSlots = r.targets.AreTargetSlots;
                        var aoc = MakeNewAOC(animationUI, visuals.animation);

                        var soundPos = Vector3.zero;
                        foreach (var targetSlotInfo in targets)
                        {
                            AbilityActionHandler abilityActionHandler;
                            Vector3 position;
                            if (targetSlotInfo.IsTargetCharacterSlot)
                            {
                                if (animationUI._idleCharacterActionHandlers.Count > 0)
                                {
                                    abilityActionHandler = animationUI._idleCharacterActionHandlers[0];
                                    animationUI._idleCharacterActionHandlers.RemoveAt(0);
                                }
                                else
                                {
                                    abilityActionHandler = animationUI.SpawnNewActionHandler(animationUI._characterActionHandlerTemplate);
                                }
                                position = animationUI._characterAttackLocations[targetSlotInfo.SlotID].position;
                                if (!areTargetSlots && targetSlotInfo.HasUnit)
                                {
                                    position = animationUI._characterAttackLocations[targetSlotInfo.Unit.SlotID].position;
                                    position += (animationUI._characterAttackLocations[targetSlotInfo.Unit.SlotID + targetSlotInfo.Unit.Size - 1].position - position) * 0.5f;
                                }
                                var t = abilityActionHandler.transform;
                                t.position = position;
                                t.rotation = animationUI._characterAttackLocations[targetSlotInfo.SlotID].rotation;
                            }
                            else
                            {
                                if (animationUI._idleEnemyActionHandlers.Count > 0)
                                {
                                    abilityActionHandler = animationUI._idleEnemyActionHandlers[0];
                                    animationUI._idleEnemyActionHandlers.RemoveAt(0);
                                }
                                else
                                {
                                    abilityActionHandler = animationUI.SpawnNewActionHandler(animationUI._enemyActionHandlerTemplate);
                                }
                                position = animationUI._enemyAttackLocations[targetSlotInfo.SlotID].position;
                                if (!areTargetSlots && targetSlotInfo.HasUnit)
                                {
                                    position = animationUI._enemyAttackLocations[targetSlotInfo.Unit.SlotID].position;
                                    position += (animationUI._enemyAttackLocations[targetSlotInfo.Unit.SlotID + targetSlotInfo.Unit.Size - 1].position - position) * 0.5f;
                                }
                                var t = abilityActionHandler.transform;
                                t.position = position;
                                t.rotation = animationUI._enemyAttackLocations[targetSlotInfo.SlotID].rotation;
                            }
                            soundPos += position;
                            abilityActionHandler.Animator.runtimeAnimatorController = aoc;
                            abilityActionHandler.gameObject.SetActive(value: true);
                            if (targetSlotInfo.IsTargetCharacterSlot)
                            {
                                animationUI._inUseCharacterActionHandlers.Add(abilityActionHandler);
                            }
                            else
                            {
                                animationUI._inUseEnemyActionHandlers.Add(abilityActionHandler);
                            }
                        }
                        if (visuals.audioReference != "" && r.playAudio)
                        {
                            RuntimeManager.PlayOneShot(visuals.audioReference, soundPos / Mathf.Max(1, targets.Length));
                        }
                    }
                }
                targetsAndAnimations.RemoveAll(x => ready.Contains(x));
                yield return null;
            }
            for (int num = animationUI._inUseCharacterActionHandlers.Count - 1; num >= 0; num--)
            {
                var abilityActionHandler = animationUI._inUseCharacterActionHandlers[num];
                animationUI._inUseCharacterActionHandlers.RemoveAt(num);
                abilityActionHandler.ResetHandler();
                animationUI._idleCharacterActionHandlers.Add(abilityActionHandler);
            }
            for (int num2 = animationUI._inUseEnemyActionHandlers.Count - 1; num2 >= 0; num2--)
            {
                var abilityActionHandler = animationUI._inUseEnemyActionHandlers[num2];
                animationUI._inUseEnemyActionHandlers.RemoveAt(num2);
                abilityActionHandler.ResetHandler();
                animationUI._idleEnemyActionHandlers.Add(abilityActionHandler);
            }
            yield return animationUI._animationWaitTimer;
            animationUI._playingAnimation = false;
        }

        public static AnimatorOverrideController MakeNewAOC(CombatAnimationHandler animHandler, AnimationClip clippy)
        {
            var aoc = new AnimatorOverrideController(animHandler._characterActionHandlerTemplate.Animator.runtimeAnimatorController);
            var overrideAnims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            var animationClips = aoc.animationClips;
            foreach (AnimationClip animationClip in animationClips)
            {
                overrideAnims.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip, animationClip));
            }
            overrideAnims[0] = new(overrideAnims[0].Key, clippy);
            aoc.ApplyOverrides(overrideAnims);
            return aoc;
        }
    }

    public class AdvancedAnimationData
    {
        public AttackVisualsSO visuals;
        public BaseCombatTargettingSO targets;
        public bool playAudio = true;
        public float timeDelay;
        public int targettingOffset;
    }
}
