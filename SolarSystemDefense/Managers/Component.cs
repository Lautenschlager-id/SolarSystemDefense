using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    abstract class Component
    {
        public Boolean Visible = true, Remove = false;
        public Boolean OnClick = false, MouseHover = false;
        public float Alpha = 1f;
        // [] = Not hover, Hover
        public Color[] ComponentColor, ContentColor;

        protected Texture2D Texture;
        protected Rectangle Shape, MouseShape;

        public int ID { get; set; }
        public Vector2 ComponentSize
        {
            get
            {
                return new Vector2(Texture.Width, Texture.Height);
            }
        }

        public event EventHandler EventOnClick;

        protected Component(int XPosition = 10, int YPosition = 10, int Width = 60, int Height = 20)
        {
            Width = Width < 0 ? 1 : Width;
            Height = Height < 0 ? 1 : Height;

            Texture = new Texture2D(Main.Instance.GraphicsDevice, Width, Height);
            Texture.Fill(Width * Height);

            Shape = new Rectangle(XPosition, YPosition, Width, Height);
        }

        public virtual void SetPosition(int XPosition, int YPosition)
        {
            Shape.X = XPosition;
            Shape.Y = YPosition;
        }

        private void TriggerOnClick()
        {
            EventOnClick?.Invoke(this, null);
        }

        public virtual void Update()
        {
            if (!Remove)
            {
                MouseShape = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
                MouseHover = MouseShape.Intersects(Shape);
                OnClick = MouseHover ? Control.MouseClicked : false;
                if (OnClick)
                    TriggerOnClick();
            }
        }

        public virtual void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth) { }
    }
}
