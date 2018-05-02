using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    abstract class Component
    {
        public bool Visible = true, Remove = false, MouseHover = false;
        public float Alpha = 1f;
        public float LayerDepth = Info.LayerDepth["Middleground"];
        // [] = Not hover, Hover
        public Color[] ComponentColor = Color.White.Collection(), ContentColor = Color.White.Collection();

        public SoundEffect ClickSound = null, HoverSound = null;

        protected bool wasHovering = false;

        protected Texture2D Texture;
        protected Rectangle Shape, MouseShape;

        public Rectangle GetDimension
        {
            get
            {
                return Shape;
            }
        }
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
        public Vector2 Size
        {
            get
            {
                return new Vector2(Shape.Width, Shape.Height);
            }
            set
            {
                Shape.Width = (int)value.X;
                Shape.Height = (int)value.Y;
            }
        }

        public event EventHandler OnClick;
        public event EventHandler OnUpdate;
        public event EventHandler OnHover;
        public int ID { get; set; }

        protected Component(int XPosition, int YPosition, int Width, int Height)
        {
            Width = Width < 1 ? 1 : Width;
            Height = Height < 1 ? 1 : Height;

            Texture = new Texture2D(Main.Instance.GraphicsDevice, Width, Height);
            Texture.Fill(Width * Height);

            Shape = new Rectangle(XPosition, YPosition, Width, Height);
        }

        public virtual void SetPosition(int XPosition, int YPosition)
        {
            Shape.X = XPosition;
            Shape.Y = YPosition;
        }
        public virtual void SetPosition() { }

        public virtual Vector2 GetCoordinates(Rectangle Dimension, string Align, int XAxis = 0, int YAxis = 0, int Margin = Font.Margin, float Width = 1f, float Height = 1f)
        {
            if (Align.Contains("xcenter"))
                XAxis += (int)(Dimension.X + Dimension.Width / 2 - Width / 2);
            else if (Align.Contains("right"))
                XAxis += (int)(Dimension.X + Dimension.Width - Width - 5);
            else if (Align.Contains("left"))
                XAxis += Dimension.X + 5;

            if (Align.Contains("ycenter"))
                YAxis += (int)(Dimension.Y + Dimension.Height / 2 - Height / 2);
            else if (Align.Contains("top"))
                YAxis += Dimension.Y + 5;
            else if (Align.Contains("bottom"))
                YAxis += (int)(Dimension.Y + Dimension.Height - Height - 5);

            return new Vector2(XAxis, YAxis);
        }
        public virtual Vector2 GetCoordinates(string Align, int XAxis = 0, int YAxis = 0, int Margin = Font.Margin, float Width = 1f, float Height = 1f)
        {
            return GetCoordinates(Main.ViewPort.Bounds, Align, 0, 0, Margin, Width, Height);
        }

        public virtual void Update()
        {
            if (!Remove && Visible)
            {
                MouseShape = new Rectangle((int)Control.MouseCoordinates.X, (int)Control.MouseCoordinates.Y, 1, 1);
                MouseHover = MouseShape.Intersects(Shape);

                if (MouseHover)
                {
                    OnHover?.Invoke(this, null);
                    if (!wasHovering)
                        HoverSound?.Play(.25f, 0, 0);
                    if (Control.MouseClicked)
                    {
                        OnClick?.Invoke(this, null);
                        ClickSound?.Play(.3f, 0, 0);
                    }

                    wasHovering = true;
                }
                else
                    wasHovering = false;

                OnUpdate?.Invoke(this, null);
            }
        }

        public abstract void Draw(SpriteBatch Layer);
    }
}
