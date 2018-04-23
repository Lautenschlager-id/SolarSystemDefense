using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    abstract class Component
    {
        public Boolean Visible = true, Remove = false, MouseHover = false;
        public float Alpha = 1f;
        // [] = Not hover, Hover
        public Color[] ComponentColor;

        protected Texture2D Texture;
        protected Rectangle Shape, MouseShape;

        public Texture2D GetComponent
        {
            get
            {
                return Texture;
            }
        }
        public Vector2 GetPosition
        {
            get
            {
                return new Vector2(Shape.X, Shape.Y);
            }
        }
        public Vector2 GetSize
        {
            get
            {
                return new Vector2(Shape.Width, Shape.Height);
            }
        }

        public event EventHandler OnClick;
        public int ID { get; set; }

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

        private void eventOnClick()
        {
            OnClick?.Invoke(this, null);
        }

        public virtual void Update()
        {
            if (!Remove)
            {
                MouseShape = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
                MouseHover = MouseShape.Intersects(Shape);

                if (MouseHover ? Control.MouseClicked : false)
                    eventOnClick();
            }
        }

        public virtual void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth) { }
    }
}
