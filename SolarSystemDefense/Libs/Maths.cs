using Microsoft.Xna.Framework;
using System;

namespace SolarSystemDefense
{
    static class Maths
    {
        public static Random Random = new Random();

        public static Vector2 PolarToVector(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
