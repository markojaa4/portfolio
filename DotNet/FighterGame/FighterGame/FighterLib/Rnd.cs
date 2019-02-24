using System;

namespace FighterLib
{
    internal static class Rnd
    {
        public static Random rnd = new Random();
        public static int Next() => rnd.Next();
        public static int Next(int maxValue) => rnd.Next(maxValue);
        public static int Next(int minValue, int maxValue) => rnd.Next(minValue, maxValue);
    }
}
