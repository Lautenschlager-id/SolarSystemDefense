using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    class cLabel : Component
    {
        public Rectangle SourceRectangle { get; set; }

        private bool hasRectangle = false;
        private SpriteFont TextFont;
        private string TextContent = "";

        public string Text
        {
            get
            {
                return TextContent;
            }
            set
            {
                TextContent = value;
                if (hasRectangle)
                    SetPosition();
            }
        }
        public SpriteFont SourceFont
        {
            get
            {
                return TextFont;
            }
            set
            {
                TextFont = value;
                if (hasRectangle)
                    SetPosition();
            }
        }

        public cLabel(string Text, SpriteFont TextFont, int XPosition, int YPosition, int Width = 250, int Height = 100)
            : base(XPosition, YPosition, Width, Height)
        {
            TextContent = Text;
            this.TextFont = TextFont;
        }
        public cLabel(string Text, Rectangle SourceRectangle, SpriteFont TextFont)
            : base(SourceRectangle.X, SourceRectangle.Y, SourceRectangle.Width, SourceRectangle.Height)
        {
            hasRectangle = true;
            this.SourceRectangle = SourceRectangle;

            TextContent = Text;
            this.TextFont = TextFont;

            SetPosition();
        }

        public Vector2 MeasureString()
        {
            return TextFont.MeasureString(Text);
        }

        public override void SetPosition()
        {
            Vector2 rCorner = new Vector2(SourceRectangle.Left, SourceRectangle.Top);

            Vector2 rDimension = new Vector2(SourceRectangle.Width, SourceRectangle.Height);
            Vector2 sSize = MeasureString();

            Vector2 offset = (rDimension - sSize) / 2;

            Vector2 Position = rCorner + offset;
            base.SetPosition((int)Position.X, (int)Position.Y);
        }
        public override Vector2 GetCoordinates(Rectangle Dimension, string Align, int XAxis = 0, int YAxis = 0, int Margin = 5, float Width = 1, float Height = 1)
        {
            return base.GetCoordinates(Dimension, Align, XAxis, YAxis, Margin, MeasureString().X, MeasureString().Y);
        }
        public override Vector2 GetCoordinates(string Align, int XAxis = 0, int YAxis = 0, int Margin = 5, float Width = 1, float Height = 1)
        {
            return base.GetCoordinates(Align, XAxis, YAxis, Margin, MeasureString().X, MeasureString().Y);
        }

        public override void Update()
        {
            if (!hasRectangle)
                base.Update();
        }

        public override void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            if (Visible)
                ForegroundDepth.DrawString(TextFont, Text, GetPosition, ContentColor[(MouseHover ? 1 : 0)] * Alpha);
        }
    }
}
