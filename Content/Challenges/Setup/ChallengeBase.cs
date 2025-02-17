using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public abstract class ChallengeBase
    {
        public abstract string Name { get; }
        public abstract string ID {  get; }

        public abstract string RulesText { get; }

        public abstract StartingCharacterInfo[] StartingCharacters { get; }
        public virtual string[] StartingItems => null;
        public virtual int StartingMoney => 0;

        public virtual void AttachNotifications()
        {
        }
        public virtual void ModifyStartingRun(RunDataSO run)
        {
        }
    }
}
