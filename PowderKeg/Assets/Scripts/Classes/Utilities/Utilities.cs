using UnityEngine;

namespace Murgn.Utils
{
    public static class Utilities
    {
        public static bool RandomChance(int chance = 75) => Random.Range(0, 100) < chance;
    }
}