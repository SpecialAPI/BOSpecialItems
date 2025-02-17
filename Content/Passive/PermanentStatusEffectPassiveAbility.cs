using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Passive
{
    public abstract class PermanentStatusEffectPassiveAbility<T> : BasePassiveAbilitySO where T : GenericStatusEffect, new()
    {
        public override bool DoesPassiveTrigger => false;
        public override bool IsPassiveImmediate => false;

        public int amount = 1;

        public override void TriggerPassive(object sender, object args)
        {
        }

        public override void OnPassiveConnected(IUnit unit)
        {
            CombatManager.Instance.AddSubAction(new PermanentStatusEffectConnectedAction<T>(unit.ID, unit.IsUnitCharacter, amount, GetPassiveLocData().text, passiveIcon));
        }

        public override void OnPassiveDisconnected(IUnit unit)
        {
            CombatManager.Instance.AddSubAction(new PermanentStatusEffectDisconnectedAction<T>(unit.ID, unit.IsUnitCharacter, amount));
        }
    }

    public class PermanentStatusEffectConnectedAction<T>(int id, bool isCharacter, int amount, string passiveName, Sprite passiveIcon) : CombatAction where T : GenericStatusEffect, new()
    {
        public int id = id;
        public bool isCharacter = isCharacter;
        public string passiveName = passiveName;
        public Sprite passiveIcon = passiveIcon;
        public int amount = amount;

        public override IEnumerator Execute(CombatStats stats)
        {
            var unit = isCharacter ? (IUnit)stats.TryGetCharacterOnField(id) : stats.TryGetEnemyOnField(id);

            if(unit != null && unit.IsAlive)
            {
                var passiveInfo = new ShowPassiveInformationUIAction(id, isCharacter, passiveName, passiveIcon);
                yield return passiveInfo.Execute(stats);

                var effect = new T();
                effect.SetupStatus(0, Mathf.Max(amount, 1));

                unit.ApplyStatusEffect(effect, 0);
            }
        }
    }

    public class PermanentStatusEffectDisconnectedAction<T>(int id, bool isCharacter, int amount) : CombatAction where T : GenericStatusEffect
    {
        public int id = id;
        public bool isCharacter = isCharacter;
        public int amount = amount;

        public override IEnumerator Execute(CombatStats stats)
        {
            var unit = isCharacter ? (IUnit)stats.TryGetCharacterOnField(id) : stats.TryGetEnemyOnField(id);

            if (unit != null && unit.IsAlive)
            {
                for(int i = 0; i < Mathf.Max(amount, 1); i++)
                {
                    unit.DettachStatusRestrictor(GetEffectType<T>());
                }
            }

            yield break;
        }
    }
}
