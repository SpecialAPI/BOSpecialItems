using System;
using System.Collections.Generic;
using System.Text;

namespace BOSpecialItems.Content.Extension
{
    public class CombatManagerExt : MonoBehaviour
    {
        public Dictionary<int, EnemyCombatExt> Enemies = new();
        public Dictionary<int, CharacterCombatExt> Characters = new();
    }
}
