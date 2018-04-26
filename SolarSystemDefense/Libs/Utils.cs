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

            public bool hasValue { get; private set; }

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
        public static Texture2D CreateCircle(int radius)
        {
            int extRadius = radius * 2 + 2;
            Texture2D texture = new Texture2D(Main.Instance.GraphicsDevice, extRadius, extRadius);

            Color[] data = new Color[extRadius * extRadius];

            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            double angleStep = 1f / radius;

            int x, y;
            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                x = (int)Math.Round(radius + radius * Math.Cos(angle));
                y = (int)Math.Round(radius + radius * Math.Sin(angle));
                data[y * extRadius + x + 1] = Color.White;
            }

            texture.SetData(data);
            return texture;
        }

        public class Line
        {
            private Vector2 p1, p2;

            public Vector2 PositionA
            {
                get
                {
                    return p1;
                }
                set
                {
                    p1 = value;
                    SetValues();
                }
            }
            public Vector2 PositionB
            {
                get
                {
                    return p2;
                }
                set
                {
                    p2 = value;
                    SetValues();
                }
            }
            public Color color;
            public float alpha;

            Texture2D pixel = Graphic.Pixel;

            int border;
            Rectangle rect;
            float angle;

            public Line(Vector2 PositionA, Vector2 PositionB, int border, Color color, float alpha = 1)
            {
                p1 = PositionA;
                p2 = PositionB;
                this.border = border;
                this.color = color;
                this.alpha = alpha;
                SetValues();
            }

            public void SetValues()
            {
                angle = p2.Angle(p1);
                rect = new Rectangle((int)p1.X, (int)p1.Y, (int)Vector2.Distance(p1, p2), border);
            }

            public void Draw(SpriteBatch Depth)
            {
                Depth.Draw(pixel, rect, null, color * alpha, angle, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}
