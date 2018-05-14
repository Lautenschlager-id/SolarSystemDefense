using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    class cBox : Component
    {
        bool HasContent = false, HasLabel = false;
        bool ToggleColorOnHover;
        cLabel label;

        public Texture2D GetContent { get; private set; }
        public Vector2 ContentRadius { get; private set; }
        public Vector2 ContentPosition { get; private set; }
        public Rectangle? ContentSourceRectangle;
        public string SourceText
        {
            get
            {
                return label?.Text;
            }
            set
            {
                if (HasLabel)
                    label.Text = value;
            }
        }
        public SpriteFont SourceFont
        {
            get
            {
                return label.SourceFont;
            }
            set
            {
                if (HasLabel)
                    label.SourceFont = value;
            }
        }
        public Color[] TextColor
        {
            get
            {
                return label?.ContentColor;
            }
            set
            {
                if (HasLabel)
                    label.ContentColor = value;
            }
        }
        public float SetLayerDepth
        {
            set
            {
                LayerDepth = value;
                if (HasLabel)
                    label.LayerDepth = LayerDepth;
            }
        }

        public cBox(int XPosition, int YPosition, int Width = 50, int Height = 50, bool ToggleColorOnHover = false)
            : base(XPosition, YPosition, Width, Height)
        {
            this.ToggleColorOnHover = ToggleColorOnHover;
        }
        public cBox(Texture2D Texture, int XPosition, int YPosition, int Width = 50, int Height = 50, bool ToggleColorOnHover = false)
            : base(XPosition, YPosition, Width, Height)
        {
            HasContent = true;
            this.ToggleColorOnHover = ToggleColorOnHover;
            ComponentColor = Color.Transparent.Collection();

            SetContent(Texture);
            SetPosition(XPosition, YPosition);
        }
        public cBox(string Text, SpriteFont LabelFont, int XPosition, int YPosition, int Width = 50, int Height = 50, bool ToggleColorOnHover = false)
            : base(XPosition, YPosition, Width, Height)
        {
            HasLabel = true;

            label = new cLabel(Text, GetDimension, LabelFont);

            this.ToggleColorOnHover = ToggleColorOnHover;
            HoverSound = Sound.MouseHover;
            ClickSound = Sound.Click;

            base.SetPosition(XPosition, YPosition);
        }

        public void SetContent(Texture2D NewContent)
        {
            if (HasContent)
            {
                GetContent = NewContent;
                ContentRadius = new Vector2(NewContent.Width, NewContent.Height) / 2f;
            }
        }

        public override void SetPosition(int XPosition, int YPosition)
        {
            base.SetPosition(XPosition, YPosition);
            if (HasContent)
                ContentPosition = new Vector2(XPosition + (Shape.Width / 2), YPosition + (Shape.Height / 2));
            else if (HasLabel)
            {
                label.SourceRectangle = GetDimension;
                label.SetPosition();
            }
        }
        public override Vector2 GetCoordinates(Rectangle Dimension, string Align, int XAxis = 0, int YAxis = 0, int Margin = 5, float Width = 1, float Height = 1)
        {
            return base.GetCoordinates(Dimension, Align, XAxis, YAxis, Margin, Shape.Width, Shape.Height);
        }
        public override Vector2 GetCoordinates(string Align, int XAxis = 0, int YAxis = 0, int Margin = 5, float Width = 1, float Height = 1)
        {
            return base.GetCoordinates(Align, XAxis, YAxis, Margin, Shape.Width, Shape.Height);
        }
        public void SetLabelPosition(string Align, int XAxis = 0, int YAxis = 0, int Margin = 5)
        {
            if (label != null)
            {
                Vector2 offset = label.GetCoordinates(Shape, Align, XAxis, YAxis, Margin);
                label.SetPosition((int)offset.X, (int)offset.Y);
            }
        }

        public override void Update()
        {
            base.Update();
            label?.Update();
            if (label != null) label.MouseHover = MouseHover;
        }

        public override void Draw(SpriteBatch Layer)
        {
            if (!Remove && Visible)
            {
                Layer.Draw(Texture, Shape, null, ComponentColor[(ToggleColorOnHover && MouseHover) ? 1 : 0] * Alpha, 0, Vector2.Zero, SpriteEffects.None, LayerDepth);
                if (HasContent)
                    Layer.Draw(GetContent, ContentPosition, ContentSourceRectangle, ContentColor[(ToggleColorOnHover && MouseHover) ? 1 : 0] * Alpha, 0, ContentRadius, 1f, 0, LayerDepth);
                label?.Draw(Layer);
            }
        }
    }
}
