using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    static class Utils
    {
        public sealed class AssignOnce<Type>
        {
            private Type value;

            public Boolean hasValue { get; private set; }

            public Type Value
            {
                get
                {
                    return value;
                }
                set
                {
                    if (!hasValue)
                    {
                        this.value = value;
                        hasValue = true;
                    }
                }
            }
            public Type ValueOrDefault
            {
                get
                {
                    return value;
                }
            }

            public static implicit operator Type(AssignOnce<Type> value)
            {
                return value.Value;
            }
        }

        // Textures
        public static Texture2D CreateCircle(int radius, Color borderColor, Color centerColor)
        {
            int extRadius = radius * 2 + 2;
            Texture2D texture = new Texture2D(Main.Instance.GraphicsDevice, extRadius, extRadius);

            Color[] data = new Color[extRadius * extRadius];

            for (int i = 0; i < data.Length; i++)
                data[i] = centerColor;

            double angleStep = 1f / radius;

            int x, y;
            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                x = (int)Math.Round(radius + radius * Math.Cos(angle));
                y = (int)Math.Round(radius + radius * Math.Sin(angle));
                data[y * extRadius + x + 1] = borderColor;
            }

            texture.SetData(data);
            return texture;
        }
        public static Texture2D CreateCircle(int radius, Color borderColor)
        {
            return CreateCircle(radius, borderColor, Color.Transparent);
        }
    }
}
