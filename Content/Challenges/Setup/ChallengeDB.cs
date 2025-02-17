using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Challenges.Setup
{
    public static class ChallengeDB
    {
        public static readonly Dictionary<string, ChallengeBase> Challenges = new();

        public static void FetchDatabase()
        {
            Challenges.Clear();
            var types = SpecialItemsAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(ChallengeBase)) && !x.IsAbstract && !x.IsInterface);

            foreach (var tp in types)
            {
                try
                {
                    var stuff = (ChallengeBase)Activator.CreateInstance(tp);

                    Challenges[stuff.ID] = stuff;
                }
                catch { }
            }
        }
    }
}
