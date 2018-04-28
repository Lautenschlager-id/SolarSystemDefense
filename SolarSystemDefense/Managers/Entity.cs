using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    abstract class Entity
    {
        public int Type;
        public bool Visible = true;
        public Vector2 Position, Velocity;
        public float Angle, Radius = 1, Scale = 1;

        protected Texture2D Sprite;
        protected Color ObjectColor = Color.White;

        public Vector2 Size
        {
            get
            {
                return Sprite == null ? Vector2.Zero : new Vector2(Sprite.Width, Sprite.Height);
            }
        }

        public abstract void Update();
        public virtual void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            BackgroundDepth.Draw(Sprite, Position, null, ObjectColor, Angle, Size / 2f, Scale, 0, 0);
        }
    }
}
