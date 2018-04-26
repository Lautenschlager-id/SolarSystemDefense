using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    class cBox : Component
    {
        bool HasContent = false, HasLabel = false;
        bool ToggleColorOnHover;

        Texture2D ContentTexture;

        cLabel label;
        SpriteFont LabelFont;

        public Texture2D GetContent
        {
            get
            {
                return ContentTexture;
            }
        }
        public Vector2 ContentRadius { get; private set; }
        public Vector2 ContentPosition { get; private set; }
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
                return LabelFont;
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

            SetContent(Texture);
            SetPosition(XPosition, YPosition);
        }
        public cBox(string Text, int XPosition, int YPosition, int Width = 50, int Height = 50, bool ToggleColorOnHover = false)
            : base(XPosition, YPosition, Width, Height)
        {
            HasLabel = true;

            label = new cLabel(Text, GetDimension, LabelFont = Font.Text);

            this.ToggleColorOnHover = ToggleColorOnHover;

            base.SetPosition(XPosition, YPosition);
        }

        public void SetContent(Texture2D NewContent)
        {
            if (HasContent)
            {
                ContentTexture = NewContent;
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
        public void SetLabelPosition(Rectangle Dimension, string Align, int XAxis = 0, int YAxis = 0, int Margin = 5)
        {
            if (label != null)
            {
                Vector2 offset = label.GetCoordinates(Dimension, Align, XAxis, YAxis, Margin);
                label.SetPosition((int)offset.X, (int)offset.Y);
            }
        }

        public override void Update()
        {
            base.Update();
            label?.Update();
            if (label != null) label.MouseHover = MouseHover;
        }

        public override void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            if (!Remove && Visible)
            {
                MediumDepth.Draw(Texture, Shape, null, ComponentColor[(ToggleColorOnHover && MouseHover) ? 1 : 0] * Alpha);
                if (HasContent)
                    MediumDepth.Draw(ContentTexture, ContentPosition, null, ContentColor[(ToggleColorOnHover && MouseHover) ? 1 : 0] * Alpha, 0, ContentRadius, 1f, 0, 0);
                label?.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
            }
        }
    }
}
