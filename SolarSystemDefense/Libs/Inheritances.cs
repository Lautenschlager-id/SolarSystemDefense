using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    static class Inheritances
    {
        public static float Angle(this Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }
        public static float Angle(this Vector2 v, Vector2 complement)
        {
            return (float)Math.Atan2(v.Y - complement.Y, v.X - complement.X);
        }

        public static Point toPoint(this Vector2 v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static void Fill(this Texture2D t, int size = 1)
        {
            Color[] fill = new Color[size];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.White;
            t.SetData(fill);
        }

        public static Color[] Collection(this Color color, int indexes = 2)
        {
            Color[] Out = new Color[indexes];
            for (int i = 0; i < indexes; i++)
                Out[i] = color;
            return Out;
        }
    }
}
